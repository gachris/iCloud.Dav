using iCloud.Dav.People.DataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using vCard.Net;
using vCard.Net.DataTypes;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    public class ContactSerializer : ComponentSerializer
    {
        public ContactSerializer()
        {
            SetService(new ContactDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public ContactSerializer(SerializationContext ctx) : base(ctx)
        {
            SetService(new ContactDataTypeMapper());
            SetService(new ExtendedSerializerFactory());
        }

        public override Type TargetType => typeof(Contact);

        public override string SerializeToString(object obj)
        {
            if (!(obj is Contact c))
            {
                return null;
            }

            var sb = new StringBuilder();
            var upperName = c.Name.ToUpperInvariant();
            sb.Append(TextUtil.FoldLines($"BEGIN:{upperName}"));
            sb.Append(TextUtil.FoldLines($"VERSION:3.0"));

            // Get a serializer factory
            var sf = GetService<ISerializerFactory>();

            // Sort the vCard properties in alphabetical order before serializing them!
            var properties = c.Properties.OrderBy(p => p, PropertyComparer.Default).ToList();

            var index = 0;

            // Serialize properties
            foreach (var p in properties)
            {
                // Get a serializer for each property.
                var serializer = sf.Build(p.GetType(), SerializationContext) as IStringSerializer;

                if (p?.Values == null || !p.Values.Any())
                {
                    continue;
                }

                // Push this object on the serialization context.
                SerializationContext.Push(p);

                var result = new StringBuilder();
                foreach (var v in p.Values.Where(value => value != null))
                {
                    // Get a serializer to serialize the property's value.
                    // If we can't serialize the property's value, the next step is worthless anyway.
                    var valueSerializer = sf.Build(v.GetType(), SerializationContext) as IStringSerializer;

                    // Iterate through each value to be serialized,
                    // and give it a property (with parameters).
                    // FIXME: this isn't always the way this is accomplished.
                    // Multiple values can often be serialized within the
                    // same property.  How should we fix this?

                    // NOTE:
                    // We Serialize the property's value first, as during 
                    // serialization it may modify our parameters.
                    // FIXME: the "parameter modification" operation should
                    // be separated from serialization. Perhaps something
                    // like PreSerialize(), etc.
                    var value = valueSerializer.SerializeToString(v);

                    // Get the list of parameters we'll be serializing
                    var parameterList = p.Parameters;
                    if (v is ICardDataType)
                    {
                        parameterList = (v as ICardDataType).Parameters;
                    }

                    var stringBuilder = new StringBuilder();
                    if (v is IRelatedDataType dataType && dataType.Properties.Any(x => x.Values.Any()))
                    {
                        stringBuilder.Append($"item{++index}.");
                    }
                    stringBuilder.Append(p.Name);
                    if (parameterList.Any())
                    {
                        // Get a serializer for parameters
                        var parameterSerializer = sf.Build(typeof(CardParameter), SerializationContext) as IStringSerializer;
                        if (parameterSerializer != null)
                        {
                            // Serialize each parameter
                            // Separate parameters with semicolons
                            stringBuilder.Append(";");
                            stringBuilder.Append(string.Join(";", parameterList.Select(param => parameterSerializer.SerializeToString(param))));
                        }
                    }
                    stringBuilder.Append(":");
                    stringBuilder.Append(value);

                    result.Append(TextUtil.FoldLines(stringBuilder.ToString()));

                    if (v is IRelatedDataType relatedDataType)
                    {
                        foreach (var property in relatedDataType.Properties)
                        {
                            var sb2 = new StringBuilder();
                            // Get a serializer for each property.
                            var serializer2 = sf.Build(property.GetType(), SerializationContext) as IStringSerializer;
                            sb2.Append($"item{index}.");
                            sb2.Append(serializer2.SerializeToString(property));

                            result.Append(TextUtil.FoldLines(sb2.ToString()));
                        }
                    }

                }

                // Pop the object off the serialization context.
                SerializationContext.Pop();
                sb.Append(result.ToString());
            }

            // Serialize child objects
            foreach (var child in c.Children)
            {
                // Get a serializer for each child object.
                var serializer = sf.Build(child.GetType(), SerializationContext) as IStringSerializer;
                sb.Append(serializer.SerializeToString(child));
            }

            sb.Append(TextUtil.FoldLines($"END:{upperName}"));
            return sb.ToString();
        }

        public override object Deserialize(TextReader tr) => null;

        public class PropertyComparer : IComparer<ICardProperty>
        {
            public static PropertyComparer Default = new PropertyComparer();

            public int Compare(ICardProperty x, ICardProperty y)
            {
                if (x == y)
                {
                    return 0;
                }
                return x == null
                    ? -1
                    : y == null
                    ? 1
                    : string.Compare(x.Name, y.Name, StringComparison.OrdinalIgnoreCase);
            }
        }
    }

    internal static class TextUtil
    {
        /// <summary> Folds lines at 75 characters, and prepends the next line with a space per RFC https://tools.ietf.org/html/rfc5545#section-3.1 </summary>
        public static string FoldLines(string incoming)
        {
            //The spec says nothing about trimming, but it seems reasonable...
            var trimmed = incoming.Trim();
            if (trimmed.Length <= 75)
            {
                return trimmed + SerializationConstants.LineBreak;
            }

            const int takeLimit = 74;

            var firstLine = trimmed.Substring(0, takeLimit);
            var remainder = trimmed.Substring(takeLimit, trimmed.Length - takeLimit);

            var chunkedRemainder = string.Join(SerializationConstants.LineBreak + " ", Chunk(remainder));
            return firstLine + SerializationConstants.LineBreak + " " + chunkedRemainder + SerializationConstants.LineBreak;
        }

        public static IEnumerable<string> Chunk(string str, int chunkSize = 73)
        {
            for (var index = 0; index < str.Length; index += chunkSize)
            {
                yield return str.Substring(index, Math.Min(chunkSize, str.Length - index));
            }
        }
    }
}