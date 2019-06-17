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

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Redis
{
    /// <summary>
    /// Provides implementation to create <see cref="RedisDistributedCacheStore"/> from <see cref="RedisStoreInfo"/>
    /// </summary>
    public class RedisDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="RedisDistributedCacheStore"/> from <see cref="CacheStoreInfo"/> configuration
        /// </summary>
        /// <param name="cacheStoreInfo">Redis <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="RedisDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                RedisStoreInfo redisStoreInfo =
                    JsonConvert.DeserializeObject<RedisStoreInfo>(cacheStoreInfo.StoreConfig);
                cacheStore = new RedisDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.Configuration = redisStoreInfo.Configuration;
                        options.InstanceName = redisStoreInfo.InstanceName;
                    }
                };
            }
            else
            {
                cacheStore = new RedisDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}