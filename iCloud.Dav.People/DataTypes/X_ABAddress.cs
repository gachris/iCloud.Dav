using iCloud.Dav.People.Serialization.DataTypes;
using System;
using System.IO;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents a X-ABADR value associated with a contact's address property.
    /// </summary>
    public class X_ABAddress : EncodableDataType
    {
        #region Properties

        /// <summary>
        /// Gets or sets the value of the X-ABADR.
        /// </summary>
        public virtual string Value { get; set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="X_ABAddress"/> class.
        /// </summary>
        public X_ABAddress()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="X_ABAddress"/> class with a string value.
        /// </summary>
        /// <param name="value">A string representation of the X-ABADR value.</param>
        public X_ABAddress(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return;

            var serializer = new X_ABAddressSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        #region Methods

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((X_ABAddress)obj));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
        protected bool Equals(X_ABAddress obj)
        {
            return string.Equals(Value, obj.Value, StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                return Value.GetHashCode();
            }
        }

        #endregion
    }
}