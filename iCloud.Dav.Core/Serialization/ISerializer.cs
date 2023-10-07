using System;
using System.IO;

namespace iCloud.Dav.Core.Serialization;

/// <summary>
/// Defines an interface for serializing and deserializing objects to and from different formats.
/// </summary>
public interface ISerializer
{
    /// <summary>
    /// Gets the application format this serializer supports (e.g. "json", "xml", etc.).
    /// </summary>
    string Format { get; }

    /// <summary>
    /// Serializes the specified object into a Stream.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <param name="target">The target stream into which to serialize the object.</param>
    void Serialize(object obj, Stream target);

    /// <summary>
    /// Serializes the specified object into a string.
    /// </summary>
    /// <param name="obj">The object to be serialized.</param>
    /// <returns>A string representation of the serialized object.</returns>
    string Serialize(object obj);

    /// <summary>
    /// Deserializes the string into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the string into.</typeparam>
    /// <param name="input">The string to be deserialized.</param>
    /// <returns>The deserialized object.</returns>
    T Deserialize<T>(string input);

    /// <summary>
    /// Deserializes the string into an object of the specified type.
    /// </summary>
    /// <param name="input">The string to be deserialized.</param>
    /// <param name="type">The type of object to deserialize the string into.</param>
    /// <returns>The deserialized object.</returns>
    object Deserialize(string input, Type type);

    /// <summary>
    /// Deserializes the stream into an object of type T.
    /// </summary>
    /// <typeparam name="T">The type of object to deserialize the stream into.</typeparam>
    /// <param name="input">The stream to be deserialized.</param>
    /// <returns>The deserialized object.</returns>
    T Deserialize<T>(Stream input);
}