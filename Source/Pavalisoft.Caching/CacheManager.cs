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
    public class CacheManager : ICacheManager
    {
        private readonly ICacheSettingsProvider _cacheSettingsProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly ConcurrentDictionary<string, ICachePartition> _cachePartitions;
        public CacheManager(IServiceProvider serviceProvider, ICacheSettingsProvider cacheSettingsProvider)
        {
            _serviceProvider = serviceProvider;
            _cacheSettingsProvider = cacheSettingsProvider;
            _cachePartitions = new ConcurrentDictionary<string, ICachePartition>();
        }

        public TItem Get<TItem>(string partitionName, string key)
        {
            return GetPartition(partitionName).Cache.Get<TItem>(partitionName + key);
        }

        public async Task<TItem> GetAsync<TItem>(string partitionName, string key,
            CancellationToken token = default)
        {
            return await GetPartition(partitionName).Cache.GetAsync<TItem>(partitionName + key, token);
        }

        public void Set<TItem>(string partitionName, string key, TItem value, IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null)
        {
            var partition = GetPartition(partitionName);
            partition.Cache.Set(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition, expirationToken, postEvictionCallback));
        }

        public async Task SetAsync<TItem>(string partitionName, string key, TItem value,
            IChangeToken expirationToken = null,
            PostEvictionCallbackRegistration postEvictionCallback = null,
            CancellationToken token = default)
        {
            var partition = GetPartition(partitionName);
            await partition.Cache.SetAsync(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition, expirationToken, postEvictionCallback), token);
        }

        public async Task SetAsync<TItem>(string partitionName, string key, TItem value, IChangeToken expirationToken,
            CancellationToken token = default)
        {
            var partition = GetPartition(partitionName);
            await partition.Cache.SetAsync(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition), token);
        }

        public async Task SetAsync<TItem>(string partitionName, string key, TItem value,
            PostEvictionCallbackRegistration postEvictionCallback,
            CancellationToken token = default)
        {
            var partition = GetPartition(partitionName);
            await partition.Cache.SetAsync(partitionName + key, value,
                GetDistributedCacheEntryOptions(partition), token);
        }

        public void Refresh(string partitionName, string key)
        {
            GetPartition(partitionName).Cache.Refresh(partitionName + key);
        }

        public async Task RefreshAsync(string partitionName, string key,
            CancellationToken token = default)
        {
            await GetPartition(partitionName).Cache.RefreshAsync(partitionName + key, token);
        }

        public void Remove(string partitionName, string key)
        {
            GetPartition(partitionName).Cache.Remove(partitionName + key);
        }

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
