using iCloud.Dav.People.DataTypes;
using System;
using System.Collections.Generic;
using vCard.Net;

namespace iCloud.Dav.People.Serialization
{
    internal delegate Type TypeResolverDelegate(object context);

    internal class ExtendedDataTypeMapper
    {
        private class PropertyMapping
        {
            public Type ObjectType { get; set; }
            public TypeResolverDelegate Resolver { get; set; }
            public bool AllowsMultipleValuesPerProperty { get; set; }
        }

        private readonly IDictionary<string, PropertyMapping> _propertyMap = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);

        public ExtendedDataTypeMapper()
        {
            AddPropertyMapping("ADR", typeof(Address), true);
            AddPropertyMapping("EMAIL", typeof(Email), true);
            AddPropertyMapping("TEL", typeof(Phone), true);
            AddPropertyMapping("PHOTO", typeof(Photo), true);
            AddPropertyMapping("URL", typeof(Website), true);
            AddPropertyMapping("X-ABDATE", typeof(X_ABDate), true);
            AddPropertyMapping("X-ABRELATEDNAMES", typeof(X_ABRelatedNames), true);
            AddPropertyMapping("X-SOCIALPROFILE", typeof(X_SocialProfile), true);
            AddPropertyMapping("REV", typeof(vCard.Net.DataTypes.IDateTime), false);
            AddPropertyMapping("N", typeof(vCard.Net.DataTypes.Name), false);
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
            if (p?.Name == null)
            {
                return null;
            }

            if (!_propertyMap.TryGetValue(p.Name, out var m))
            {
                return null;
            }

            return m.Resolver == null
                ? m.ObjectType
                : m.Resolver(p);
        }
    }
}