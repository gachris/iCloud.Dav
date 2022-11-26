using iCloud.Dav.Core.Utils;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace iCloud.Dav.Core.Serialization
{
    /// <summary>Class for serialization and deserialization of JSON documents using the Newtonsoft Library.</summary>
    public class XmlObjectSerializer : ISerializer
    {
        private static readonly XmlObjectSerializer _instance = new XmlObjectSerializer();
        private static DataContractSerializer _xmlSerializer;

        /// <summary>A singleton instance of the Newtonsoft JSON Serializer.</summary>
        public static XmlObjectSerializer Instance => _instance;

        public string Format => "xml";

        public void Serialize(object obj, Stream target)
        {
            _xmlSerializer = new DataContractSerializer(obj.GetType());
            _xmlSerializer.WriteObject(target, obj);
        }

        public string Serialize(object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var reader = new StreamReader(memoryStream))
                {
                    _xmlSerializer = new DataContractSerializer(obj.GetType());
                    _xmlSerializer.WriteObject(memoryStream, obj);
                    memoryStream.Position = 0;
                    return reader.ReadToEnd();
                }
            }
        }

        public T Deserialize<T>(string input) => (T)Deserialize(input, typeof(T));

        public object Deserialize(string input, Type type)
        {
            using (var stream = new MemoryStream())
            {
                var data = System.Text.Encoding.UTF8.GetBytes(input);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                _xmlSerializer = new DataContractSerializer(type);
                var result = _xmlSerializer.ReadObject(stream);
                return result.ThrowIfNull(nameof(result));
            }
        }

        public T Deserialize<T>(Stream stream)
        {
            _xmlSerializer = new DataContractSerializer(typeof(T));
            var result = _xmlSerializer.ReadObject(stream);
            return (T)result.ThrowIfNull(nameof(result));
        }
    }
}