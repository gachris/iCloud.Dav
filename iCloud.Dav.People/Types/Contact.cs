using iCloud.Dav.Core;
using iCloud.Dav.People.Serialization.Converters;
using System;
using System.ComponentModel;

namespace iCloud.Dav.People.Types;

/// <inheritdoc/>
[Serializable]
[TypeConverter(typeof(ContactConverter))]
public class Contact : vCard.Net.Types.Contact, IDirectResponseSchema
{
    /// <inheritdoc/>
    public virtual string? ETag { get; set; }
}