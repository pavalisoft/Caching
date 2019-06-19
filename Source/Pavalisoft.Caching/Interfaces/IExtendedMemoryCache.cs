using Pavalisoft.Caching.Cache;
using System.Threading;
using System.Threading.Tasks;

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents a distributed cache of serialized values.
    /// </summary>
    public interface IExtendedMemoryCache
    {
        /// <summary> Gets a value with the given key. </summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <returns>The located value or null.</returns>
        object Get(string key);

        /// <summary>Gets a value with the given key.</summary>
        /// <param name="key">A string identifying the requested value. </param>
        /// <param name="token">(Optional) A token that allows processing to be cancelled. </param>
        /// <returns>The System.Threading.Tasks.Task that represents the asynchronous operation, containing the located value or null.</returns>
        Task<object> GetAsync(string key, CancellationToken token = default);

        /// <summary>Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).</summary>
        /// <param name="key">A string identifying the requested calue.</param>
        void Refresh(string key);

        /// <summary>Refreshes a value in the cache based on its key, resetting its sliding expiration timeout (if any).</summary>
        /// <param name="key">A string identifying the requested value. </param>
        /// <param name="token">Optional. <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/>that represents the asynchronous operation.</returns>
        Task RefreshAsync(string key, CancellationToken token = default);

        /// <summary>Removes the value with the given key.</summary>
        /// <param name="key">A string identifying the requested value.</param>
        void Remove(string key);

        /// <summary>Removes the value with the given key.</summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="token">(Optional) Optional. <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled. </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task RemoveAsync(string key, CancellationToken token = default);

        /// <summary>Sets a value with the given key.</summary>
        /// <param name="key">A string identifying the requested value.</param>
        /// <param name="value">The value to set in the cache.</param>
        /// <param name="options">The cache options for the value.</param>
        void Set(string key, object value, ExtendedDistributedCacheEntryOptions options);

        /// <summary>Sets the value with the given key.</summary>
        /// <param name="key">A string identifying the requested value. </param>
        /// <param name="value">The value to set in the cache. </param>
        /// <param name="options">The cache options for the value. </param>
        /// <param name="token">(Optional) Optional. <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled. </param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetAsync(string key, object value, ExtendedDistributedCacheEntryOptions options, CancellationToken token = default);
    }
}