﻿/* 
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

using Microsoft.Extensions.Caching.Redis;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Redis
{
    /// <summary>
    /// Provides implementation to create <see cref="RedisDistributedCacheStore"/> from <see cref="RedisStoreInfo"/>
    /// </summary>
    public class RedisDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="RedisDistributedCacheStore"/> from <see cref="CacheStoreDefinition"/> configuration
        /// </summary>
        /// <param name="cacheStoreInfo">Redis <see cref="CacheStoreDefinition"/> configuration</param>
        /// <returns><see cref="RedisDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreDefinition cacheStoreInfo)
        {
            return new RedisDistributedCacheStore
            {
                CacheOptions = !string.IsNullOrWhiteSpace(cacheStoreInfo.CacheOptions)
                ? JsonConvert.DeserializeObject<RedisCacheOptions>(cacheStoreInfo.CacheOptions)
                : new RedisCacheOptions()
            };
        }
    }
}