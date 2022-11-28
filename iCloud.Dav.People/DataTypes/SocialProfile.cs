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
        public virtual ProfileType Type
        {
            get
            {
                var types = Parameters.GetMany("TYPE");

                _ = types.TryParse<ProfileTypeInternal>(out var typeInternal);
                var isPreferred = typeInternal.HasFlag(ProfileTypeInternal.Pref);
                if (isPreferred)
                {
                    IsPreferred = true;
                    typeInternal = typeInternal.RemoveFlags(ProfileTypeInternal.Pref);
                }

                var typeFromInternal = ProfileTypeMapping.GetType(typeInternal);
                if (typeFromInternal is 0)
                {
                    typeFromInternal = ProfileType.Custom;
                }

                return typeFromInternal;
            }
            set
            {
                var typeInternal = ProfileTypeMapping.GetType(value);
                if (IsPreferred)
                {
                    typeInternal = typeInternal.AddFlags(ProfileTypeInternal.Pref);
                }

                if (!(typeInternal is 0))
                {
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
                {
                    Parameters.Remove("TYPE");
                }

                var typeFromInternal = ProfileTypeMapping.GetType(typeInternal);

                switch (typeFromInternal)
                {
                    case ProfileType.Custom:
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
                    var typeInternal = ProfileTypeMapping.GetType(ProfileType.Twitter);
                    Parameters.Set("TYPE", typeInternal.StringArrayFlags().Select(x => x.ToUpperInvariant()));
                }
                else
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
            set => Parameters.Set("X-USER", value);
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
            Type = ProfileType.Twitter;
        }

        public SocialProfile(string value)
        {
            Initialize();
            Type = ProfileType.Twitter;
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