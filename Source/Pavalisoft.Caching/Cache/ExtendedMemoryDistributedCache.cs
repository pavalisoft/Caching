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
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Cache
{
    public class ExtendedMemoryDistributedCache : IExtendedDistributedCache
    {
        private static readonly Task CompletedTask = Task.FromResult<object>(null);

        private readonly IMemoryCache _memCache;

        public ExtendedMemoryDistributedCache(IOptions<MemoryDistributedCacheOptions> optionsAccessor)
        {
            if (optionsAccessor == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }

            _memCache = new MemoryCache(optionsAccessor.Value);
        }

        public byte[] Get(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return (byte[])_memCache.Get(key);
        }

        public Task<byte[]> GetAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            return Task.FromResult(Get(key));
        }

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
                    Size = value.Length
                };
            Set(key, value, cacheEntryOptions);
        }

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

        public void Refresh(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _memCache.TryGetValue(key, out object value);
        }

        public Task RefreshAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Refresh(key);
            return CompletedTask;
        }

        public void Remove(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            _memCache.Remove(key);
        }

        public Task RemoveAsync(string key, CancellationToken token = default)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            Remove(key);
            return CompletedTask;
        }

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

            var memoryCacheEntryOptions = GetMemoryCacheEntryOptions(value, options);

            _memCache.Set(key, value, memoryCacheEntryOptions);
        }

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

        private MemoryCacheEntryOptions GetMemoryCacheEntryOptions(byte[] value, ExtendedDistributedCacheEntryOptions options)
        {
            var memoryCacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = options.AbsoluteExpiration,
                AbsoluteExpirationRelativeToNow = options.AbsoluteExpirationRelativeToNow,
                SlidingExpiration = options.SlidingExpiration,
                Priority = options.Priority,
                Size = value.Length
            };
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
