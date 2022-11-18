using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <inheritdoc/>
[Serializable]
[TypeConverter(typeof(ContactGroupConverter))]
public class ContactGroup : vCard.Net.Data.ContactGroup, IDirectResponseSchema
{
    /// <inheritdoc/>
    public virtual string? ETag { get; set; }
}
