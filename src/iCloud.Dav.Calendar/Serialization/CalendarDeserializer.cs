using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.Serialization;
using iCloud.Dav.Calendar.Utils;

namespace iCloud.Dav.Calendar.Serialization;

/// <summary>
/// Deserializer for calendar components.
/// </summary>
public class CalendarDeserializer
{
    private static readonly Regex _contentLineRegex = new Regex(BuildContentLineRegex(), RegexOptions.Compiled);

    private readonly ExtendedDataTypeMapper _dataTypeMapper;
    private readonly ISerializerFactory _serializerFactory;
    private readonly CalendarComponentFactory _componentFactory;

    /// <summary>
    /// Gets the default instance of the <see cref="CalendarDeserializer"/> class.
    /// </summary>
    public static readonly CalendarDeserializer Default = new CalendarDeserializer(new ExtendedDataTypeMapper(), new SerializerFactory(), new ExtendedCalendarComponentFactory());

    /// <summary>
    /// Creates a new instance of the <see cref="CalendarDeserializer"/> class.
    /// </summary>
    /// <param name="dataTypeMapper">The data type mapper to use for deserialization.</param>
    /// <param name="serializerFactory">The serializer factory to use for deserialization.</param>
    /// <param name="componentFactory">The calendar component factory to use for deserialization.</param>
    internal CalendarDeserializer(ExtendedDataTypeMapper dataTypeMapper, ISerializerFactory serializerFactory, CalendarComponentFactory componentFactory)
    {
        _dataTypeMapper = dataTypeMapper;
        _serializerFactory = serializerFactory;
        _componentFactory = componentFactory;
    }

    private static string BuildContentLineRegex()
    {
        var text = "((?<paramValue>[^\\x00-\\x08\\x0A-\\x1F\\x7F\";:,]*)|\"(?<paramValue>[^\\x00-\\x08\\x0A-\\x1F\\x7F\"]*)\")";
        var text2 = "(?<paramName>[-A-Za-z0-9_]+)";
        var text3 = text2 + "=" + text + "(," + text + ")*";
        var text4 = "(?<name>[-A-Za-z0-9_]+)";
        var text5 = "(?<value>[^\\x00-\\x08\\x0E-\\x1F\\x7F]*)";
        return "^" + text4 + "(;" + text3 + ")*:" + text5 + "$";
    }

    /// <summary>
    /// Deserializes a <see cref="TextReader"/> containing calendar data data into a sequence of <see cref="ICalendarComponent"/> objects.
    /// </summary>
    /// <param name="reader">The <see cref="TextReader"/> containing the calendar data.</param>
    /// <returns>A sequence of <see cref="ICalendarComponent"/> objects.</returns>
    public IEnumerable<ICalendarComponent> Deserialize(TextReader reader)
    {
        SerializationContext context = new SerializationContext();
        Stack<ICalendarComponent> stack = new Stack<ICalendarComponent>();
        ICalendarComponent current = null;
        foreach (string contentLine in GetContentLines(reader))
        {
            CalendarProperty calendarProperty = ParseContentLine(context, contentLine);
            if (string.Equals(calendarProperty.Name, "BEGIN", StringComparison.OrdinalIgnoreCase))
            {
                stack.Push(current);
                current = _componentFactory.Build((string)calendarProperty.Value);
                SerializationUtil.OnDeserializing(current);
                continue;
            }

            if (current == null)
            {
                throw new SerializationException("Expected 'BEGIN', found '" + calendarProperty.Name + "'");
            }

            if (string.Equals(calendarProperty.Name, "END", StringComparison.OrdinalIgnoreCase))
            {
                if (!string.Equals((string)calendarProperty.Value, current.Name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new SerializationException($"Expected 'END:{current.Name}', found 'END:{calendarProperty.Value}'");
                }

                SerializationUtil.OnDeserialized(current);
                ICalendarComponent calendarComponent = current;
                current = stack.Pop();
                if (current == null)
                {
                    yield return calendarComponent;
                }
                else
                {
                    current.Children.Add(calendarComponent);
                }
            }
            else
            {
                current.Properties.Add(calendarProperty);
            }
        }

        if (current != null)
        {
            throw new SerializationException("Unclosed component " + current.Name);
        }
    }

    private CalendarProperty ParseContentLine(SerializationContext context, string input)
    {
        var match = _contentLineRegex.Match(input);
        if (!match.Success)
        {
            throw new SerializationException("Could not parse line: '" + input + "'");
        }

        var value = match.Groups["name"].Value;
        var value2 = match.Groups["value"].Value;
        var captures = match.Groups["paramName"].Captures;
        var captures2 = match.Groups["paramValue"].Captures;
        var calendarProperty = new CalendarProperty(value.ToUpperInvariant());
        context.Push(calendarProperty);
        SetPropertyParameters(calendarProperty, captures, captures2);
        SetPropertyValue(context, calendarProperty, value2);
        context.Pop();
        return calendarProperty;
    }

    private static void SetPropertyParameters(CalendarProperty property, CaptureCollection paramNames, CaptureCollection paramValues)
    {
        var i = 0;
        for (var j = 0; j < paramNames.Count; j++)
        {
            var value = paramNames[j].Value;
            var calendarParameter = new CalendarParameter(value);
            for (var num = j + 1 < paramNames.Count ? paramNames[j + 1].Index : int.MaxValue; i < paramValues.Count && paramValues[i].Index < num; i++)
            {
                var value2 = paramValues[i].Value;
                calendarParameter.AddValue(value2);
            }

            property.AddParameter(calendarParameter);
        }
    }

    private void SetPropertyValue(SerializationContext context, CalendarProperty property, string value)
    {
        var objectType = _dataTypeMapper.GetPropertyMapping(property) ?? typeof(string);
        var serializerBase = (SerializerBase)_serializerFactory.Build(objectType, context);
        using var tr = new StringReader(value);
        var obj = serializerBase.Deserialize(tr);
        if (obj is IEnumerable<string> enumerable)
        {
            foreach (var item in enumerable)
            {
                property.AddValue(item);
            }
        }
        else
        {
            property.AddValue(obj);
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
                continue;
            }

            if (currentLine.Length > 0)
            {
                yield return currentLine.ToString();
            }

            currentLine.Clear();
            currentLine.Append(nextLine);
        }

        if (currentLine.Length > 0)
        {
            yield return currentLine.ToString();
        }
    }
}