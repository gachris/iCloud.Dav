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
    ///     Phone information for a <see cref="Contact" />.
    /// </summary>
    /// <seealso cref="PhoneType" />
    [Serializable]
    public class Phone : EncodableDataType, IRelatedDataType
    {
        public const string AppleWatch = "APPLE WATCH";
        public const string School = "_$!<School>!$_";

        public virtual bool IsPreferred { get; set; }

        /// <summary>
        /// The full telephone number.
        /// </summary>
        public virtual string FullNumber { get; set; }

        /// <summary>
        /// The phone type.
        /// </summary>
        public virtual PhoneType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<PhoneTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(PhoneTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(PhoneTypeInternal.Pref);
                }

                var typeFromInternal = PhoneTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case School:
                            typeFromInternal = PhoneType.School;
                            break;
                        case AppleWatch:
                            typeFromInternal = PhoneType.AppleWatch;
                            break;
                        default:
                            typeFromInternal = PhoneType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = PhoneTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(PhoneTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                var typeFromInternal = PhoneTypeMapping.GetType(typeInternal);

                switch (typeFromInternal)
                {
                    case PhoneType.School:
                        Label = new Label() { Value = School };
                        break;
                    case PhoneType.AppleWatch:
                        Label = new Label() { Value = AppleWatch };
                        break;
                    case PhoneType.Custom:
                        Label = new Label() { Value = Label?.Value };
                        break;
                    default:
                        Label = null;
                        break;
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
                    var typeInternal = PhoneTypeMapping.GetType(PhoneType.Other);
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>
        /// Returns a list of properties that are associated with the TEL object.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        public Phone()
        {
            Initialize();
            Type = PhoneType.Other;
        }

        public Phone(string value)
        {
            Initialize();
            Type = PhoneType.Other;

            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new PhoneSerializer();
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