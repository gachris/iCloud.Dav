using iCloud.Dav.People.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using vCard.Net;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    /// <summary>
    /// Deserializer for vCard contact groups.
    /// </summary>
    public class ContactGroupDeserializer
    {
        private const string _nameGroup = "name";
        private const string _valueGroup = "value";
        private const string _paramNameGroup = "paramName";
        private const string _paramValueGroup = "paramValue";

        private static readonly Regex _contentLineRegex = new Regex(BuildContentLineRegex(), RegexOptions.Compiled);

        private readonly ContactGroupDataTypeMapper _dataTypeMapper;
        private readonly ISerializerFactory _serializerFactory;

        /// <summary>
        /// Gets the default instance of the <see cref="ContactGroupDeserializer"/> class.
        /// </summary>
        public static readonly ContactGroupDeserializer Default = new ContactGroupDeserializer(
            new ContactGroupDataTypeMapper(),
            new ExtendedSerializerFactory());

        /// <summary>
        /// Creates a new instance of the <see cref="ContactGroupDeserializer"/> class.
        /// </summary>
        /// <param name="dataTypeMapper">The data type mapper to use for deserialization.</param>
        /// <param name="serializerFactory">The serializer factory to use for deserialization.</param>
        internal ContactGroupDeserializer(ContactGroupDataTypeMapper dataTypeMapper, ISerializerFactory serializerFactory)
        {
            _dataTypeMapper = dataTypeMapper;
            _serializerFactory = serializerFactory;
        }

        private static string BuildContentLineRegex()
        {
            // name          = iana-token / x-name
            // iana-token    = 1*(ALPHA / DIGIT / "-")
            // x-name        = "X-" [vendorid "-"] 1*(ALPHA / DIGIT / "-")
            // vendorid      = 3*(ALPHA / DIGIT)
            // Add underscore to match behavior of bug 2033495
            const string identifier = "[-A-Za-z0-9_.]+";

            // param-value   = paramtext / quoted-string
            // paramtext     = *SAFE-CHAR
            // quoted-string = DQUOTE *QSAFE-CHAR DQUOTE
            // QSAFE-CHAR    = WSP / %x21 / %x23-7E / NON-US-ASCII
            // ; Any character except CONTROL and DQUOTE
            // SAFE-CHAR     = WSP / %x21 / %x23-2B / %x2D-39 / %x3C-7E
            //               / NON-US-ASCII
            // ; Any character except CONTROL, DQUOTE, ";", ":", ","
            var paramValue = $"((?<{_paramValueGroup}>[^\\x00-\\x08\\x0A-\\x1F\\x7F\";:,]*)|\"(?<{_paramValueGroup}>[^\\x00-\\x08\\x0A-\\x1F\\x7F\"]*)\")";

            // param         = param-name "=" param-value *("," param-value)
            // param-name    = iana-token / x-name
            var paramName = $"(?<{_paramNameGroup}>{identifier})";
            var param = $"{paramName}={paramValue}(,{paramValue})*";

            // contentline   = name *(";" param ) ":" value CRLF
            var name = $"(?<{_nameGroup}>{identifier})";
            // value         = *VALUE-CHAR
            var value = $"(?<{_valueGroup}>[^\\x00-\\x08\\x0E-\\x1F\\x7F]*)";
            var contentLine = $"^{name}(;{param})*:{value}$";
            return contentLine;
        }

        /// <summary>
        /// Deserializes a <see cref="TextReader"/> containing vCard data into a sequence of <see cref="ContactGroup"/> objects.
        /// </summary>
        /// <param name="tr">The <see cref="TextReader"/> containing the vCard data.</param>
        /// <returns>A sequence of <see cref="ContactGroup"/> objects.</returns>
        public IEnumerable<ContactGroup> Deserialize(TextReader tr)
        {
            var context = new SerializationContext();

            context.SetService(new ContactGroupDataTypeMapper());
            context.SetService(new ExtendedSerializerFactory());

            var stack = new Stack<ContactGroup>();
            var current = default(ContactGroup);
            foreach (var contentLineString in GetContentLines(tr))
            {
                var contentLine = ParseContentLine(context, contentLineString);
                if (string.Equals(contentLine.Name, "BEGIN", StringComparison.OrdinalIgnoreCase))
                {
                    stack.Push(current);
                    current = (ContactGroup)Activator.CreateInstance(typeof(ContactGroup));
                    SerializationUtil.OnDeserializing(current);
                }
                else
                {
                    if (current == null)
                    {
                        throw new SerializationException($"Expected 'BEGIN', found '{contentLine.Name}'");
                    }
                    if (string.Equals(contentLine.Name, "END", StringComparison.OrdinalIgnoreCase))
                    {
                        if (!string.Equals((string)contentLine.Value, current.Name, StringComparison.OrdinalIgnoreCase))
                        {
                            throw new SerializationException($"Expected 'END:{current.Name}', found 'END:{contentLine.Value}'");
                        }
                        SerializationUtil.OnDeserialized(current);
                        var finished = current;
                        current = stack.Pop();
                        if (current == null)
                        {
                            yield return finished;
                        }
                        else
                        {
                            current.Children.Add(finished);
                        }
                    }
                    else
                    {
                        current.Properties.Add(contentLine);
                    }
                }
            }
            if (current != null)
            {
                throw new SerializationException($"Unclosed component {current.Name}");
            }
        }

        private CardProperty ParseContentLine(SerializationContext context, string input)
        {
            var match = _contentLineRegex.Match(input);
            if (!match.Success)
            {
                throw new SerializationException($"Could not parse line: '{input}'");
            }
            var name = match.Groups[_nameGroup].Value;
            var value = match.Groups[_valueGroup].Value;
            var paramNames = match.Groups[_paramNameGroup].Captures;
            var paramValues = match.Groups[_paramValueGroup].Captures;

            var property = new CardProperty(name.ToUpperInvariant());
            context.Push(property);
            SetPropertyParameters(property, paramNames, paramValues);
            SetPropertyValue(context, property, value);
            context.Pop();
            return property;
        }

        private static void SetPropertyParameters(CardProperty property, CaptureCollection paramNames, CaptureCollection paramValues)
        {
            var paramValueIndex = 0;
            for (var paramNameIndex = 0; paramNameIndex < paramNames.Count; paramNameIndex++)
            {
                var paramName = paramNames[paramNameIndex].Value;
                var parameter = new CardParameter(paramName);
                var nextParamIndex = paramNameIndex + 1 < paramNames.Count ? paramNames[paramNameIndex + 1].Index : int.MaxValue;
                while (paramValueIndex < paramValues.Count && paramValues[paramValueIndex].Index < nextParamIndex)
                {
                    var paramValue = paramValues[paramValueIndex].Value;
                    parameter.AddValue(paramValue);
                    paramValueIndex++;
                }
                property.AddParameter(parameter);
            }
        }

        private void SetPropertyValue(SerializationContext context, CardProperty property, string value)
        {
            var type = _dataTypeMapper.GetPropertyMapping(property) ?? typeof(string);
            var serializer = (SerializerBase)_serializerFactory.Build(type, context);
            using (var valueReader = new StringReader(value))
            {
                var propertyValue = serializer.Deserialize(valueReader);
                if (propertyValue is IEnumerable<string> propertyValues)
                {
                    foreach (var singlePropertyValue in propertyValues)
                    {
                        property.AddValue(singlePropertyValue);
                    }
                }
                else
                {
                    property.AddValue(propertyValue);
                }
            }
        }

        private static IEnumerable<string> GetContentLines(TextReader reader)
        {
            var currentLine = new StringBuilder();
            while (true)
            {
                var nextLine = reader.ReadLine();
                if (nextLine == null)
                {
                    break;
                }

                if (nextLine.Length <= 0)
                {
                    continue;
                }

                if (nextLine[0] is ' ' || nextLine[0] is '\t')
                {
                    currentLine.Append(nextLine, 1, nextLine.Length - 1);
                }
                else
                {
                    if (currentLine.Length > 0)
                    {
                        yield return currentLine.ToString();
                    }
                    currentLine.Clear();
                    currentLine.Append(nextLine);
                }
            }
            if (currentLine.Length > 0)
            {
                yield return currentLine.ToString();
            }
        }
    }
}