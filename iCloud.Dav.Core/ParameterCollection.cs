using iCloud.Dav.Core.Utils;
using System;
using System.Collections;
using System.Collections.Generic;

namespace iCloud.Dav.Core
{
    /// <summary>A collection of parameters (key value pairs). May contain duplicate keys.</summary>
    public class ParameterCollection : List<KeyValuePair<string, string>>
    {
        /// <summary>Constructs a new parameter collection.</summary>
        public ParameterCollection()
        {
        }

        /// <summary>Constructs a new parameter collection from the given collection.</summary>
        public ParameterCollection(IEnumerable<KeyValuePair<string, string>> collection) : base(collection)
        {
        }

        /// <summary>Adds a single parameter to this collection.</summary>
        public void Add(string key, string value) => Add(new KeyValuePair<string, string>(key, value));

        /// <summary>Returns <c>true</c> if this parameter is set within the collection.</summary>
        public bool ContainsKey(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            return TryGetValue(key, out var str);
        }

        /// <summary>
        /// Tries to find the a key within the specified key value collection. Returns true if the key was found.
        /// If a pair was found the out parameter value will contain the value of that pair.
        /// </summary>
        public bool TryGetValue(string key, out string value)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            foreach (var keyValuePair in this)
            {
                if (keyValuePair.Key.Equals(key))
                {
                    value = keyValuePair.Value;
                    return true;
                }
            }
            value = null;
            return false;
        }

        /// <summary>
        /// Returns the value of the first matching key, or throws a KeyNotFoundException if the parameter is not
        /// present within the collection.
        /// </summary>
        public string GetFirstMatch(string key)
        {
            if (!TryGetValue(key, out var str)) throw new KeyNotFoundException("Parameter with the name '" + key + "' was not found.");
            return str;
        }

        /// <summary>
        /// Returns all matches for the specified key. May return an empty enumeration if the key is not present.
        /// </summary>
        public IEnumerable<string> GetAllMatches(string key)
        {
            key.ThrowIfNullOrEmpty(nameof(key));
            foreach (var keyValuePair in this)
            {
                if (keyValuePair.Key.Equals(key))
                    yield return keyValuePair.Value;
            }
        }

        /// <summary>
        /// Returns all matches for the specified key. May return an empty enumeration if the key is not present.
        /// </summary>
        public IEnumerable<string> this[string key] => GetAllMatches(key);

        /// <summary>
        /// Creates a parameter collection from the specified URL encoded query string.
        /// Example:
        ///     The query string "foo=bar&amp;chocolate=cookie" would result in two parameters (foo and bar)
        ///     with the values "bar" and "cookie" set.
        /// </summary>
        public static ParameterCollection FromQueryString(string qs)
        {
            var parameterCollection = new ParameterCollection();
            var str1 = qs;
            var chArray = new char[1] { '&' };
            foreach (var str2 in str1.Split(chArray))
            {
                var strArray = str2.Split('=');
                if (strArray.Length == 2)
                    parameterCollection.Add(Uri.UnescapeDataString(strArray[0]), Uri.UnescapeDataString(strArray[1]));
                else
                    throw new ArgumentException(string.Format("Invalid query string [{0}]. Invalid part [{1}]", qs, str2));
            }
            return parameterCollection;
        }

        /// <summary>
        /// Creates a parameter collection from the specified dictionary.
        /// If the value is an enumerable, a parameter pair will be added for each value.
        /// Otherwise the value will be converted into a string using the .ToString() method.
        /// </summary>
        public static ParameterCollection FromDictionary(IDictionary<string, object> dictionary)
        {
            var parameterCollection = new ParameterCollection();
            foreach (var keyValuePair in dictionary)
            {
                if (!(keyValuePair.Value is string) && keyValuePair.Value is IEnumerable enumerable)
                {
                    foreach (var o in enumerable)
                        parameterCollection.Add(keyValuePair.Key, Utilities.ConvertToString(o));
                }
                else
                    parameterCollection.Add(keyValuePair.Key, keyValuePair.Value == null ? null : Utilities.ConvertToString(keyValuePair.Value));
            }
            return parameterCollection;
        }
    }
}