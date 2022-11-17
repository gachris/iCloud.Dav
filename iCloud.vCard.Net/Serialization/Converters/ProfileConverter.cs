using iCloud.vCard.Net.Serialization.Mapping;
using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace iCloud.vCard.Net.Serialization.Converters;

internal class ProfileConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext? context, Type sourceType) => sourceType.IsGenericType && sourceType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override bool CanConvertTo(ITypeDescriptorContext? context, [NotNullWhen(true)] Type? destinationType) => (destinationType?.IsGenericType ?? false) && destinationType.GetGenericTypeDefinition().IsAssignableFrom(typeof(IEnumerable<>));

    /// <inheritdoc/>
    public override object? ConvertFrom(ITypeDescriptorContext? context, CultureInfo? culture, object value)
    {
        if (!CanConvertFrom(context, typeof(IEnumerable<CardProperty>)) || value is not IEnumerable<CardProperty> cardProperties) throw GetConvertFromException(value);
        return Convert(cardProperties);
    }

    /// <inheritdoc/>
    public override object? ConvertTo(ITypeDescriptorContext? context, CultureInfo? culture, object? value, Type destinationType)
    {
        if (!CanConvertTo(context, destinationType) || value is not Profile socialProfile) throw GetConvertToException(value, destinationType);
        return Convert(socialProfile);
    }

    /// <summary>Converts the X-SOCIALPROFILE property to social profile.</summary>
    private static Profile? Convert(IEnumerable<CardProperty> properties)
    {
        var socialProfileProperty = properties.FindByName(Constants.Contact.Property.Profile.Property.X_SOCIALPROFILE).ThrowIfNull(Constants.Contact.Property.Profile.Property.X_SOCIALPROFILE);
        var socialProfile = new Profile { Url = socialProfileProperty.ToString() };

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
            var labelProperty = properties.FindByName(Constants.Contact.Property.Profile.Property.X_ABLABEL);
            if (labelProperty is not null && labelProperty.Value?.ToString() is string label)
            {
                socialProfileTypeFromInternal = ProfileType.Custom;
                socialProfile.Label = label;
            }

            socialProfile.UserName = socialProfile.Url?.Replace(Constants.Contact.Property.Profile.CustomProfilePrefix, null);
        }
        else
        {
            socialProfile.UserName = socialProfileProperty.FindByName(Constants.Contact.Property.Profile.Property.X_USER)?.Value;
        }

        socialProfile.SocialProfileType = socialProfileTypeFromInternal;

        return socialProfile;
    }

    /// <summary>Converts the social profile to X-SOCIALPROFILE property.</summary>
    private static IEnumerable<CardProperty> Convert(Profile socialProfile)
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
            ProfileType.Custom => Constants.Contact.Property.Profile.CustomProfilePrefix + "{0}",
            _ => throw new ArgumentOutOfRangeException(nameof(ProfileType)),
        };

        url = string.Format(url, socialProfile.UserName);

        var groupId = Guid.NewGuid().ToString();
        var urlProperty = new CardProperty(Constants.Contact.Property.Profile.Property.X_SOCIALPROFILE, url, groupId);
        properties.Add(urlProperty);

        var socialProfileTypeInternal = ProfileTypeMapping.GetType(socialProfile.SocialProfileType);
        if (socialProfile.IsPreferred)
        {
            socialProfileTypeInternal = socialProfileTypeInternal.AddFlags(ProfileTypeInternal.Pref);
        }

        if (socialProfileTypeInternal is not 0)
        {
            socialProfileTypeInternal.StringArrayFlags()?.
                ForEach(type => urlProperty.Subproperties.Add(Constants.Contact.Property.Profile.Property.TYPE, type.ToLower()));
        }

        var label = socialProfile.SocialProfileType == ProfileType.Custom ? socialProfile.Label : null;
        if (label is not null)
        {
            var labelProperty = new CardProperty(Constants.Contact.Property.Profile.Property.X_ABLABEL, label, groupId);
            properties.Add(labelProperty);
        }

        urlProperty.Subproperties.Add(Constants.Contact.Property.Profile.Property.X_USER, socialProfile.UserName);

        return properties;
    }
}
