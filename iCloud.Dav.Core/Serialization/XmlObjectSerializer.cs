using iCloud.Dav.Core.Utils;
using System;
using System.IO;
using System.Runtime.Serialization;

namespace iCloud.Dav.Core.Serialization
{
    /// <summary>
    /// Class for serialization and deserialization of XML documents using the DataContractSerializer.
    /// </summary>
    public class XmlObjectSerializer : ISerializer
    {
        private static readonly XmlObjectSerializer _instance = new XmlObjectSerializer();
        private static DataContractSerializer _xmlSerializer;

        /// <summary>
        /// Gets a singleton instance of the XmlObjectSerializer.
        /// </summary>
        public static XmlObjectSerializer Instance => _instance;

        /// <summary>
        /// Gets the application format this serializer supports, which is "xml".
        /// </summary>
        public string Format => "xml";

        /// <summary>
        /// Serializes the specified object into a stream using the DataContractSerializer.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <param name="target">The target stream into which to serialize the object.</param>
        public void Serialize(object obj, Stream target)
        {
            _xmlSerializer = new DataContractSerializer(obj.GetType());
            _xmlSerializer.WriteObject(target, obj);
        }

        /// <summary>
        /// Serializes the specified object into a string using the DataContractSerializer.
        /// </summary>
        /// <param name="obj">The object to be serialized.</param>
        /// <returns>A string representation of the serialized object.</returns>
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

        /// <summary>
        /// Deserializes the string into an object of type T using the DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the string into.</typeparam>
        /// <param name="input">The string to be deserialized.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(string input) => (T)Deserialize(input, typeof(T));

        /// <summary>
        /// Deserializes the string into an object of the specified type using the DataContractSerializer.
        /// </summary>
        /// <param name="input">The string to be deserialized.</param>
        /// <param name="type">The type of object to deserialize the string into.</param>
        /// <returns>The deserialized object.</returns>
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

        /// <summary>
        /// Deserializes the stream into an object of type T using the DataContractSerializer.
        /// </summary>
        /// <typeparam name="T">The type of object to deserialize the stream into.</typeparam>
        /// <param name="stream">The stream to be deserialized.</param>
        /// <returns>The deserialized object.</returns>
        public T Deserialize<T>(Stream stream)
        {
            _xmlSerializer = new DataContractSerializer(typeof(T));
            var result = _xmlSerializer.ReadObject(stream);
            return (T)result.ThrowIfNull(nameof(result));
        }
    }
}