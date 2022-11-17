using iCloud.vCard.Net.Serialization;
using iCloud.vCard.Net.Types;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace iCloud.vCard.Net.Serialization.Write;

/// <summary>
///     Implements the standard Person 2.1 and 3.0 text formats.
/// </summary>
public class ContactWriter : CardWriter<Contact>
{
    private static readonly List<Action<List<CardProperty>, Contact>> _properties = new()
    {
        Build_BEGIN,
        Build_VERSION,
        Build_UID,
        Build_N,
        Build_FN,
        Build_X_PHONETIC_FIRST_NAME,
        Build_X_PHONETIC_LAST_NAME,
        Build_ORG,
        Build_X_PHONETIC_ORG,
        Build_NICKNAME,
        Build_BDAY,
        Build_TITLE,
        Build_NOTE,
        Build_PHOTO,
        Build_ADR,
        Build_TEL,
        Build_X_ABRELATEDNAMES,
        Build_URL,
        Build_EMAIL,
        Build_X_ABDATE,
        Build_X_SOCIALPROFILE,
        Build_PRODID,
        Build_REV,
        Build_END
    };

    /// <summary>Creates a new instance of the standard writer.</summary>
    /// <remarks>
    ///     The standard writer is configured to create Person
    ///     files in the highest supported version.  This is
    ///     currently version 3.0.
    /// </remarks>
    public ContactWriter()
    {
    }

    /// <summary>
    ///     Builds a collection of standard properties based on
    ///     the specified Person.
    /// </summary>
    /// <returns>
    ///     A <see cref="List{CardProperty}" /> that contains all
    ///     properties for the current Person, including the header
    ///     and footer properties.
    /// </returns>
    /// <seealso cref="Contact" />
    /// <seealso cref="CardProperty" />
    private static IEnumerable<CardProperty> BuildProperties(Contact card)
    {
        var properties = new List<CardProperty>();
        _properties.ForEach(propertyAction => propertyAction.Invoke(properties, card));

        var groups = properties.GroupBy(x => x.Group).Where(x => x.Count() > 1 && x.Key != null);

        var current = 0;

        foreach (var group in groups)
        {
            current++;
            group.ForEach(x => x.Name = string.Concat($"item{current}.", x.Name));
        }

        return properties;
    }

