﻿using System.Text;
using iCloud.Dav.People.DataTypes;
using iCloud.Dav.People.Serialization;

namespace iCloud.Dav.People.Extensions;

internal static class ContactGroupExtensions
{
    public static ContactGroup ToContactGroup(this string data)
    {
        if (!data.Contains(WebDavExtensions.GroupKind))
            throw new ArgumentException("Value cannot be converted to a Contact Group.");

        var bytes = Encoding.UTF8.GetBytes(data);
        using var stream = new MemoryStream(bytes);
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return ContactGroupDeserializer.Default.Deserialize(reader).First();
    }

    public static string SerializeToString(this ContactGroup data)
    {
        var serializer = new ContactGroupSerializer();
        return serializer.SerializeToString(data);
    }
}