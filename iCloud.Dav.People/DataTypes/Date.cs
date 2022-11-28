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
    /// <summary>A date defined in a <see cref="Date"/>.</summary>
    public class Date : EncodableDataType, IRelatedDataType
    {
        public const string Anniversary = "_$!<Anniversary>!$_";

        public virtual bool IsPreferred { get; set; }

        public virtual DateTime? DateTime { get; set; }

        public virtual DateType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<DateTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(DateTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(DateTypeInternal.Pref);
                }

                var typeFromInternal = DateTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case Anniversary:
                            typeFromInternal = DateType.Anniversary;
                            break;
                        default:
                            typeFromInternal = DateType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = DateTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(DateTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                var typeFromInternal = DateTypeMapping.GetType(typeInternal);

                switch (typeFromInternal)
                {
                    case DateType.Anniversary:
                        Label = new Label() { Value = Anniversary };
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
                    var typeInternal = DateTypeMapping.GetType(DateType.Other);
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

        public Date()
        {
            Initialize();
            Type = DateType.Other;
        }

        public Date(string value)
        {
            Initialize();
            Type = DateType.Other;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new DateSerializer();
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