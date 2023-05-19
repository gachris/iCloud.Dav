using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Serialization.DataTypes;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>
    /// Represents a postal address that can be associated with a contact.
    /// </summary>
    public class Address : EncodableDataType, IRelatedDataType
    {
        #region Fields/Consts

        /// <summary>
        /// A constant string representing the school address type.
        /// </summary>
        public const string School = "_$!<School>!$_";

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the address is preferred.
        /// </summary>
        public virtual bool IsPreferred { get; set; }

        /// <summary>
        /// Gets or sets the type of address.
        /// </summary>
        public virtual AddressType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<AddressTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(AddressTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(AddressTypeInternal.Pref);
                }

                var typeFromInternal = AddressTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case School:
                            typeFromInternal = AddressType.School;
                            break;
                        default:
                            typeFromInternal = AddressType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = AddressTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(AddressTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Remove("TYPE");
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                switch (value)
                {
                    case AddressType.School:
                        Label = new Label() { Value = School };
                        break;
                    case AddressType.Custom:
                        Label = new Label() { Value = Label?.Value };
                        break;
                    default:
                        Label = null;
                        break;
                }
            }
        }

        /// <summary>
        /// Gets or sets the label of the address.
        /// </summary>
        public virtual Label Label
        {
            get => Properties.Get<Label>("X-ABLABEL");
            set
            {
                if (value == null && Label != null)
                {
                    Properties.Remove("X-ABLABEL");
                    Parameters.Remove("TYPE");

                    var type = AddressTypeMapping.GetType(AddressType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant());

                    Parameters.Set("TYPE", type);
                }
                else if (value != null)
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>
        /// Gets or sets the PO Box of the address.
        /// </summary>
        public string POBox { get; set; }

        /// <summary>
        /// Gets or sets the extended address of the address.
        /// </summary>
        public string ExtendedAddress { get; set; }

        /// <summary>
        /// Gets or sets the street address of the address.
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// Gets or sets the locality (i.e. city) of the address.
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// Gets or sets the region (i.e. state or province) of the address.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Gets or sets the postal (zip) code of the address.
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// Gets or sets the country of the address.
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the country code of the address.
        /// </summary>
        public virtual X_ABAddress CountryCode
        {
            get => Properties.Get<X_ABAddress>("X-ABADR");
            set
            {
                if (value == null)
                {
                    Properties.Remove("X-ABADR");
                }
                else
                {
                    Properties.Set("X-ABADR", value);
                }
            }
        }

        /// <summary>
        /// Gets the list of properties associated with the address.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class.
        /// </summary>
        public Address()
        {
            Initialize();
            Type = AddressType.Other;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Address"/> class with a string value.
        /// </summary>
        /// <param name="value">The value of the address.</param>
        public Address(string value)
        {
            Initialize();
            Type = AddressType.Other;

            if (string.IsNullOrWhiteSpace(value)) return;

            var serializer = new AddressSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        #region Methods

        /// <summary>
        /// Initializes the properties of the address.
        /// </summary>
        private void Initialize() => Properties = new CardDataTypePropertyList();

        /// <summary>
        /// This method is called during deserialization of the object, before the object is deserialized.
        /// </summary>
        /// <param name="context">The streaming context for the deserialization.</param>
        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((Address)obj));
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
        protected bool Equals(Address obj)
        {
            return Equals(IsPreferred, obj.IsPreferred) &&
                   Equals(Type, obj.Type) &&
                   Equals(Label, obj.Label) &&
                   string.Equals(POBox, obj.POBox, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(ExtendedAddress, obj.ExtendedAddress, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(StreetAddress, obj.StreetAddress, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Locality, obj.Locality, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Region, obj.Region, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(PostalCode, obj.PostalCode, StringComparison.OrdinalIgnoreCase) &&
                   string.Equals(Country, obj.Country, StringComparison.OrdinalIgnoreCase) &&
                   Equals(CountryCode, obj.CountryCode);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + IsPreferred.GetHashCode();
                hash = hash * 23 + Type.GetHashCode();
                hash = hash * 23 + (Label != null ? Label.GetHashCode() : 0);
                hash = hash * 23 + (POBox != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(POBox) : 0);
                hash = hash * 23 + (ExtendedAddress != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(ExtendedAddress) : 0);
                hash = hash * 23 + (StreetAddress != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(StreetAddress) : 0);
                hash = hash * 23 + (Locality != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Locality) : 0);
                hash = hash * 23 + (Region != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Region) : 0);
                hash = hash * 23 + (PostalCode != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(PostalCode) : 0);
                hash = hash * 23 + (Country != null ? StringComparer.OrdinalIgnoreCase.GetHashCode(Country) : 0);
                hash = hash * 23 + (CountryCode != null ? CountryCode.GetHashCode() : 0);
                return hash;
            }
        }

        #endregion
    }
}