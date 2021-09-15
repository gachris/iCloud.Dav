using System;
using System.IO;

namespace iCloud.Dav.Core.Serializer
{
    public interface ISerializer
    {
        /// <summary>Gets the application format this serializer supports (e.g. "json", "xml", etc.).</summary>
        string Format { get; }

        /// <summary>Serializes the specified object into a Stream.</summary>
        void Serialize(object obj, Stream target);

        /// <summary>Serializes the specified object into a string.</summary>
        string Serialize(object obj);

        /// <summary>Deserializes the string into an object.</summary>
        T Deserialize<T>(string input);

        /// <summary>Deserializes the string into an object.</summary>
        object Deserialize(string input, Type type);

        /// <summary>Deserializes the stream into an object.</summary>
        T Deserialize<T>(Stream input);
    }
}
