using iCloud.vCard.Net.Serialization;
using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace iCloud.vCard.Net.Serialization.Read;

/// <summary>
///     Reads a Person written in the standard 2.0 or 3.0 text formats.
///     This is the primary (standard) Person format used by most
///     applications.
/// </summary>
public partial class ContactReader : CardReader<Contact>
{
    #region Fields/Consts

    private static readonly Dictionary<string, Action<Contact, IEnumerable<CardProperty>>> _properties = new()
    {
        { Constants.Contact.Property.Address.Property.ADR, Read_ADR },
        { Constants.Contact.Property.BDAY, Read_BDAY },
        { Constants.Contact.Property.EmailAddress.Property.EMAIL, Read_EMAIL },
        { Constants.Contact.Property.FN,  Read_FN },
        { Constants.Contact.Property.N,  Read_N },
        { Constants.Contact.Property.NICKNAME,  Read_NICKNAME },
        { Constants.Contact.Property.NOTE,  Read_NOTE },
        { Constants.Contact.Property.ORG,  Read_ORG },
        { Constants.Contact.Property.Photo.Property.PHOTO,  Read_PHOTO },
        { Constants.Contact.Property.PRODID,  Read_PRODID },
        { Constants.Contact.Property.REV,  Read_REV },
        { Constants.Contact.Property.Phone.Property.TEL,  Read_TEL },
        { Constants.Contact.Property.TITLE,  Read_TITLE },
        { Constants.Contact.Property.UID,  Read_UID },
        { Constants.Contact.Property.Website.Property.URL,  Read_URL },
        { Constants.Contact.Property.X_PHONETIC_FIRST_NAME,  Read_X_PHONETIC_FIRST_NAME },
        { Constants.Contact.Property.X_PHONETIC_LAST_NAME,  Read_X_PHONETIC_LAST_NAME },
        { Constants.Contact.Property.Profile.Property.X_SOCIALPROFILE,  Read_X_SOCIALPROFILE },
        { Constants.Contact.Property.X_PHONETIC_ORG,  Read_X_PHONETIC_ORG },
        { Constants.Contact.Property.RelatedPerson.Property.X_ABRELATEDNAMES,  Read_X_ABRELATEDNAMES },
        { Constants.Contact.Property.Date.Property.X_ABDATE,  Read_X_ABDATE },
        //{ "X-AIM",  Read_IMPP },
        //{ "X-ICQ",  Read_IMPP },
        //{ "X-JABBER",  Read_IMPP },
        //{ "X-MSN",  Read_IMPP },
        //{ "X-YAHOO",  Read_IMPP },
        //{ "IMPP",  Read_IMPP },
    };

    #endregion

    /// <summary>
    ///     Updates a Person object based on the contents of a CardProperty.
    /// </summary>
    /// <param name="card">An initialized Person object.</param>
    /// <param name="property">An initialized CardProperty object.</param>
    /// <remarks>
    ///     <para>
    ///         This method examines the contents of a property
    ///         and attempts to update an existing Person based on
    ///         the property name and value.  This function must
    ///         be updated when new Person properties are implemented.
    ///     </para>
    /// </remarks>
    protected override void ReadInto(Contact card, IEnumerable<CardProperty> property)
    {
        card.ThrowIfNull(nameof(card));
        property.ThrowIfNull(nameof(property));

        _properties.TryGetValue(property.First().Name, out var propertyAction);
        propertyAction?.Invoke(card, property);
    }

    /// <summary>Reads an ADR property.</summary>
    private static void Read_ADR(Contact card, IEnumerable<CardProperty> properties)
    {
        var addressConverter = TypeDescriptor.GetConverter(typeof(Address));
        if (addressConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var address = (Address?)addressConverter.ConvertFrom(properties);
            if (address is null) return;
            card.Addresses.Add(address);
        }
    }

    /// <summary>Reads the N property.</summary>
    private static void Read_N(Contact card, IEnumerable<CardProperty> properties)
    {
        var nproperty = properties.FindByName(Constants.Contact.Property.N).ThrowIfNull(Constants.Contact.Property.N);

        var values = nproperty.ToString()?.Split(';');
        if (values is null) return;
        if (!values.Any()) return;

        card.LastName = values[0];
        if (values.Length == 1)
            return;
        card.FirstName = values[1];

        if (values.Length == 2)
            return;
        card.MiddleName = values[2];

        if (values.Length == 3)
            return;
        card.NamePrefix = values[3];

        if (values.Length == 4)
            return;
        card.NameSuffix = values[4];
    }

    /// <summary>Reads the BDAY property.</summary>
    private static void Read_BDAY(Contact card, IEnumerable<CardProperty> properties)
    {
        var bdayproperty = properties.FindByName(Constants.Contact.Property.BDAY).ThrowIfNull(Constants.Contact.Property.BDAY);
        card.Birthdate = DateTimeHelper.TryParseDate(bdayproperty.ToString());
    }

    /// <summary>Reads the URL property.</summary>
    private static void Read_URL(Contact card, IEnumerable<CardProperty> properties)
    {
        var websiteConverter = TypeDescriptor.GetConverter(typeof(Website));
        if (websiteConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var website = (Website?)websiteConverter.ConvertFrom(properties);
            if (website is null) return;
            card.Websites.Add(website);
        }
    }

