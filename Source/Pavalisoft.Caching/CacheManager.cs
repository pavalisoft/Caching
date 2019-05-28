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

using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Primitives;
using Pavalisoft.Caching.Cache;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides Cache Api <see cref="ICacheManager"/>to interact with Cache Partitions for cache
    /// </summary>
    public class CacheManager : ICacheManager
    {
        private readonly ICacheSettingsProvider _cacheSettingsProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, ICachePartition> _cachePartitions;

        /// <summary>
        /// Creates an instance of <see cref="CacheManager"/> with cache manager settings <see cref="ICacheSettingsProvider"/>
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
        /// <param name="cacheSettingsProvider"><see cref="ICacheSettingsProvider"/> provides cache manager configuration</param>
        public CacheManager(IServiceProvider serviceProvider, ICacheSettingsProvider cacheSettingsProvider)
        {
            _serviceProvider = serviceProvider;
            _cacheSettingsProvider = cacheSettingsProvider;
            _cachePartitions = new ConcurrentDictionary<string, ICachePartition>();
        }

        /// <summary>
        /// Gets cache item having specified cache <paramref name="key"/> form the specified cache <paramref name="partitionName"/>
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        /// <returns>Cache Item of <typeparamref name="TItem"/></returns>
        public TItem Get<TItem>(string partitionName, string key)
        {
            return GetPartition(partitionName).Cache.Get<TItem>(partitionName + key);
        }

        /// <summary>
        /// Gets cache item having specified cache <paramref name="key"/> form the specified cache <paramref name="partitionName"/>
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while getting cache item</param>
        /// <returns>Cache Item of <typeparamref name="TItem"/></returns>
        public async Task<TItem> GetAsync<TItem>(string partitionName, string key,
            CancellationToken token = default)
        {
            return await GetPartition(partitionName).Cache.GetAsync<TItem>(partitionName + key, token);
        }

        /// <summary>
        /// Adds an object to distributed cache
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="expirationToken">Cache <see cref="IChangeToken"/> expiration token to be used while adding cache item</param>
        /// <param name="postEvictionCallback"><see cref="PostEvictionCallbackRegistration"/> delegate</param>
        public void Set<TItem>(string partitionName, string key, TItem value, IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null)
        {
            var partition = GetPartition(partitionName);
            partition.Cache.Set(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition, expirationToken, postEvictionCallback));
        }

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
        public async Task SetAsync<TItem>(string partitionName, string key, TItem value,
            IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null,
            CancellationToken token = default)
        {
            var partition = GetPartition(partitionName);
            await partition.Cache.SetAsync(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition, expirationToken, postEvictionCallback), token);
        }

        /// <summary>
        /// Adds an object to distributed cache asynchronously without post eviction callback
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="expirationToken">Cache <see cref="IChangeToken"/> expiration token to be used while adding cache item</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        public async Task SetAsync<TItem>(string partitionName, string key, TItem value, IChangeToken expirationToken,
            CancellationToken token = default)
        {
            var partition = GetPartition(partitionName);
            await partition.Cache.SetAsync(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition), token);
        }

        /// <summary>
        /// Adds an object to distributed cache asynchronously without expiration change token
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="postEvictionCallback"><see cref="PostEvictionCallbackRegistration"/> delegate</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        public async Task SetAsync<TItem>(string partitionName, string key, TItem value,
            PostEvictionCallbackRegistration postEvictionCallback,
            CancellationToken token = default)
        {
            var partition = GetPartition(partitionName);
            await partition.Cache.SetAsync(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition), token);
        }

        /// <summary>
        /// Refreshes the cache item of the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        public void Refresh(string partitionName, string key)
        {
            GetPartition(partitionName).Cache.Refresh(partitionName + key);
        }

        /// <summary>
        /// Refreshes the cache item asynchronously for the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache Key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while refreshing cache item</param>
        public async Task RefreshAsync(string partitionName, string key,
            CancellationToken token = default)
        {
            await GetPartition(partitionName).Cache.RefreshAsync(partitionName + key, token);
        }

        /// <summary>
        /// Removes the Cache object from the cache for the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        public void Remove(string partitionName, string key)
        {
            GetPartition(partitionName).Cache.Remove(partitionName + key);
        }

        /// <summary>
        /// Removes the Cache object asynchronously from the cache for the specified cache key
        /// </summary>
        /// <param name="partitionName">Cache Partition Name</param>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while removing cache item</param>
        public async Task RemoveAsync(string partitionName, string key,
            CancellationToken token = default)
        {
            await GetPartition(partitionName).Cache.RemoveAsync(partitionName + key, token);
        }

        private ICachePartition GetPartition(string partitionName)
        {
            if (!_cachePartitions.TryGetValue(partitionName, out var partition))
            {
                partition = GetCacheFactory(partitionName);
                _cachePartitions.TryAdd(partitionName, partition);
            }
            return partition;
        }

        private ICachePartition GetCacheFactory(string partitionName)
        {
            ICachePartition partition = _cacheSettingsProvider.GetCachePartition(partitionName);
            var store = partition.Store;
            ICache cache = null;
            if (store is ICacheStore<MemoryDistributedCacheOptions> memoryStore)
            {
                cache = new Cache.Cache(_serviceProvider.GetService(memoryStore.CacheType) as IExtendedDistributedCache);
                cache.SetCacheStore(memoryStore);
            }
            else if (store is ICacheStore<RedisCacheOptions> redisStore)
            {
                cache = new Cache.Cache(_serviceProvider.GetService(redisStore.CacheType) as IExtendedDistributedCache);
                cache.SetCacheStore(redisStore);
            }
            else if (store is ICacheStore<SqlServerCacheOptions> sqlStore)
            {
                cache = new Cache.Cache(_serviceProvider.GetService(sqlStore.CacheType) as IExtendedDistributedCache);
                cache.SetCacheStore(sqlStore);
            }
            partition.Cache = cache;
            return partition;
        }

        private ExtendedDistributedCacheEntryOptions GetDistributedCacheEntryOptions(ICachePartition cachePartition,
            IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null)
        {
            var cacheEntryOptions = new ExtendedDistributedCacheEntryOptions
            {
                AbsoluteExpiration = cachePartition.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = cachePartition.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = cachePartition.SlidingExpiration,
                Priority = cachePartition.Priority
            };
            if (expirationToken != null)
                cacheEntryOptions.ExpirationTokens.Add(expirationToken);
            if (postEvictionCallback != null)
                cacheEntryOptions.PostEvictionCallbacks.Add(postEvictionCallback);
            return cacheEntryOptions;
        }
    }
}
