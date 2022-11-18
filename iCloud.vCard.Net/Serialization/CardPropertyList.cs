using iCloud.vCard.Net.Data;
using iCloud.vCard.Net.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace iCloud.vCard.Net.Serialization;

public class CardPropertyList : IList<CardProperty>
{
    private readonly List<CardProperty> _list = new();

    public CardPropertyList()
    {
    }

    public CardPropertyList(IEnumerable<CardProperty> cardProperties) => AddRange(cardProperties);

    #region Implementation of IEnumerable

    public IEnumerator<CardProperty> GetEnumerator() => _list.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    #endregion

    #region Implementation of ICollection<T>

    public void Add(CardProperty item) => _list.Add(item);

    public void Clear() => _list.Clear();

    public bool Contains(CardProperty item) => _list.Contains(item);

    public bool Contains(string name) => _list.Select(x => x.Name).Contains(name);

    public void CopyTo(CardProperty[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

    public bool Remove(CardProperty item) => _list.Remove(item);

    public int Count => _list.Count;

    public bool IsReadOnly => ((IList)_list).IsReadOnly;

    #endregion

    #region Implementation of IList<T>

    public int IndexOf(CardProperty item) => _list.IndexOf(item);

    public void Insert(int index, CardProperty item) => _list.Insert(index, item);

    public void RemoveAt(int index) => _list.RemoveAt(index);

    public CardProperty this[int index]
    {
        get => _list[index];
        set => _list[index] = value;
    }

    #endregion

    public void AddRange(IEnumerable<CardProperty> properties) => _list.AddRange(properties);

    public CardProperty? FindByName(string name) => _list.FirstOrDefault(x => x.Name == name);

    public T? Get<T>(string name)
    {
        var value = FindByName(name)?.Value;

        if (typeof(CardDataType).IsAssignableFrom(typeof(T)))
        {
            var list = new List<T>();

            var properties = _list.Where(x => x.Name == name);

            var allProperties = _list.Where(x => properties.Select(y => y.Group).Contains(x.Group)).ToList();
            allProperties.AddRange(properties);

            allProperties = allProperties.Distinct().ToList();

            var groups = allProperties.GroupBy(x => x.Group);

            foreach (var item in groups)
            {
                if (Activator.CreateInstance(typeof(T), new CardPropertyList(item.Select(x => x))) is T t)
                {
                    list.Add(t);
                }
            }

            return list.FirstOrDefault();
        }
        else if (typeof(T) == typeof(DateTime?))
        {
            return (T?)(object?)DateTimeHelper.ParseDate(value?.ToString());
        }
        else
        {
            return (T?)value;
        }
    }

    public List<T> GetMany<T>(string name)
    {
        var list = new List<T>();

        var properties = this.Where(x => x.Name == name);

        var allProperties = this.Where(x => properties.Select(y => y.Group).Contains(x.Group)).ToList();
        allProperties.AddRange(properties);

        allProperties = allProperties.Distinct().ToList();

        var groups = allProperties.GroupBy(x => x.Group);

        foreach (var item in groups)
        {
            list.Add((T)Activator.CreateInstance(typeof(T), new CardPropertyList(item.Select(x => x))));
        }
        return list;
    }

    public void Set<T>(string name, T value)
    {
        if (value is not null && typeof(T) == typeof(Organization))
        {
            var oRGSerializer = new OrganizationSerializer();
            var property = oRGSerializer.Serialize((Organization)(object)value).First();

            if (FindByName(name) is CardProperty cardProperty2)
            {
                Remove(cardProperty2);
            }

            Add(property);

            return;
        }
        if (value is not null && typeof(T) == typeof(Name))
        {
            var nameSerializer = new NameSerializer();
            var property = nameSerializer.Serialize((Name)(object)value).First();

            if (FindByName(name) is CardProperty cardProperty2)
            {
                Remove(cardProperty2);
            }

            Add(property);

            return;
        }

        if (FindByName(name) is CardProperty cardProperty1)
        {
            if (value == null)
            {
                Remove(cardProperty1);
            }
            else
            {
                cardProperty1.Value = value;
            }
        }
        else if (value != null)
        {
            var cardProperty = new CardProperty(name)
            {
                Value = value
            };
            Add(cardProperty);
        }
    }
}
