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
    public class InstantMessage : EncodableDataType, IRelatedDataType
    {
        public virtual bool IsPreferred { get; set; }

        public virtual InstantMessageServiceType ServiceType
        {
            get
            {
                _ = Parameters.Get("X-SERVICE-TYPE").TryParse<InstantMessageServiceType>(out var serviceType);
                return serviceType;
            }
            set => Parameters.Set("X-SERVICE-TYPE", value.StringArrayFlags().Select(x => x.ToLowerInvariant()));
        }

        public virtual string UserName { get; set; }

        /// <summary>
        /// The type of instant message (e.g. home, work, etc).
        /// </summary>
        public virtual InstantMessageType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<InstantMessageTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(InstantMessageTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(InstantMessageTypeInternal.Pref);
                }

                var typeFromInternal = InstantMessageTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        default:
                            typeFromInternal = InstantMessageType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = InstantMessageTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(InstantMessageTypeInternal.Pref);
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
                    case InstantMessageType.Custom:
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
                if (value == null && Label != null)
                {
                    Properties.Remove("X-ABLABEL");
                    Parameters.Remove("TYPE");
                    Parameters.Set("TYPE", InstantMessageTypeMapping.GetType(InstantMessageType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else if (value != null)
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

        public InstantMessage()
        {
            Initialize();
            Type = InstantMessageType.Other;
        }

        public InstantMessage(string value)
        {
            Initialize();
            Type = InstantMessageType.Other;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new InstantMessageSerializer();
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