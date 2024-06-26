﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using vCard.Net;
using vCard.Net.CardComponents;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.DataTypes;

/// <summary>
/// A class that represents a vCard contact group object.
/// </summary>
[TypeConverter(typeof(ContactGroupConverter))]
public class ContactGroup : UniqueComponent, IDirectResponseSchema, IUrlPath
{
    /// <summary>
    /// Gets or sets the e-tag associated with this contact group.
    /// </summary>
    /// <remarks>
    /// Will be set by the service deserialization method,
    /// or the by xml response parser if implemented on service.
    /// </remarks>
    public virtual string ETag { get; set; }

    /// <summary>
    /// Gets or sets the id associated with this contact group.
    /// </summary>
    /// <remarks>
    /// A value that uniquely identifies the vCard. 
    /// It is used for requests and in most cases has the same value as the <seealso cref="UniqueComponent.Uid"/>.
    /// The initial value of Id is same as the <seealso cref="UniqueComponent.Uid"/>
    /// </remarks>
    public virtual string Id { get; set; }

    /// <summary>
    /// Gets or sets the product identifier associated with this contact group.
    /// </summary>
    public virtual string ProductId
    {
        get => Properties.Get<string>("PRODID");
        set => Properties.Set("PRODID", value);
    }

    /// <summary>
    /// Gets or sets the revision date associated with this contact group.
    /// </summary>
    /// <remarks>
    ///     The revision date is not automatically updated by the
    ///     card when modifying properties. It is up to the
    ///     developer to change the revision date as needed.
    /// </remarks>
    public virtual IDateTime RevisionDate
    {
        get => Properties.Get<IDateTime>("REV");
        set => Properties.Set("REV", value);
    }

    /// <summary>
    /// Gets or sets formatted name associated with this contact group.
    /// </summary>
    /// <remarks>
    ///     This property allows the name of the vCard to be
    ///     written in a manner specific to his or her culture.
    ///     The formatted name is not required to strictly
    ///     correspond with the family name, given name, etc.
    /// </remarks>
    public virtual string FormattedName
    {
        get => Properties.Get<string>("FN");
        set => Properties.Set("FN", value);
    }

    /// <summary>
    /// Gets or sets the name associated with this contact group.
    /// </summary>
    public virtual string N
    {
        get => Properties.Get<string>("N");
        set => Properties.Set("N", value);
    }

    /// <summary>
    /// Gets or sets the kind associated with this contact group.
    /// </summary>
    public Kind Kind
    {
        get => Properties.Get<Kind>("X-ADDRESSBOOKSERVER-KIND");
        set => Properties.Set("X-ADDRESSBOOKSERVER-KIND", value);
    }

    /// <summary>
    /// Gets or sets the list of members associated with this contact group.
    /// </summary>
    public virtual IList<string> Members
    {
        get => Properties.GetMany<string>("X-ADDRESSBOOKSERVER-MEMBER");
        set => Properties.Set("X-ADDRESSBOOKSERVER-MEMBER", value);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ContactGroup" /> class.
    /// </summary>
    public ContactGroup()
    {
        Name = Components.VCARD;
        EnsureProperties();
    }

    /// <summary>
    /// Method that is called after the object is deserialized. It ensures that the object has all required properties.
    /// </summary>
    /// <param name="context">The <see cref="StreamingContext"/> that contains contextual information about the source or destination.</param>
    protected override void OnDeserialized(StreamingContext context)
    {
        base.OnDeserialized(context);

        EnsureProperties();
    }

    /// <summary>
    /// Ensures contact group properties.
    /// </summary>
    private void EnsureProperties()
    {
        if (string.IsNullOrEmpty(Uid))
        {
            Uid = Guid.NewGuid().ToString();
        }

        if (string.IsNullOrEmpty(Id))
        {
            Id = Uid;
        }

        Kind ??= new Kind()
        {
            CardKind = CardKind.Group
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object obj)
    {
        return !(obj is null) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((ContactGroup)obj));
    }

    /// <summary>
    /// Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="obj">The object to compare with the current object.</param>
    /// <returns><see langword = "true" /> if the specified object is equal to the current object; otherwise, <see langword = "false" />.</returns>
    protected bool Equals(ContactGroup obj)
    {
        return obj == null ? false : CompareTo(obj) == 0;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return Uid?.GetHashCode() ?? base.GetHashCode();
    }
}