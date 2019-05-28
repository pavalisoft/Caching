/* 
   Copyright 2019 Pavalisoft

   Licensed under the Apache License, Version 2.0 (the "License");
   you may not use this file except in compliance with the License.
   You may obtain a copy of the License at

       http://www.apache.org/licenses/LICENSE-2.0

   Unless required by applicable law or agreed to in writing, software
   distributed under the License is distributed on an "AS IS" BASIS,
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
   See the License for the specific language governing permissions and
   limitations under the License. 
*/

using System.Threading;
using System.Threading.Tasks;
using Pavalisoft.Caching.Cache;

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents a <see cref="ICache" /> in <see cref="ICachePartition"/> implementation
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Gets the Cache object for the specified cache key
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached object</returns>
        TItem Get<TItem>(string key);

        /// <summary>
        /// Gets the Cache object asynchronously for the specified cache key
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while getting cache item</param>
        /// <returns>Cached object</returns>
        Task<TItem> GetAsync<TItem>(string key, CancellationToken token = default);

        /// <summary>
        /// Adds an object to distributed cache
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="ExtendedDistributedCacheEntryOptions"/></param>
        void Set<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options);

        /// <summary>
        /// Adds an object to distributed cache asynchronously
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="ExtendedDistributedCacheEntryOptions"/></param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        Task SetAsync<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options, CancellationToken token = default);

        /// <summary>
        /// Refreshes the cache item of the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        void Refresh(string key);

        /// <summary>
        /// Refreshes the cache item asynchronously for the specified cache key
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while refreshing cache item</param>
        Task RefreshAsync(string key, CancellationToken token = default);

        /// <summary>
        /// Removes the Cache object from the cache for the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        void Remove(string key);

        /// <summary>
        /// Removes the Cache object asynchronously from the cache for the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while removing cache item</param>
        Task RemoveAsync(string key, CancellationToken token = default);

        /// <summary>
        /// Gets the <see cref="ICacheStore{T}"/> for the type <typeparamref name="T"/> from cache manager
        /// </summary>
        /// <typeparam name="T">Cache Store Type</typeparam>
        /// <returns><see cref="ICacheStore{T}"/></returns>
        ICacheStore<T> GetCacheStore<T>();

        /// <summary>
        /// Sets the <see cref="ICacheStore{T}"/> of the type <typeparamref name="T"/> to the cache manager
        /// </summary>
        /// <typeparam name="T">Cache Store type</typeparam>
        /// <param name="cacheStore">Cache Store</param>
        void SetCacheStore<T>(ICacheStore<T> cacheStore);
    }
}
