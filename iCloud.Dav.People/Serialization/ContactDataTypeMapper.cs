using iCloud.Dav.People.DataTypes;
using System;
using System.Collections.Generic;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.Serialization
{
    internal class ContactDataTypeMapper
    {
        private class PropertyMapping
        {
            public Type ObjectType { get; set; }
            public TypeResolverDelegate Resolver { get; set; }
            public bool AllowsMultipleValuesPerProperty { get; set; }
        }

        private readonly IDictionary<string, PropertyMapping> _propertyMap = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);

        public ContactDataTypeMapper()
        {
            AddPropertyMapping("ADR", typeof(Address), true);
            AddPropertyMapping("EMAIL", typeof(Email), true);
            AddPropertyMapping("TEL", typeof(Phone), true);
            AddPropertyMapping("ORG", typeof(Organization), false);
            AddPropertyMapping("PHOTO", typeof(Photo), false);
            AddPropertyMapping("URL", typeof(Website), true);
            AddPropertyMapping("REV", typeof(IDateTime), false);
            AddPropertyMapping("BDAY", typeof(IDateTime), false);
            AddPropertyMapping("N", typeof(Name), false);
            AddPropertyMapping("X-ABDATE", typeof(Date), true);
            AddPropertyMapping("X-ABRELATEDNAMES", typeof(RelatedNames), true);
            AddPropertyMapping("X-SOCIALPROFILE", typeof(SocialProfile), true);
            AddPropertyMapping("X-ADDRESSBOOKSERVER-KIND", typeof(Kind), false);
            AddPropertyMapping("X-ABLABEL", typeof(Label), true);
            AddPropertyMapping("X-ABADR", typeof(X_ABAddress), true);
            AddPropertyMapping("IMPP", typeof(InstantMessage), true);
        }

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

        public void RemovePropertyMapping(string name)
        {
            if (name != null && _propertyMap.ContainsKey(name))
            {
                _propertyMap.Remove(name);
            }
        }

        public virtual bool GetPropertyAllowsMultipleValues(object obj)
        {
            var p = obj as ICardProperty;
            return !string.IsNullOrWhiteSpace(p?.Name)
                && _propertyMap.TryGetValue(p.Name, out var m)
                && m.AllowsMultipleValuesPerProperty;
        }

        public virtual Type GetPropertyMapping(object obj)
        {
            var p = obj as ICardProperty;
            return p?.Name == null
                ? null
                : !_propertyMap.TryGetValue(p.Name, out var m)
                ? null
                : m.Resolver == null
                ? m.ObjectType
                : m.Resolver(p);
        }
    }
}