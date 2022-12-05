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
    /// <summary>A web site defined in a <see cref="Contact"/>.</summary>
    /// <seealso cref="Type" />
    [Serializable]
    public class Website : EncodableDataType, IRelatedDataType
    {
        public const string HomePage = "_$!<HomePage>!$_";
        public const string School = "_$!<School>!$_";
        public const string Blog = "BLOG";

        public virtual bool IsPreferred { get; set; }

        /// <summary>The URL of the web site.</summary>
        /// <remarks>The format of the URL is not validated.</remarks>
        public virtual string Url { get; set; }

        /// <summary>The type of web site (e.g. home, work, etc).</summary>
        public virtual WebsiteType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<WebsiteTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(WebsiteTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(WebsiteTypeInternal.Pref);
                }

                var typeFromInternal = WebsiteTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    switch (Label?.Value)
                    {
                        case HomePage:
                            typeFromInternal = WebsiteType.HomePage;
                            break;
                        case School:
                            typeFromInternal = WebsiteType.School;
                            break;
                        case Blog:
                            typeFromInternal = WebsiteType.Blog;
                            break;
                        default:
                            typeFromInternal = WebsiteType.Custom;
                            break;
                    }
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = WebsiteTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(WebsiteTypeInternal.Pref);
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
                    case WebsiteType.HomePage:
                        Label = new Label() { Value = HomePage };
                        break;
                    case WebsiteType.School:
                        Label = new Label() { Value = School };
                        break;
                    case WebsiteType.Blog:
                        Label = new Label() { Value = Blog };
                        break;
                    case WebsiteType.Custom:
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
                    Parameters.Set("TYPE", WebsiteTypeMapping.GetType(WebsiteType.Other).StringArrayFlags().Select(x => x.ToUpperInvariant()));
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

        public Website()
        {
            Initialize();
            Type = WebsiteType.Other;
        }

        public Website(string value)
        {
            Initialize();
            Type = WebsiteType.Other;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new WebsiteSerializer();
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