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
        public virtual RelatedPeopleType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<RelatedPeopleTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(RelatedPeopleTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(RelatedPeopleTypeInternal.Pref);
                }

                var typeFromInternal = RelatedPeopleTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case Father:
                            typeFromInternal = RelatedPeopleType.Father;
                            break;
                        case Mother:
                            typeFromInternal = RelatedPeopleType.Mother;
                            break;
                        case Parent:
                            typeFromInternal = RelatedPeopleType.Parent;
                            break;
                        case Brother:
                            typeFromInternal = RelatedPeopleType.Brother;
                            break;
                        case Sister:
                            typeFromInternal = RelatedPeopleType.Sister;
                            break;
                        case Child:
                            typeFromInternal = RelatedPeopleType.Child;
                            break;
                        case Friend:
                            typeFromInternal = RelatedPeopleType.Friend;
                            break;
                        case Spouse:
                            typeFromInternal = RelatedPeopleType.Spouse;
                            break;
                        case Partner:
                            typeFromInternal = RelatedPeopleType.Partner;
                            break;
                        case Assistant:
                            typeFromInternal = RelatedPeopleType.Assistant;
                            break;
                        case Manager:
                            typeFromInternal = RelatedPeopleType.Manager;
                            break;
                        default:
                            typeFromInternal = RelatedPeopleType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = RelatedPeopleTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(RelatedPeopleTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                var typeFromInternal = RelatedPeopleTypeMapping.GetType(typeInternal);

                switch (typeFromInternal)
                {
                    case RelatedPeopleType.Father:
                        Label = new Label() { Value = Father };
                        break;
                    case RelatedPeopleType.Mother:
                        Label = new Label() { Value = Mother };
                        break;
                    case RelatedPeopleType.Parent:
                        Label = new Label() { Value = Parent };
                        break;
                    case RelatedPeopleType.Brother:
                        Label = new Label() { Value = Brother };
                        break;
                    case RelatedPeopleType.Sister:
                        Label = new Label() { Value = Sister };
                        break;
                    case RelatedPeopleType.Child:
                        Label = new Label() { Value = Child };
                        break;
                    case RelatedPeopleType.Friend:
                        Label = new Label() { Value = Friend };
                        break;
                    case RelatedPeopleType.Spouse:
                        Label = new Label() { Value = Spouse };
                        break;
                    case RelatedPeopleType.Partner:
                        Label = new Label() { Value = Partner };
                        break;
                    case RelatedPeopleType.Assistant:
                        Label = new Label() { Value = Assistant };
                        break;
                    case RelatedPeopleType.Manager:
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
                if (value == null)
                {
                    Properties.Remove("X-ABLABEL");
                    var typeInternal = RelatedPeopleTypeMapping.GetType(RelatedPeopleType.Other);
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

        public RelatedNames()
        {
            Initialize();
            Type = RelatedPeopleType.Other;
        }

        public RelatedNames(string value)
        {
            Initialize();
            Type = RelatedPeopleType.Other;
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