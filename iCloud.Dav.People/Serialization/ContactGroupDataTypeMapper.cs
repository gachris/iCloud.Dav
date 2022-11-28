using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using vCard.Net;
using vCard.Net.DataTypes;

namespace iCloud.Dav.People.Serialization
{
    internal class ContactGroupDataTypeMapper
    {
        private class PropertyMapping
        {
            public Type ObjectType { get; set; }
            public TypeResolverDelegate Resolver { get; set; }
            public bool AllowsMultipleValuesPerProperty { get; set; }
        }

        private readonly IDictionary<string, PropertyMapping> _propertyMap = new Dictionary<string, PropertyMapping>(StringComparer.OrdinalIgnoreCase);

        public ContactGroupDataTypeMapper()
        {
            AddPropertyMapping("REV", typeof(IDateTime), false);
            AddPropertyMapping("X-ADDRESSBOOKSERVER-KIND", typeof(Kind), false);
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

            var name = Regex.Replace(p.Name, @"^ITEM(\d+).", replace => string.Empty);

            return !_propertyMap.TryGetValue(name, out var m)
                ? null
                : m.Resolver == null
                ? m.ObjectType
                : m.Resolver(p);
        }
    }
}