    /// <summary>Builds the BDAY property.</summary>
    private static void Build_BDAY(List<CardProperty> properties, Contact card)
    {
        if (!card.Birthdate.HasValue) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.BDAY, card.Birthdate.Value);
        properties.Add(cardProperty);
    }

    /// <summary>Builds EMAIL properties.</summary>
    private static void Build_EMAIL(List<CardProperty> allProperties, Contact card) => card.EmailAddresses.ForEach(emailAddress =>
                                                                                           {
                                                                                               var converter = TypeDescriptor.GetConverter(typeof(Email));
                                                                                               if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                               {
                                                                                                   var properties = (IEnumerable<CardProperty>?)converter.ConvertTo(emailAddress, typeof(IEnumerable<CardProperty>));

                                                                                                   if (properties is null) return;
                                                                                                   allProperties.AddRange(properties);
                                                                                               }
                                                                                           });

    /// <summary>Builds TEL properties.</summary>
    private static void Build_TEL(List<CardProperty> allProperties, Contact card) => card.Phones.ForEach(phone =>
                                                                                         {
                                                                                             var converter = TypeDescriptor.GetConverter(typeof(Phone));
                                                                                             if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                             {
                                                                                                 var properties = (IEnumerable<CardProperty>?)converter.ConvertTo(phone, typeof(IEnumerable<CardProperty>));
                                                                                                 if (properties is null) return;
                                                                                                 allProperties.AddRange(properties);
                                                                                             }
                                                                                         });

    /// <summary>Builds ADR properties.</summary>
    private static void Build_ADR(List<CardProperty> properties, Contact card) => card.Addresses.ForEach(address =>
                                                                                      {
                                                                                          var converter = TypeDescriptor.GetConverter(typeof(Address));
                                                                                          if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                          {
                                                                                              var propertiesToAdd = (IEnumerable<CardProperty>?)converter.ConvertTo(address, typeof(IEnumerable<CardProperty>));
                                                                                              if (propertiesToAdd is null) return;
                                                                                              properties.AddRange(propertiesToAdd);
                                                                                          }
                                                                                      });

    private static void Build_URL(List<CardProperty> properties, Contact card) => card.Websites.ForEach(website =>
                                                                                      {
                                                                                          var converter = TypeDescriptor.GetConverter(typeof(Website));
                                                                                          if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                          {
                                                                                              var propertiesToAdd = (IEnumerable<CardProperty>?)converter.ConvertTo(website, typeof(IEnumerable<CardProperty>));
                                                                                              if (propertiesToAdd is null) return;
                                                                                              properties.AddRange(propertiesToAdd);
                                                                                          }
                                                                                      });

    private static void Build_FN(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.FormattedName)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.FN, card.FormattedName);
        properties.Add(cardProperty);
    }

    private static void Build_N(List<CardProperty> properties, Contact card)
    {
        var values = new ValueCollection(';')
        {
            card.LastName,
            card.FirstName,
            card.MiddleName,
            card.NamePrefix,
            card.NameSuffix
        };
        var cardProperty = new CardProperty(Constants.Contact.Property.N, values);
        properties.Add(cardProperty);
    }

    /// <summary>Builds the NICKNAME property.</summary>
    private static void Build_NICKNAME(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.Nickname)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.NICKNAME, card.Nickname);
        properties.Add(cardProperty);
    }

    /// <summary>Builds the NOTE property.</summary>
    private static void Build_NOTE(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.Notes)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.NOTE, card.Notes);
        properties.Add(cardProperty);
    }

    /// <summary>Builds the ORG property.</summary>
    private static void Build_ORG(List<CardProperty> properties, Contact card)
    {
        if (!string.IsNullOrEmpty(card.Organization) || !string.IsNullOrEmpty(card.Department))
        {
            var values = new ValueCollection(';')
            {
                card.Organization,
                card.Department,
            };
            var cardProperty = new CardProperty(Constants.Contact.Property.ORG, values);
            properties.Add(cardProperty);
        }
    }

    private static void Build_PHOTO(List<CardProperty> properties, Contact card)
    {
        if (card.Photo is null) return;
        var converter = TypeDescriptor.GetConverter(typeof(Photo));
        if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
        {
            var propertiesToAdd = (IEnumerable<CardProperty>?)converter.ConvertTo(card.Photo, typeof(IEnumerable<CardProperty>));
            if (propertiesToAdd is null) return;
            properties.AddRange(propertiesToAdd);
        }
    }

    private static void Build_X_ABRELATEDNAMES(List<CardProperty> properties, Contact card) => card.RelatedPeople.ForEach(relatedPeople =>
                                                                                                   {
                                                                                                       var converter = TypeDescriptor.GetConverter(typeof(RelatedPeople));
                                                                                                       if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                                       {
                                                                                                           var propertiesToAdd = (IEnumerable<CardProperty>?)converter.ConvertTo(relatedPeople, typeof(IEnumerable<CardProperty>));
                                                                                                           if (propertiesToAdd is null) return;
                                                                                                           properties.AddRange(propertiesToAdd);
                                                                                                       }
                                                                                                   });

    private static void Build_X_ABDATE(List<CardProperty> properties, Contact card) => card.Dates.ForEach(date =>
                                                                                           {
                                                                                               var converter = TypeDescriptor.GetConverter(typeof(Date));
                                                                                               if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                               {
                                                                                                   var propertiesToAdd = (IEnumerable<CardProperty>?)converter.ConvertTo(date, typeof(IEnumerable<CardProperty>));
                                                                                                   if (propertiesToAdd is null) return;
                                                                                                   properties.AddRange(propertiesToAdd);
                                                                                               }
                                                                                           });

    private static void Build_X_SOCIALPROFILE(List<CardProperty> properties, Contact card) => card.Profiles.ForEach(date =>
                                                                                                  {
                                                                                                      var converter = TypeDescriptor.GetConverter(typeof(Profile));
                                                                                                      if (converter.CanConvertTo(typeof(IEnumerable<CardProperty>)))
                                                                                                      {
                                                                                                          var propertiesToAdd = (IEnumerable<CardProperty>?)converter.ConvertTo(date, typeof(IEnumerable<CardProperty>));
                                                                                                          if (propertiesToAdd is null) return;
                                                                                                          properties.AddRange(propertiesToAdd);
                                                                                                      }
                                                                                                  });

    private static void Build_X_PHONETIC_ORG(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.PhoneticOrganization)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.X_PHONETIC_ORG, card.PhoneticOrganization);
        properties.Add(cardProperty);
    }

    private static void Build_X_PHONETIC_LAST_NAME(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.PhoneticLastName)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.X_PHONETIC_LAST_NAME, card.PhoneticLastName);
        properties.Add(cardProperty);
    }

    private static void Build_X_PHONETIC_FIRST_NAME(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.PhoneticFirstName)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.X_PHONETIC_FIRST_NAME, card.PhoneticFirstName);
        properties.Add(cardProperty);
    }

    /// <summary>Builds PRODID properties.</summary>
    private static void Build_PRODID(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.ProductId)) return;
        properties.Add(new CardProperty(Constants.Contact.Property.PRODID, card.ProductId));
    }

    /// <summary>Builds the REV property.</summary>
    private static void Build_REV(List<CardProperty> properties, Contact card)
    {
        if (!card.RevisionDate.HasValue) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.REV, card.RevisionDate.Value.ToString());
        properties.Add(cardProperty);
    }

    private static void Build_TITLE(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.Title)) return;
        var cardProperty = new CardProperty(Constants.Contact.Property.TITLE, card.Title);
        properties.Add(cardProperty);
    }

    private static void Build_UID(List<CardProperty> properties, Contact card)
    {
        if (string.IsNullOrEmpty(card.Uid)) return;
        properties.Add(new CardProperty(Constants.Contact.Property.UID, card.Uid));
    }

    private static void Build_BEGIN(List<CardProperty> properties, Contact card) => properties.Add(new CardProperty(Constants.Contact.Property.BEGIN, Constants.Contact.vCard_Type));

    private static void Build_END(List<CardProperty> properties, Contact card) => properties.Add(new CardProperty(Constants.Contact.Property.END, Constants.Contact.vCard_Type));

    private static void Build_VERSION(List<CardProperty> properties, Contact card) => properties.Add(new CardProperty(Constants.Contact.Property.VERSION, Constants.Contact.vCard_Version));

    /// <summary>Writes a Person to an output text writer.</summary>
    public override void Write(Contact card, TextWriter output, string charsetName)
    {
        if (card == null)
            throw new ArgumentNullException(nameof(card));
        if (output == null)
            throw new ArgumentNullException(nameof(output));
        Write(BuildProperties(card), output, charsetName);
    }

    /// <summary>
    ///     Writes a collection of Person properties to an output text writer.
    /// </summary>
    public void Write(IEnumerable<CardProperty> properties, TextWriter output, string charsetName)
    {
        if (properties == null)
            throw new ArgumentNullException(nameof(properties));
        if (output == null)
            throw new ArgumentNullException(nameof(output));
        foreach (var property in properties)
            output.WriteLine(EncodeProperty(property, charsetName));
    }
}
