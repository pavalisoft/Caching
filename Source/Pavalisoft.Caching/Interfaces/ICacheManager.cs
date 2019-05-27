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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents <see cref="ICacheManager"/> implementation
    /// </summary>
    public interface ICacheManager
    {
        /// <summary>
        /// Gets cache item having specified cache <paramref name="key"/> form the specified cache <paramref name="partitionName"/>
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        /// <returns>Cache Item of <typeparamref name="TItem"/></returns>
        TItem Get<TItem>(string partitionName, string key);

        /// <summary>
        /// Gets cache item having specified cache <paramref name="key"/> form the specified cache <paramref name="partitionName"/>
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while getting cache item</param>
        /// <returns>Cache Item of <typeparamref name="TItem"/></returns>
        Task<TItem> GetAsync<TItem>(string partitionName, string key,
            CancellationToken token = default);

        /// <summary>
        /// Adds an object to distributed cache
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="expirationToken">Cache <see cref="IChangeToken"/> expiration token to be used while adding cache item</param>
        /// <param name="postEvictionCallback"><see cref="PostEvictionCallbackRegistration"/> delegate</param>
        void Set<TItem>(string partitionName, string key, TItem value, IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null);

        /// <summary>
        /// Adds an object to distributed cache asynchronously
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="expirationToken">Cache <see cref="IChangeToken"/> expiration token to be used while adding cache item</param>
        /// <param name="postEvictionCallback"><see cref="PostEvictionCallbackRegistration"/> delegate</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        Task SetAsync<TItem>(string partitionName, string key, TItem value,
            IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null,
            CancellationToken token = default);

        /// <summary>
        /// Adds an object to distributed cache asynchronously without post eviction callback
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="expirationToken">Cache <see cref="IChangeToken"/> expiration token to be used while adding cache item</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        Task SetAsync<TItem>(string partitionName, string key, TItem value, IChangeToken expirationToken,
            CancellationToken token = default);

        /// <summary>
        /// Adds an object to distributed cache asynchronously without expiration change token
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="postEvictionCallback"><see cref="PostEvictionCallbackRegistration"/> delegate</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        Task SetAsync<TItem>(string partitionName, string key, TItem value,
            PostEvictionCallbackRegistration postEvictionCallback,
            CancellationToken token = default);

        /// <summary>
        /// Refreshes the cache item of the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        void Refresh(string partitionName, string key);

        /// <summary>
        /// Refreshes the cache item asynchronously for the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while refreshing cache item</param>
        Task RefreshAsync(string partitionName, string key,
            CancellationToken token = default);

        /// <summary>
        /// Removes the Cache object from the cache for the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        void Remove(string partitionName, string key);

        /// <summary>
        /// Removes the Cache object asynchronously from the cache for the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while removing cache item</param>
        Task RemoveAsync(string partitionName, string key,
            CancellationToken token = default);
    }
}
