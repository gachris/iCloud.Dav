using System;
using System.IO;
using System.Runtime.Serialization;

namespace iCloud.Dav.Core.Serialization;

/// <summary>
/// A serializer implementation that handles object serialization and deserialization to/from XML format.
/// </summary>
public class XmlObjectSerializer : ISerializer
{
    #region Fields/Consts

    private static DataContractSerializer _xmlSerializer;
    private static readonly XmlObjectSerializer _instance = new XmlObjectSerializer();

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the <see cref="XmlObjectSerializer"/>.
    /// </summary>
    public static XmlObjectSerializer Instance => _instance;

    #endregion

    /// <summary>
    /// Gets the format supported by this serializer, which is "xml" for XML format.
    /// </summary>
    public string Format => "xml";

    /// <summary>
    /// Serializes the specified object into a Stream in XML format.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="target">The target stream into which to serialize the object.</param>
    public void Serialize(object obj, Stream target)
    {
        _xmlSerializer = new DataContractSerializer(obj.GetType());
        _xmlSerializer.WriteObject(target, obj);
    }

    /// <summary>
    /// Serializes the specified object into an XML string.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <returns>A string representation of the serialized object in XML format.</returns>
    public string Serialize(object obj)
    {
        using var memoryStream = new MemoryStream();
        using var reader = new StreamReader(memoryStream);
        _xmlSerializer = new DataContractSerializer(obj.GetType());
        _xmlSerializer.WriteObject(memoryStream, obj);
        memoryStream.Position = 0;
        return reader.ReadToEnd();
    }

    /// <summary>
    /// Deserializes the XML string into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the XML string into.</typeparam>
    /// <param name="input">The XML string to be deserialized.</param>
    /// <returns>The deserialized object of type T.</returns>
    public T Deserialize<T>(string input) => (T)Deserialize(input, typeof(T));

    /// <summary>
    /// Deserializes the XML string into an object of the specified type.
    /// </summary>
    /// <param name="input">The XML string to be deserialized.</param>
    /// <param name="type">The type of object to deserialize the XML string into.</param>
    /// <returns>The deserialized object.</returns>
    public object Deserialize(string input, Type type)
    {
        using var stream = new MemoryStream();
        var data = System.Text.Encoding.UTF8.GetBytes(input);
        stream.Write(data, 0, data.Length);
        stream.Position = 0;
        _xmlSerializer = new DataContractSerializer(type);
        return _xmlSerializer.ReadObject(stream);
    }

    /// <summary>
    /// Deserializes the XML stream into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the XML stream into.</typeparam>
    /// <param name="stream">The XML stream to be deserialized.</param>
    /// <returns>The deserialized object of type T.</returns>
    public T Deserialize<T>(Stream stream)
    {
        _xmlSerializer = new DataContractSerializer(typeof(T));
        return (T)_xmlSerializer.ReadObject(stream);
    }
}