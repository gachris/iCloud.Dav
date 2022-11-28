using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Serialization.DataTypes;
using iCloud.Dav.People.Utils;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes
{
    /// <summary>A postal address.</summary>
    public class Address : EncodableDataType, IRelatedDataType
    {
        public const string School = "_$!<School>!$_";

        /// <summary>
        /// The type of address.
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
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                var typeFromInternal = AddressTypeMapping.GetType(typeInternal);

                switch (typeFromInternal)
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

        public virtual bool IsPreferred { get; set; }

        /// <summary>
        /// This property is used to set or get the PO Box
        /// </summary>
        public string POBox { get; set; }

        /// <summary>
        /// This property is used to set or get the extended address
        /// </summary>
        public string ExtendedAddress { get; set; }

        /// <summary>
        /// This property is used to set or get the street address
        /// </summary>
        public string StreetAddress { get; set; }

        /// <summary>
        /// This property is used to set or get the locality (i.e. city)
        /// </summary>
        public string Locality { get; set; }

        /// <summary>
        /// This property is used to set or get the region (i.e. state or province)
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// This property is used to set or get the postal (zip) code
        /// </summary>
        public string PostalCode { get; set; }

        /// <summary>
        /// This property is used to set or get the country
        /// </summary>
        public string Country { get; set; }

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

        public virtual Label Label
        {
            get => Properties.Get<Label>("X-ABLABEL");
            set
            {
                if (value == null)
                {
                    Properties.Remove("X-ABLABEL");
                    var addressTypeInternal = AddressTypeMapping.GetType(AddressType.Other);
                    Parameters.Set("TYPE", addressTypeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>
        /// Returns a list of properties that are associated with the ADR object.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        public Address()
        {
            Initialize();
            Type = AddressType.Other;
        }

        public Address(string value)
        {
            Initialize();
            Type = AddressType.Other;

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new AddressSerializer();
            CopyFrom(serializer.Deserialize(new StringReader(value)) as ICopyable);
        }

        private void Initialize() => Properties = new CardDataTypePropertyList();

        protected override void OnDeserializing(StreamingContext context)
        {
            base.OnDeserializing(context);

            Initialize();
        }
    }
}