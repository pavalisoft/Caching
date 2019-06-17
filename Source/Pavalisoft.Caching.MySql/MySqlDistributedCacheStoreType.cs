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

namespace Pavalisoft.Caching.MySql
{
    /// <summary>
    /// Provides implementation to create <see cref="MySqlDistributedCacheStore"/> from <see cref="MySqlStoreInfo"/>
    /// </summary>
    public class MySqlDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="MySqlDistributedCacheStore"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo">MySQL <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="MySqlDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                MySqlStoreInfo sqlServerStoreInfo =
                    JsonConvert.DeserializeObject<MySqlStoreInfo>(cacheStoreInfo.StoreConfig);
                cacheStore = new MySqlDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.ConnectionString = sqlServerStoreInfo.ConnectionString;
                        options.ReadConnectionString = options.ConnectionString;
                        options.WriteConnectionString = options.WriteConnectionString;
                        options.DefaultSlidingExpiration = sqlServerStoreInfo.DefaultSlidingExpiration;
                        options.ExpiredItemsDeletionInterval = sqlServerStoreInfo.ExpiredItemsDeletionInterval;
                        options.SchemaName = sqlServerStoreInfo.SchemaName;
                        options.TableName = sqlServerStoreInfo.TableName;
                    }
                };
            }
            else
            {
                cacheStore = new MySqlDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}