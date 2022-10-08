using iCloud.Dav.People.Types;
using System;
using System.Collections.Generic;
using System.IO;

namespace iCloud.Dav.People.Serialization.Write;

/// <summary>
///     Implements the standard Person 2.1 and 3.0 text formats.
/// </summary>
internal class ContactGroupWriter : CardWriter<ContactGroup>
{
    private static readonly List<Action<List<CardProperty>, ContactGroup>> WriteProperties = new()
    {
        Build_BEGIN,
        Build_VERSION,
        Build_UID,
        Build_N,
        Build_FN,
        Build_X_ADDRESSBOOKSERVER_KIND,
        Build_X_ADDRESSBOOKSERVER_MEMBER,
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
    public ContactGroupWriter()
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
    /// <seealso cref="ContactGroup" />
    /// <seealso cref="CardProperty" />
    private static IEnumerable<CardProperty> BuildProperties(ContactGroup card)
    {
        var properties = new List<CardProperty>();
        WriteProperties.ForEach(propertyAction => propertyAction.Invoke(properties, card));
        return properties;
    }

    private static void Build_FN(IList<CardProperty> properties, ContactGroup card)
    {
        if (string.IsNullOrEmpty(card.FormattedName)) return;
        var cardProperty = new CardProperty(Constants.ContactGroup.Property.FN, card.FormattedName);
        properties.Add(cardProperty);
    }

    private static void Build_N(IList<CardProperty> properties, ContactGroup card)
    {
        if (string.IsNullOrEmpty(card.Name)) return;
        var cardProperty = new CardProperty(Constants.ContactGroup.Property.N, card.Name);
        properties.Add(cardProperty);
    }

    /// <summary>Builds PRODID properties.</summary>
    private static void Build_PRODID(IList<CardProperty> properties, ContactGroup card)
    {
        if (string.IsNullOrEmpty(card.ProductId)) return;
        properties.Add(new CardProperty(Constants.ContactGroup.Property.PRODID, card.ProductId));
    }

    /// <summary>Builds the REV property.</summary>
    private static void Build_REV(IList<CardProperty> properties, ContactGroup card)
    {
        if (!card.RevisionDate.HasValue) return;
        var cardProperty = new CardProperty(Constants.ContactGroup.Property.REV, card.RevisionDate.Value.ToString());
        properties.Add(cardProperty);
    }

    private static void Build_UID(IList<CardProperty> properties, ContactGroup card)
    {
        if (string.IsNullOrEmpty(card.UniqueId)) return;
        properties.Add(new CardProperty(Constants.ContactGroup.Property.UID, card.UniqueId));
    }

    private static void Build_X_ADDRESSBOOKSERVER_KIND(IList<CardProperty> properties, ContactGroup card)
    {
        var cardProperty = new CardProperty(Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_KIND, Constants.ContactGroup.Member_Type);
        properties.Add(cardProperty);
    }

    private static void Build_X_ADDRESSBOOKSERVER_MEMBER(IList<CardProperty> properties, ContactGroup card)
    {
        foreach (var memberResourceName in card.MemberResourceNames)
        {
            if (string.IsNullOrEmpty(memberResourceName)) continue;
            var member_urn = string.Concat(Constants.ContactGroup.urn_Prefix, memberResourceName);
            var cardProperty = new CardProperty(Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_MEMBER, member_urn);
            properties.Add(cardProperty);
        }
    }

    private static void Build_BEGIN(IList<CardProperty> properties, ContactGroup card) =>
        properties.Add(new CardProperty(Constants.ContactGroup.Property.BEGIN, Constants.ContactGroup.vCard_Type));

    private static void Build_END(IList<CardProperty> properties, ContactGroup card) =>
        properties.Add(new CardProperty(Constants.ContactGroup.Property.END, Constants.ContactGroup.vCard_Type));

    private static void Build_VERSION(IList<CardProperty> properties, ContactGroup card) =>
        properties.Add(new CardProperty(Constants.ContactGroup.Property.VERSION, Constants.ContactGroup.vCard_Version));

    /// <summary>Writes a Person to an output text writer.</summary>
    public override void Write(ContactGroup card, TextWriter output, string charsetName)
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
