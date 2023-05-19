using System;
using vCard.Net;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    /// <summary>
    /// Factory class for creating serializers for extended vCard data types and parameters.
    /// </summary>
    internal class ExtendedSerializerFactory : SerializerFactory, ISerializerFactory
    {
        private readonly ISerializerFactory _mDataTypeSerializerFactory;

        /// <summary>
        /// Initializes a new instance of the ExtendedSerializerFactory class.
        /// </summary>
        public ExtendedSerializerFactory() => _mDataTypeSerializerFactory = new ExtendedDataTypeSerializerFactory();

        /// <summary>
        /// Builds a serializer for the specified object type and serialization context.
        /// </summary>
        /// <param name="objectType">The type of object to serialize or deserialize.</param>
        /// <param name="ctx">The serialization context.</param>
        /// <returns>A serializer for the specified object type and serialization context.</returns>
        public override ISerializer Build(Type objectType, SerializationContext ctx)
        {
            ISerializer s = null;

            if (typeof(CardParameter).IsAssignableFrom(objectType))
            {
                s = new ParameterSerializer(ctx);
            }
            else if (typeof(vCard.Net.DataTypes.ICardDataType).IsAssignableFrom(objectType))
            {
                s = _mDataTypeSerializerFactory.Build(objectType, ctx);
            }

            return s ?? base.Build(objectType, ctx);
        }
    }
}