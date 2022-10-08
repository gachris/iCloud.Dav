using iCloud.Dav.Core.Utils;
using iCloud.Dav.People.Types;
using iCloud.Dav.People.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.Dav.People.Serialization.Read;

/// <summary>
///     Reads a group written in the standard 2.0 or 3.0 text formats.
///     This is the primary (standard) group format used by most
///     applications.
/// </summary>
internal class ContactGroupReader : CardReader<ContactGroup>
{
    #region Fields/Consts

    private static readonly Dictionary<string, Action<ContactGroup, IEnumerable<CardProperty>>> _properties = new()
    {
        { Constants.ContactGroup.Property.UID,  Read_UID },
        { Constants.ContactGroup.Property.N,  Read_N },
        { Constants.ContactGroup.Property.FN,  Read_FN },
        { Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_KIND,  Read_X_ADDRESSBOOKSERVER_KIND },
        { Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_MEMBER,  Read_X_ADDRESSBOOKSERVER_MEMBER },
        { Constants.ContactGroup.Property.PRODID,  Read_PRODID },
        { Constants.ContactGroup.Property.REV,  Read_REV },
    };

    #endregion

    /// <summary>
    ///     Updates a group object based on the contents of a CardProperty.
    /// </summary>
    /// <param name="contactGroup">An initialized group object.</param>
    /// <param name="properties">An initialized CardProperty object.</param>
    /// <remarks>
    ///     <para>
    ///         This method examines the contents of a property
    ///         and attempts to update an existing group based on
    ///         the property name and value.  This function must
    ///         be updated when new group properties are implemented.
    ///     </para>
    /// </remarks>
    protected override void ReadInto(ContactGroup contactGroup, IEnumerable<CardProperty> properties)
    {
        contactGroup.ThrowIfNull(nameof(contactGroup));
        properties.ThrowIfNull(nameof(properties));

        _properties.TryGetValue(properties.First().Name, out var propertyAction);
        propertyAction?.Invoke(contactGroup, properties);
    }

    /// <summary>Reads the UID property.</summary>
    private static void Read_UID(ContactGroup contactGroup, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.UID).ThrowIfNull(Constants.ContactGroup.Property.UID);
        contactGroup.UniqueId = property.ToString();
    }

    /// <summary>Reads the N property.</summary>
    private static void Read_N(ContactGroup card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.N).ThrowIfNull(Constants.ContactGroup.Property.N);
        var values = property?.ToString()?.Split(';');
        card.Name = values?[0];
    }

    /// <summary>Reads the FN property.</summary>
    private static void Read_FN(ContactGroup contactGroup, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.FN).ThrowIfNull(Constants.ContactGroup.Property.FN);
        contactGroup.FormattedName = property.ToString();
    }

    /// <summary>Reads the X-ADDRESSBOOKSERVER-KIND property.</summary>
    private static void Read_X_ADDRESSBOOKSERVER_KIND(ContactGroup card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_KIND).ThrowIfNull(Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_KIND);
        card.GroupType = property.ToString();
    }

    /// <summary>Reads the X-ADDRESSBOOKSERVER-MEMBER property.</summary>
    private static void Read_X_ADDRESSBOOKSERVER_MEMBER(ContactGroup card, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_MEMBER).ThrowIfNull(Constants.ContactGroup.Property.X_ADDRESSBOOKSERVER_MEMBER);
        var memberResourceName = property.ToString().Replace(Constants.ContactGroup.urn_Prefix, null);
        card.MemberResourceNames.Add(memberResourceName);
    }

    /// <summary>Reads the PRODID property.</summary>
    private static void Read_PRODID(ContactGroup contactGroup, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.PRODID).ThrowIfNull(Constants.ContactGroup.Property.PRODID);
        contactGroup.ProductId = property.ToString();
    }

    /// <summary>Reads the REV property.</summary>
    private static void Read_REV(ContactGroup contactGroup, IEnumerable<CardProperty> properties)
    {
        var property = properties.FindByName(Constants.ContactGroup.Property.REV).ThrowIfNull(Constants.ContactGroup.Property.REV);
        contactGroup.RevisionDate = DateTimeHelper.ParseDate(property.ToString());
    }
}
