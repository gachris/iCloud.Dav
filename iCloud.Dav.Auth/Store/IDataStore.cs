using System.Threading.Tasks;

namespace iCloud.Dav.Auth.Store;

/// <summary>IDataStore</summary>
public interface IDataStore
{
    /// <summary>
    /// Clears all values in the data store.
    /// </summary>
    Task ClearAsync();

    /// <summary>
    /// Deletes the given key.
    /// </summary>
    /// <param name="key">The key to delete from the data store.</param>
    Task DeleteAsync<T>(string key);

    /// <summary>
    /// Returns the stored value for the given key.
    /// </summary>
    /// <typeparam name="T">The type to retrieve.</typeparam>
    /// <param name="key">The key to retrieve from the data store.</param>
    /// <returns>The stored object.</returns>
    Task<T?> GetAsync<T>(string key);

    /// <summary>
    /// Stores the given value for the given key.
    /// </summary>
    /// <typeparam name="T">The type to store in the data store.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value to store in the data store.</param>
    Task StoreAsync<T>(string key, T value);
}