    /// <summary>Reads an EMAIL property.</summary>
    private static void Read_EMAIL(Contact card, IEnumerable<CardProperty> properties)
    {
        var emailConverter = TypeDescriptor.GetConverter(typeof(Email));
        if (emailConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var email = (Email?)emailConverter.ConvertFrom(properties);
            if (email is null) return;
            card.EmailAddresses.Add(email);
        }
    }

    /// <summary>Reads the TEL property.</summary>
    private static void Read_TEL(Contact card, IEnumerable<CardProperty> properties)
    {
        var phoneConverter = TypeDescriptor.GetConverter(typeof(Phone));
        if (phoneConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var phone = (Phone?)phoneConverter.ConvertFrom(properties);
            if (phone is null) return;
            card.Phones.Add(phone);
        }
    }

    private static void Read_X_ABDATE(Contact card, IEnumerable<CardProperty> properties)
    {
        var dateConverter = TypeDescriptor.GetConverter(typeof(Date));
        if (dateConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var date = (Date?)dateConverter.ConvertFrom(properties);
            if (date is null) return;
            card.Dates.Add(date);
        }
    }

    private static void Read_X_SOCIALPROFILE(Contact card, IEnumerable<CardProperty> properties)
    {
        var socialProfileConverter = TypeDescriptor.GetConverter(typeof(Profile));
        if (socialProfileConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var socialProfile = (Profile?)socialProfileConverter.ConvertFrom(properties);
            if (socialProfile is null) return;
            card.Profiles.Add(socialProfile);
        }
    }

    private static void Read_X_ABRELATEDNAMES(Contact card, IEnumerable<CardProperty> properties)
    {
        var relatedPersonConverter = TypeDescriptor.GetConverter(typeof(RelatedPeople));
        if (relatedPersonConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var relatedPerson = (RelatedPeople?)relatedPersonConverter.ConvertFrom(properties);
            if (relatedPerson is null) return;
            card.RelatedPeople.Add(relatedPerson);
        }
    }

    /// <summary>Reads the ORG property.</summary>
    private static void Read_ORG(Contact card, IEnumerable<CardProperty> properties)
    {
        var orgProperty = properties.FindByName(Constants.Contact.Property.ORG).ThrowIfNull(Constants.Contact.Property.ORG);
        var values = orgProperty.Value?.ToString()?.Split(';');
        card.Organization = values?[0];
        if (values?.Length == 1) return;
        card.Department = values?[1];
    }

    /// <summary>Reads the PHOTO property.</summary>
    private static void Read_PHOTO(Contact card, IEnumerable<CardProperty> properties)
    {
        var photoConverter = TypeDescriptor.GetConverter(typeof(Photo));
        if (photoConverter.CanConvertFrom(typeof(IEnumerable<CardProperty>)))
        {
            var photo = (Photo?)photoConverter.ConvertFrom(properties);
            if (photo is null) return;
            card.Photo = photo;
        }
    }

    private static void Read_X_PHONETIC_ORG(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.X_PHONETIC_ORG).ThrowIfNull(Constants.Contact.Property.X_PHONETIC_ORG);
        card.PhoneticOrganization = property.ToString();
    }

    /// <summary>Reads the FN property.</summary>
    private static void Read_FN(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.FN).ThrowIfNull(Constants.Contact.Property.FN);
        card.FormattedName = property.ToString();
    }

    /// <summary>Reads the NICKNAME property.</summary>
    private static void Read_NICKNAME(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.NICKNAME).ThrowIfNull(Constants.Contact.Property.NICKNAME);
        card.Nickname = property.ToString();
    }

    /// <summary>Reads the NOTE property.</summary>
    private static void Read_NOTE(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.NOTE).ThrowIfNull(Constants.Contact.Property.NOTE);
        card.Notes = property.ToString();
    }

    /// <summary>Reads the PRODID property.</summary>
    private static void Read_PRODID(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.PRODID).ThrowIfNull(Constants.Contact.Property.PRODID);
        card.ProductId = property.ToString();
    }

    /// <summary>Reads the REV property.</summary>
    private static void Read_REV(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.REV).ThrowIfNull(Constants.Contact.Property.REV);
        card.RevisionDate = DateTimeHelper.ParseDate(property.ToString());
    }

    /// <summary>Reads the TITLE property.</summary>
    private static void Read_TITLE(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.TITLE).ThrowIfNull(Constants.Contact.Property.TITLE);
        card.Title = property.ToString();
    }

    /// <summary>Reads the UID property.</summary>
    private static void Read_UID(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.UID).ThrowIfNull(Constants.Contact.Property.UID);
        card.Uid = property.ToString();
    }

    private static void Read_IMPP(Contact card, IEnumerable<CardProperty> properties)
    {
    }

    private static void Read_X_PHONETIC_FIRST_NAME(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.X_PHONETIC_FIRST_NAME).ThrowIfNull(Constants.Contact.Property.X_PHONETIC_FIRST_NAME);
        card.PhoneticFirstName = property.ToString();
    }

    private static void Read_X_PHONETIC_LAST_NAME(Contact card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.Contact.Property.X_PHONETIC_LAST_NAME).ThrowIfNull(Constants.Contact.Property.X_PHONETIC_LAST_NAME);
        card.PhoneticLastName = property.ToString();
    }
}