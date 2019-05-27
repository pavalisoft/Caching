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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Cache
{
    public class ExtendedRedisCache : RedisCache, IExtendedDistributedCache
    {
        public ExtendedRedisCache(IOptions<RedisCacheOptions> optionsAccessor) : base(optionsAccessor)
        {
        }

        public void Set(string key, byte[] value, ExtendedDistributedCacheEntryOptions options)
        {
            Set(key, value, options as DistributedCacheEntryOptions);
        }

        public async Task SetAsync(string key, byte[] value, ExtendedDistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            await SetAsync(key, value, options as DistributedCacheEntryOptions, token);
        }
    }
}
