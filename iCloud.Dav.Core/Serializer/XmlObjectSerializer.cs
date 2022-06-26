using System;
using System.IO;
using System.Runtime.Serialization;

namespace iCloud.Dav.Core.Serializer
{
    /// <summary>Class for serialization and deserialization of JSON documents using the Newtonsoft Library.</summary>
    public class XmlObjectSerializer : ISerializer
    {
        private static XmlObjectSerializer _instance;
        private static DataContractSerializer _xmlSerializer;

        /// <summary>A singleton instance of the Newtonsoft JSON Serializer.</summary>
        public static XmlObjectSerializer Instance => _instance ??= new XmlObjectSerializer();

        public string Format => "xml";

        public void Serialize(object obj, Stream target)
        {
            if (obj == null) obj = string.Empty;

            _xmlSerializer = new DataContractSerializer(obj.GetType());
            _xmlSerializer.WriteObject(target, obj);
        }

        public string Serialize(object obj)
        {
            if (obj == null) obj = string.Empty;

            using var memoryStream = new MemoryStream();
            using var reader = new StreamReader(memoryStream);
            _xmlSerializer = new DataContractSerializer(obj.GetType());
            _xmlSerializer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;
            return reader.ReadToEnd();
        }

        public T Deserialize<T>(string input)
        {
            return (T)Deserialize(input, typeof(T));
        }

        public object Deserialize(string input, Type type)
        {
            if (string.IsNullOrEmpty(input)) return default;

            using var stream = new MemoryStream();
            var data = System.Text.Encoding.UTF8.GetBytes(input);
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            _xmlSerializer = new DataContractSerializer(type);
            return _xmlSerializer.ReadObject(stream);
        }

        public T Deserialize<T>(Stream input)
        {
            if (input == null) return default;

            _xmlSerializer = new DataContractSerializer(typeof(T));
            return (T)_xmlSerializer.ReadObject(input);
        }
    }
}
