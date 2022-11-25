using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.DataTypes.Mapping;
using iCloud.Dav.People.Utils;
using System;
using System.IO;
using vCard.Net.Serialization;
using vCard.Net.Serialization.DataTypes;

namespace iCloud.Dav.People.Serialization.DataTypes;

public class X_SocialProfileSerializer : EncodableDataTypeSerializer
{
    public X_SocialProfileSerializer() : base()
    {
    }

    public X_SocialProfileSerializer(SerializationContext ctx) : base(ctx)
    {
    }

    public override Type TargetType => typeof(X_SocialProfile);

    public override string? SerializeToString(object obj)
    {
        if (obj is not X_SocialProfile socialProfile)
        {
            return null;
        }

        // var properties = new List<CardProperty>();
        // socialProfile.UserName = socialProfile.UserName.ThrowIfNull(nameof(socialProfile.UserName));

        // var url = socialProfile.SocialProfileType switch
        // {
        //     ProfileType.Twitter => "https://twitter.com/{0}",
        //     ProfileType.Facebook => "https://www.facebook.com/{0}",
        //     ProfileType.LinkedIn => "https://www.linkedin.com/in/{0}",
        //     ProfileType.Flickr => "https://www.flickr.com/photos/{0}/",
        //     ProfileType.Myspace => "https://www.myspace.com/{0}",
        //     ProfileType.SinaWeibo => "https://www.weibo.com/n/{0}",
        //     ProfileType.Custom => Constants.Contact.Profile.CustomProfilePrefix + "{0}",
        //     _ => throw new ArgumentOutOfRangeException(nameof(ProfileType)),
        // };

        // url = string.Format(url, socialProfile.UserName);

        // var groupId = Guid.NewGuid().ToString();
        // var urlProperty = new CardProperty(Constants.Contact.Profile.Property.X_SOCIALPROFILE, url, groupId);
        // properties.Add(urlProperty);

        // var socialProfileTypeInternal = ProfileTypeMapping.GetType(socialProfile.SocialProfileType);
        // if (socialProfile.IsPreferred)
        // {
        //     socialProfileTypeInternal = socialProfileTypeInternal.AddFlags(ProfileTypeInternal.Pref);
        // }

        // if (socialProfileTypeInternal is not 0)
        // {
        //     socialProfileTypeInternal.StringArrayFlags()?.
        //         ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Profile.Property.TYPE, type.ToLower()));
        // }

        // var label = socialProfile.SocialProfileType == ProfileType.Custom ? socialProfile.Label : null;
        // if (label is not null)
        // {
        //     var labelProperty = new CardProperty(Constants.Contact.Profile.Property.X_ABLABEL, label, groupId);
        //     properties.Add(labelProperty);
        // }

        // urlProperty.Subproperties.Add(Constants.Contact.Profile.Property.X_USER, socialProfile.UserName);

        // return new(properties);

        var value = socialProfile.Value.ToString();
        return Encode(socialProfile, value);
    }

    public X_SocialProfile? Deserialize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return null;
        }

        if (CreateAndAssociate() is not X_SocialProfile socialProfile)
        {
            return null;
        }

        // Decode the value, if necessary!
        value = Decode(socialProfile, value);

        if (value is null)
        {
            return null;
        }

        socialProfile.Url = value;

        var types = socialProfile.Parameters.GetMany("TYPE");

        _ = types.TryParse<ProfileTypeInternal>(out var socialProfileTypeInternal);
        var isPreferred = socialProfileTypeInternal.HasFlag(ProfileTypeInternal.Pref);
        if (isPreferred)
        {
            socialProfile.IsPreferred = true;
            socialProfileTypeInternal = socialProfileTypeInternal.RemoveFlags(ProfileTypeInternal.Pref);
        }

        var socialProfileTypeFromInternal = ProfileTypeMapping.GetType(socialProfileTypeInternal);
        if (socialProfileTypeFromInternal is 0)
        {
            //var labelProperty = properties.FindByName(Constants.Contact.Profile.Property.X_ABLABEL);
            //if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            //{
            //    socialProfileTypeFromInternal = ProfileType.Custom;
            //    socialProfile.Label = label;
            //}

            socialProfile.UserName = socialProfile.Url.Replace("x-apple:", null);
        }
        else
        {
            socialProfile.UserName = socialProfile.Parameters.Get("X-USER");
        }

        socialProfile.SocialProfileType = socialProfileTypeFromInternal;

        return socialProfile;
    }

    public override object? Deserialize(TextReader tr) => Deserialize(tr.ReadToEnd());
}