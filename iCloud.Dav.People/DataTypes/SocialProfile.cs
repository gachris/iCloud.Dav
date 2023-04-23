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
    ///     Social profile information for a <see cref="SocialProfile" />.
    /// </summary>
    public class SocialProfile : EncodableDataType, IRelatedDataType
    {
        public static string Twitter = "https://twitter.com/{0}";
        public static string Facebook = "https://www.facebook.com/{0}";
        public static string LinkedIn = "https://www.linkedin.com/in/{0}";
        public static string Flickr = "https://www.flickr.com/photos/{0}/";
        public static string Myspace = "https://www.myspace.com/{0}";
        public static string SinaWeibo = "https://www.weibo.com/n/{0}";
        public static string Custom = "x-apple:{0}";

        public virtual bool IsPreferred { get; set; }

        /// <summary>The social profile type.</summary>
        public virtual SocialProfileType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<SocialProfileTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(SocialProfileTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(SocialProfileTypeInternal.Pref);
                }

                var typeFromInternal = SocialProfileTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    typeFromInternal = SocialProfileType.Custom;
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = SocialProfileTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(SocialProfileTypeInternal.Pref);
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
                    case SocialProfileType.Twitter:
                        Label = null;
                        Url = string.Format(Twitter, UserName);
                        break;
                    case SocialProfileType.Facebook:
                        Url = string.Format(Facebook, UserName);
                        Label = null;
                        break;
                    case SocialProfileType.LinkedIn:
                        Url = string.Format(LinkedIn, UserName);
                        Label = null;
                        break;
                    case SocialProfileType.Flickr:
                        Url = string.Format(Flickr, UserName);
                        Label = null;
                        break;
                    case SocialProfileType.Myspace:
                        Url = string.Format(Myspace, UserName);
                        Label = null;
                        break;
                    case SocialProfileType.SinaWeibo:
                        Url = string.Format(SinaWeibo, UserName);
                        Label = null;
                        break;
                    case SocialProfileType.Custom:
                        Url = string.Format(Custom, UserName);
                        Label = new Label() { Value = Label?.Value };
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
                    Parameters.Set("TYPE", SocialProfileTypeMapping.GetType(SocialProfileType.Twitter).StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else if (value != null)
                {
                    Properties.Set("X-ABLABEL", value);
                    Parameters.Remove("TYPE");
                }
            }
        }

        /// <summary>The user name.</summary>
        public virtual string UserName
        {
            get => Parameters.Get("X-USER");
            set
            {
                Parameters.Set("X-USER", value);

                switch (Type)
                {
                    case SocialProfileType.Twitter:
                        Url = string.Format(Twitter, UserName);
                        break;
                    case SocialProfileType.Facebook:
                        Url = string.Format(Facebook, UserName);
                        break;
                    case SocialProfileType.LinkedIn:
                        Url = string.Format(LinkedIn, UserName);
                        break;
                    case SocialProfileType.Flickr:
                        Url = string.Format(Flickr, UserName);
                        break;
                    case SocialProfileType.Myspace:
                        Url = string.Format(Myspace, UserName);
                        break;
                    case SocialProfileType.SinaWeibo:
                        Url = string.Format(SinaWeibo, UserName);
                        break;
                    case SocialProfileType.Custom:
                        Url = string.Format(Custom, UserName);
                        break;
                }
            }
        }

        /// <summary>The URL of the X-SOCIALPROFILE site.</summary>
        /// <remarks>The URL is not validated.</remarks>
        public virtual string Url { get; set; }

        /// <summary>
        /// Returns a list of properties that are associated with the TEL object.
        /// </summary>
        public virtual CardDataTypePropertyList Properties { get; protected set; }

        public SocialProfile()
        {
            Initialize();
            Type = SocialProfileType.Twitter;
        }

        public SocialProfile(string value)
        {
            Initialize();
            Type = SocialProfileType.Twitter;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var serializer = new SocialProfileSerializer();
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