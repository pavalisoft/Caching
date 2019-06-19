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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using Pavalisoft.Caching.Cache;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.InMemory
{
    /// <summary>
    /// Provides distributed cache implementation using <see cref="MemoryCache"/>
    /// </summary>
    public class ExtendedMemoryCache : MemoryCache, IExtendedMemoryCache, IExtendedDistributedCache
    {
        private static readonly Task CompletedTask = Task.FromResult<object>(null);

        /// <summary>
        /// Creates an instance of <see cref="ExtendedMemoryDistributedCache"/> with <see cref="MemoryDistributedCacheOptions"/>
        /// </summary>
        /// <param name="optionsAccessor"></param>
        public ExtendedMemoryCache(IOptions<MemoryCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
        }

        /// <summary>
        /// Gets Cache object in binary representation for the given <paramref name="key"/>
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <returns>Cached object in binary format</returns>
        public byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if(TryGetValue(key, out object result))
                return (byte[])result;
            return null;
        }

        /// <summary>
        /// Gets Cached object in binary format asynchronously
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="token"><see cref="CancellationToken"/> used</param>
        /// <returns>Cached object in binary format</returns>
        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Task.FromResult(Get(key));
        }

        /// <summary>
        /// Adds an object to distributed cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="DistributedCacheEntryOptions"/></param>
        public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var cacheEntryOptions =
                new ExtendedDistributedCacheEntryOptions
                {
                    AbsoluteExpiration = options.AbsoluteExpiration,
                    AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
                    SlidingExpiration = options.SlidingExpiration,
                    Size = new long?((long)value.LongLength)
                };
            Set(key, value, cacheEntryOptions);
        }

        /// <summary>
        /// Adds an object to distributed cache asynchronously
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="DistributedCacheEntryOptions"/></param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        public Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Set(key, value, options);
            return CompletedTask;
        }

        /// <summary>
        /// Refreshes the cache item of the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        public void Refresh(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            TryGetValue(key, out object value);
        }

        /// <summary>
        /// Refreshes the cache item asynchronously for the specified cache key
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while refreshing cache item</param>
        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Refresh(key);
            return CompletedTask;
        }

        /// <summary>
        /// Removes the Cache object from the cache for the specified cache key
        /// </summary>
        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            base.Remove(key);
        }

        /// <summary>
        /// Removes the Cache object asynchronously from the cache for the specified cache key
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while removing cache item</param>
        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Remove(key);
            return CompletedTask;
        }

        /// <summary>
        /// Adds an object to distributed cache
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="ExtendedDistributedCacheEntryOptions"/></param>
        public void Set(string key, byte[] value, ExtendedDistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            
            using (ICacheEntry cacheEntry = CreateEntry(key))
            {
                var memoryCacheEntryOptions = GetMemoryCacheEntryOptions(options, value.LongLength);
                cacheEntry.SetOptions(memoryCacheEntryOptions);
                if (memoryCacheEntryOptions.AbsoluteExpiration != null)
                    cacheEntry.SetAbsoluteExpiration(memoryCacheEntryOptions.AbsoluteExpiration.Value);
                if (memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow != null)
                    cacheEntry.SetAbsoluteExpiration(memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow.Value);
                if (memoryCacheEntryOptions.SlidingExpiration != null)
                    cacheEntry.SetSlidingExpiration(memoryCacheEntryOptions.SlidingExpiration.Value);
                cacheEntry.SetPriority(memoryCacheEntryOptions.Priority);
                cacheEntry.SetSize(value.LongLength);
                cacheEntry.SetValue(value);
            }
        }

        /// <summary>
        /// Adds an object to distributed cache asynchronously
        /// </summary>
        /// <param name="key">Cache Key</param>
        /// <param name="value">Cache object</param>
        /// <param name="options">Distributed cache options. <see cref="ExtendedDistributedCacheEntryOptions"/></param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while setting cache item</param>
        public Task SetAsync(string key, byte[] value, ExtendedDistributedCacheEntryOptions options, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Set(key, value, options);
            return CompletedTask;
        }        

        object IExtendedMemoryCache.Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (TryGetValue(key, out object result))
                return result;
            return null;
        }

        Task<object> IExtendedMemoryCache.GetAsync(string key, CancellationToken token)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            if (TryGetValue(key, out object result))
                return Task.FromResult(result);
            return Task.FromResult((object)null);
        }

        public void Set(string key, object value, ExtendedDistributedCacheEntryOptions options)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            using (ICacheEntry cacheEntry = CreateEntry(key))
            {
                var memoryCacheEntryOptions = GetMemoryCacheEntryOptions(options);
                cacheEntry.SetOptions(memoryCacheEntryOptions);
                if (memoryCacheEntryOptions.AbsoluteExpiration != null)
                    cacheEntry.SetAbsoluteExpiration(memoryCacheEntryOptions.AbsoluteExpiration.Value);
                if (memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow != null)
                    cacheEntry.SetAbsoluteExpiration(memoryCacheEntryOptions.AbsoluteExpirationRelativeToNow.Value);
                if (memoryCacheEntryOptions.SlidingExpiration != null)
                    cacheEntry.SetSlidingExpiration(memoryCacheEntryOptions.SlidingExpiration.Value);
                cacheEntry.SetPriority(memoryCacheEntryOptions.Priority);
                cacheEntry.SetValue(value);
            }
        }

        public Task SetAsync(string key, object value, ExtendedDistributedCacheEntryOptions options, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Set(key, value, options);
            return CompletedTask;
        }

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions(ExtendedDistributedCacheEntryOptions options, long? size = default)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = options.SlidingExpiration,
                Priority = options.Priority
            };
            if(size != null)
                memoryCacheEntryOptions.SetSize(size.Value);

            if (options.ExpirationTokens != null && options.ExpirationTokens.Any())
            {
                foreach (IChangeToken expirationToken in options.ExpirationTokens)
                {
                    memoryCacheEntryOptions.ExpirationTokens.Add(expirationToken);
                }
            }
            if (options.PostEvictionCallbacks != null && options.PostEvictionCallbacks.Any())
            {
                foreach (var callbackRegistration in options.PostEvictionCallbacks)
                {
                    memoryCacheEntryOptions.PostEvictionCallbacks.Add(callbackRegistration);
                }
            }
            return memoryCacheEntryOptions;
        }
    }
}
