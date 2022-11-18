using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;

namespace iCloud.vCard.Net.Serialization;

public class ProfileSerializer : SerializerBase<Profile>
{
    /// <summary>Converts the X-SOCIALPROFILE property to social profile.</summary>
    public override void Deserialize(CardPropertyList properties, Profile socialProfile)
    {
        var socialProfileProperty = properties.FindByName(Constants.Contact.Profile.Property.X_SOCIALPROFILE).ThrowIfNull(Constants.Contact.Profile.Property.X_SOCIALPROFILE);
        socialProfile.Url = socialProfileProperty.ToString();

        _ = socialProfileProperty.Subproperties.TryParse<ProfileTypeInternal>(out var socialProfileTypeInternal);
        var isPreferred = socialProfileTypeInternal.HasFlag(ProfileTypeInternal.Pref);
        if (isPreferred)
        {
            socialProfile.IsPreferred = true;
            socialProfileTypeInternal = socialProfileTypeInternal.RemoveFlags(ProfileTypeInternal.Pref);
        }

        var socialProfileTypeFromInternal = ProfileTypeMapping.GetType(socialProfileTypeInternal);
        if (socialProfileTypeFromInternal is 0)
        {
            var labelProperty = properties.FindByName(Constants.Contact.Profile.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                socialProfileTypeFromInternal = ProfileType.Custom;
                socialProfile.Label = label;
            }

            socialProfile.UserName = socialProfile.Url?.Replace(Constants.Contact.Profile.CustomProfilePrefix, null);
        }
        else
        {
            socialProfile.UserName = socialProfileProperty.FindByName(Constants.Contact.Profile.Property.X_USER)?.Value;
        }

        socialProfile.SocialProfileType = socialProfileTypeFromInternal;
    }

    /// <summary>Converts the social profile to X-SOCIALPROFILE property.</summary>
    public override CardPropertyList Serialize(Profile socialProfile)
    {
        var properties = new List<CardProperty>();
        socialProfile.UserName = socialProfile.UserName.ThrowIfNull(nameof(socialProfile.UserName));

        var url = socialProfile.SocialProfileType switch
        {
            ProfileType.Twitter => "https://twitter.com/{0}",
            ProfileType.Facebook => "https://www.facebook.com/{0}",
            ProfileType.LinkedIn => "https://www.linkedin.com/in/{0}",
            ProfileType.Flickr => "https://www.flickr.com/photos/{0}/",
            ProfileType.Myspace => "https://www.myspace.com/{0}",
            ProfileType.SinaWeibo => "https://www.weibo.com/n/{0}",
            ProfileType.Custom => Constants.Contact.Profile.CustomProfilePrefix + "{0}",
            _ => throw new ArgumentOutOfRangeException(nameof(ProfileType)),
        };

        url = string.Format(url, socialProfile.UserName);

        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Profile.Property.X_SOCIALPROFILE, url, groupId);
        properties.Add(urlProperty);

        var socialProfileTypeInternal = ProfileTypeMapping.GetType(socialProfile.SocialProfileType);
        if (socialProfile.IsPreferred)
        {
            socialProfileTypeInternal = socialProfileTypeInternal.AddFlags(ProfileTypeInternal.Pref);
        }

        if (socialProfileTypeInternal is not 0)
        {
            socialProfileTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Profile.Property.TYPE, type.ToLower()));
        }

        var label = socialProfile.SocialProfileType == ProfileType.Custom ? socialProfile.Label : null;
        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Profile.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        urlProperty.Subproperties.Add(Constants.Contact.Profile.Property.X_USER, socialProfile.UserName);

        return new(properties);
    }
}
