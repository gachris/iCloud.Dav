using System;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace iCloud.Dav.Core.Serializer
{
    /// <summary>Class for serialization and deserialization of JSON documents using the Newtonsoft Library.</summary>
    public class DataContractSerializerEx : ISerializer
    {
        private static DataContractSerializer _xmlSerializer;
        private static DataContractSerializerEx _instance;

        /// <summary>A singleton instance of the Newtonsoft JSON Serializer.</summary>
        public static DataContractSerializerEx Instance
        {
            get { return _instance ??= new DataContractSerializerEx(); }
        }

        public string Format
        {
            get
            {
                return "xml";
            }
        }

        public void Serialize(object obj, Stream target)
        {
            if (obj == null)
                obj = string.Empty;

            _xmlSerializer = new DataContractSerializer(obj.GetType());
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            using var writer = XmlWriter.Create(target, settings);
            _xmlSerializer.WriteObject(writer, obj);
        }

        public string Serialize(object obj)
        {
            using var memoryStream = new MemoryStream();
            using var reader = new StreamReader(memoryStream);
            _xmlSerializer = new DataContractSerializer(obj.GetType());
            _xmlSerializer.WriteObject(memoryStream, obj);
            memoryStream.Position = 0;
            var result = reader.ReadToEnd();
            return result;
        }

        public T Deserialize<T>(string input)
        {
            return (T)Deserialize(input, typeof(T));
        }

        public object Deserialize(string input, Type type)
        {
            using var stream = new MemoryStream();
            var data = System.Text.Encoding.UTF8.GetBytes(input.Trim());
            stream.Write(data, 0, data.Length);
            stream.Position = 0;
            _xmlSerializer = new DataContractSerializer(type);
            var result = _xmlSerializer.ReadObject(stream);
            return result;
        }

        public T Deserialize<T>(Stream input)
        {
            if (input == null)
                return default;

            _xmlSerializer = new DataContractSerializer(typeof(T));
            return (T)_xmlSerializer.ReadObject(input);
        }
    }
}
