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

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Cache
{
    /// <summary>
    /// Provides distribution cache implementation of<see cref="ICache" /> in <see cref="ICachePartition"/>
    /// </summary>
    public class Cache : ICache
    {
        private readonly IExtendedDistributedCache _distributedCache;
        private ICacheStore _cacheStore;

        /// <summary>
        /// Creates an instance of <see cref="Cache"/> with <see cref="IExtendedDistributedCache"/>
        /// </summary>
        /// <param name="distributedCache"><see cref="IExtendedDistributedCache"/></param>
        /// <param name="cacheStore"><see cref="ICacheStore"/></param>
        public Cache(IExtendedDistributedCache distributedCache, ICacheStore cacheStore)
        {
            _distributedCache = distributedCache;
            _cacheStore = cacheStore;
        }

        /// <summary>
        /// Gets the Cache object for the specified cache key
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache key</param>
        /// <returns>Cached object</returns>
        public TItem Get<TItem>(string key)
        {
            byte[] cache = _distributedCache.Get(key);
            if (cache == null) return default;
            string str = Encoding.UTF8.GetString(cache);
            return JsonConvert.DeserializeObject<TItem>(str);
        }

        /// <summary>
        /// Gets the Cache object asynchronously for the specified cache key
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while getting cache item</param>
        /// <returns>Cached object</returns>
        public async Task<TItem> GetAsync<TItem>(string key, CancellationToken token = default)
        {
            byte[] cache = await _distributedCache.GetAsync(key, token);
            if (cache == null) return default;
            string str = Encoding.UTF8.GetString(cache);
            return JsonConvert.DeserializeObject<TItem>(str);
        }

        /// <summary>
        /// Adds an object to distributed cache
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="ExtendedDistributedCacheEntryOptions"/></param>
        public void Set<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options)
        {
            string str = JsonConvert.SerializeObject(value);
            byte[] val = Encoding.UTF8.GetBytes(str);
            _distributedCache.Set(key, val, options);
        }

        /// <summary>
        /// Adds an object to distributed cache asynchronously
        /// </summary>
        /// <typeparam name="TItem">Cache object type</typeparam>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="ExtendedDistributedCacheEntryOptions"/></param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        public async Task SetAsync<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            string str = JsonConvert.SerializeObject(value);
            byte[] val = Encoding.UTF8.GetBytes(str);
            await _distributedCache.SetAsync(key, val, options, token);
        }

        /// <summary>
        /// Refreshes the cache item of the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        public void Refresh(string key)
        {
            _distributedCache.Refresh(key);
        }

        /// <summary>
        /// Refreshes the cache item asynchronously for the specified cache key
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while refreshing cache item</param>
        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            await _distributedCache.RefreshAsync(key, token);
        }

        /// <summary>
        /// Removes the Cache object from the cache for the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        /// <summary>
        /// Removes the Cache object asynchronously from the cache for the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while removing cache item</param>
        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await _distributedCache.RemoveAsync(key, token);
        }

        /// <summary>
        /// Gets the <see cref="ICacheStore{T}"/> for the type <typeparamref name="T"/> from cache manager
        /// </summary>
        /// <typeparam name="T">Cache Store Type</typeparam>
        /// <returns><see cref="ICacheStore{T}"/></returns>
        public ICacheStore<T> GetCacheStore<T>()
        {
            return _cacheStore as ICacheStore<T>;
        }

        /// <summary>
        /// Sets the <see cref="ICacheStore{T}"/> of the type <typeparamref name="T"/> to the cache manager
        /// </summary>
        /// <typeparam name="T">Cache Store type</typeparam>
        /// <param name="cacheStore">Cache Store</param>
        public void SetCacheStore<T>(ICacheStore<T> cacheStore)
        {
            _cacheStore = cacheStore;
        }
    }
}
