using System;
using vCard.Net.Serialization;

namespace iCloud.Dav.People.Serialization
{
    /// <inheritdoc/>
    public class ExtendedSerializerFactory : SerializerFactory, ISerializerFactory
    {
        private readonly ISerializerFactory _mDataTypeSerializerFactory;

        /// <inheritdoc/>
        public ExtendedSerializerFactory() => _mDataTypeSerializerFactory = new ExtendedDataTypeSerializerFactory();

        /// <inheritdoc/>
        public override ISerializer Build(Type objectType, SerializationContext ctx)
        {
            ISerializer s = null;

            if (typeof(vCard.Net.DataTypes.ICardDataType).IsAssignableFrom(objectType))
            {
                s = _mDataTypeSerializerFactory.Build(objectType, ctx);
            }

            return s ?? base.Build(objectType, ctx);
        }
    }
}