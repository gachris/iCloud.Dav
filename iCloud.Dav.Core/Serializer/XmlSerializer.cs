using System;
using System.IO;
using System.Xml;

namespace iCloud.Dav.Core.Serializer
{
    /// <summary>Class for serialization and deserialization of JSON documents using the Newtonsoft Library.</summary>
    public class XmlSerializer : ISerializer
    {
        private static System.Xml.Serialization.XmlSerializer _xmlSerializer;
        private static XmlSerializer _instance;

        /// <summary>A singleton instance of the Newtonsoft JSON Serializer.</summary>
        public static XmlSerializer Instance
        {
            get { return _instance = _instance ?? new XmlSerializer(); }
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

            _xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
            using (XmlWriter writer = XmlWriter.Create(target, settings))
            {
                _xmlSerializer.Serialize(writer, obj);
            }
        }

        public string Serialize(object obj)
        {
            if (obj == null)
                obj = string.Empty;

            _xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
            using (var stringWriter = new StringWriter())
            {
                var settings = new XmlWriterSettings { Indent = true, OmitXmlDeclaration = true };
                using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
                {
                    _xmlSerializer.Serialize(writer, obj);
                    return stringWriter.ToString();
                }
            }
        }

        public T Deserialize<T>(string input)
        {
            return (T)this.Deserialize(input, typeof(T));
        }

        public object Deserialize(string input, Type type)
        {
            if (string.IsNullOrEmpty(input))
                return default;

            _xmlSerializer = new System.Xml.Serialization.XmlSerializer(type);
            using (StringReader reader = new StringReader(input))
            {
                return _xmlSerializer.Deserialize(reader);
            }
        }

        public T Deserialize<T>(Stream input)
        {
            if (input == null)
                return default;

            _xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
            return (T)_xmlSerializer.Deserialize(input);
        }
    }
}
