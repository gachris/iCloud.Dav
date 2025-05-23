﻿using iCloud.Dav.People.DataTypes;

namespace iCloud.Dav.People.Serialization;

/// <summary>
/// Maps vCard properties to .NET data types.
/// </summary>
internal class ContactDataTypeMapper
{
    private class PropertyMapping
    {
        /// <summary>
        /// Gets or sets the type of the object to map to.
        /// </summary>
        public Type ObjectType { get; set; }

        /// <summary>
        /// Gets or sets the resolver delegate to use.
        /// </summary>
        public TypeResolverDelegate Resolver { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the property allows multiple values.
        /// </summary>
        public bool AllowsMultipleValuesPerProperty { get; set; }
    }

    private readonly IDictionary<string, PropertyMapping> _propertyMap = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);

    /// <summary>
    /// Initializes a new instance of the ContactDataTypeMapper class.
    /// </summary>
    public ContactDataTypeMapper()
    {
        AddPropertyMapping("ADR", typeof(Address), true);
        AddPropertyMapping("EMAIL", typeof(Email), true);
        AddPropertyMapping("TEL", typeof(Phone), true);
        AddPropertyMapping("ORG", typeof(vCard.Net.DataTypes.Organization), false);
        AddPropertyMapping("PHOTO", typeof(Photo), false);
        AddPropertyMapping("URL", typeof(Website), true);
        AddPropertyMapping("REV", typeof(vCard.Net.DataTypes.IDateTime), false); 
        AddPropertyMapping("BDAY", typeof(Birthdate), false);
        AddPropertyMapping("N", typeof(vCard.Net.DataTypes.StructuredName), false);
        AddPropertyMapping("X-ABDATE", typeof(Date), true);
        AddPropertyMapping("X-ABRELATEDNAMES", typeof(RelatedNames), true);
        AddPropertyMapping("X-SOCIALPROFILE", typeof(SocialProfile), true);
        AddPropertyMapping("X-ADDRESSBOOKSERVER-KIND", typeof(vCard.Net.DataTypes.Kind), false);
        AddPropertyMapping("X-ABLABEL", typeof(Label), true);
        AddPropertyMapping("X-ABADR", typeof(X_ABAddress), true);
        AddPropertyMapping("IMPP", typeof(InstantMessage), true);
    }

    /// <summary>
    /// Adds a property mapping.
    /// </summary>
    /// <param name="name">The name of the vCard property.</param>
    /// <param name="objectType">The type of the .NET object to map to.</param>
    /// <param name="allowsMultipleValues">Indicates whether the property allows multiple values.</param>
    public void AddPropertyMapping(string name, Type objectType, bool allowsMultipleValues)
    {
        if (name == null || objectType == null)
        {
            return;
        }

        var m = new PropertyMapping
        {
            ObjectType = objectType,
            AllowsMultipleValuesPerProperty = allowsMultipleValues
        };

        _propertyMap[name] = m;
    }

    /// <summary>
    /// Adds a property mapping.
    /// </summary>
    /// <param name="name">The name of the vCard property.</param>
    /// <param name="resolver">The resolver delegate to use.</param>
    /// <param name="allowsMultipleValues">Indicates whether the property allows multiple values.</param>
    public void AddPropertyMapping(string name, TypeResolverDelegate resolver, bool allowsMultipleValues)
    {
        if (name == null || resolver == null)
        {
            return;
        }

        var m = new PropertyMapping
        {
            Resolver = resolver,
            AllowsMultipleValuesPerProperty = allowsMultipleValues
        };

        _propertyMap[name] = m;
    }

    /// <summary>
    /// Removes a property mapping.
    /// </summary>
    /// <param name="name">The name of the vCard property.</param>
    public void RemovePropertyMapping(string name)
    {
        if (name != null && _propertyMap.ContainsKey(name))
        {
            _propertyMap.Remove(name);
        }
    }

    /// <summary>
    /// Gets a value indicating whether the property allows multiple values.
    /// </summary>
    /// <param name="obj">The object to check.</param>
    /// <returns>true if the property allows multiple values; otherwise, false.</returns>
    public virtual bool GetPropertyAllowsMultipleValues(object obj)
    {
        var p = obj as vCard.Net.IVCardProperty;
        return !string.IsNullOrWhiteSpace(p?.Name)
            && _propertyMap.TryGetValue(p.Name, out var m)
            && m.AllowsMultipleValuesPerProperty;
    }

    /// <summary>
    /// Gets the .NET type that corresponds to the vCard property.
    /// </summary>
    /// <param name="obj">The vCard property.</param>
    /// <returns>The .NET type that corresponds to the vCard property.</returns>
    public virtual Type GetPropertyMapping(object obj)
    {
        var p = obj as vCard.Net.IVCardProperty;
        return p?.Name == null
            ? null
            : !_propertyMap.TryGetValue(p.Name, out var m)
            ? null
            : m.Resolver == null
            ? m.ObjectType
            : m.Resolver(p);
    }
}