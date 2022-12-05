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
    /// <summary>
    /// Represents an RFC 6350 "X-ABRELATEDNAMES" extended value.
    /// </summary>
    public class RelatedNames : EncodableDataType, IRelatedDataType
    {
        public const string Father = "_$!<Father>!$_";
        public const string Mother = "_$!<Mother>!$_";
        public const string Parent = "_$!<Parent>!$_";
        public const string Brother = "_$!<Brother>!$_";
        public const string Sister = "_$!<Sister>!$_";
        public const string Child = "_$!<Child>!$_";
        public const string Friend = "_$!<Friend>!$_";
        public const string Spouse = "_$!<Spouse>!$_";
        public const string Partner = "_$!<Partner>!$_";
        public const string Assistant = "_$!<Assistant>!$_";
        public const string Manager = "_$!<Manager>!$_";

        public virtual bool IsPreferred { get; set; }

        public virtual string Name { get; set; }

        /// <summary>The related person type.</summary>
        public virtual RelatedNamesType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<RelatedNamesTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(RelatedNamesTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(RelatedNamesTypeInternal.Pref);
                }

                var typeFromInternal = RelatedNamesMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case Father:
                            typeFromInternal = RelatedNamesType.Father;
                            break;
                        case Mother:
                            typeFromInternal = RelatedNamesType.Mother;
                            break;
                        case Parent:
                            typeFromInternal = RelatedNamesType.Parent;
                            break;
                        case Brother:
                            typeFromInternal = RelatedNamesType.Brother;
                            break;
                        case Sister:
                            typeFromInternal = RelatedNamesType.Sister;
                            break;
                        case Child:
                            typeFromInternal = RelatedNamesType.Child;
                            break;
                        case Friend:
                            typeFromInternal = RelatedNamesType.Friend;
                            break;
                        case Spouse:
                            typeFromInternal = RelatedNamesType.Spouse;
                            break;
                        case Partner:
                            typeFromInternal = RelatedNamesType.Partner;
                            break;
                        case Assistant:
                            typeFromInternal = RelatedNamesType.Assistant;
                            break;
                        case Manager:
                            typeFromInternal = RelatedNamesType.Manager;
                            break;
                        default:
                            typeFromInternal = RelatedNamesType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = RelatedNamesMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(RelatedNamesTypeInternal.Pref);
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
                    case RelatedNamesType.Father:
                        Label = new Label() { Value = Father };
                        break;
                    case RelatedNamesType.Mother:
                        Label = new Label() { Value = Mother };
                        break;
                    case RelatedNamesType.Parent:
                        Label = new Label() { Value = Parent };
                        break;
                    case RelatedNamesType.Brother:
                        Label = new Label() { Value = Brother };
                        break;
                    case RelatedNamesType.Sister:
                        Label = new Label() { Value = Sister };
                        break;
                    case RelatedNamesType.Child:
                        Label = new Label() { Value = Child };
                        break;
                    case RelatedNamesType.Friend:
                        Label = new Label() { Value = Friend };
                        break;
                    case RelatedNamesType.Spouse:
                        Label = new Label() { Value = Spouse };
                        break;
                    case RelatedNamesType.Partner:
                        Label = new Label() { Value = Partner };
                        break;
                    case RelatedNamesType.Assistant:
                        Label = new Label() { Value = Assistant };
                        break;
                    case RelatedNamesType.Manager:
                        Label = new Label() { Value = Manager };
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
                    Parameters.Set("TYPE", RelatedNamesMapping.GetType(RelatedNamesType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
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

        public RelatedNames()
        {
            Initialize();
            Type = RelatedNamesType.Other;
        }

        public RelatedNames(string value)
        {
            Initialize();
            Type = RelatedNamesType.Other;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new RelatedNamesSerializer();
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