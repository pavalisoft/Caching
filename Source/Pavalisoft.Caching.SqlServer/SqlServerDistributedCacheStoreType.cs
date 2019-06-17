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

using Microsoft.Extensions.Caching.SqlServer;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.SqlServer
{
    /// <summary>
    /// Provides implementation to create <see cref="SqlServerDistributedCacheStore"/> from <see cref="SqlServerStoreInfo"/>
    /// </summary>
    public class SqlServerDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="SqlServerDistributedCacheStore"/> from <see cref="CacheStoreDefinition"/>
        /// </summary>
        /// <param name="cacheStoreInfo">SQL Server <see cref="CacheStoreDefinition"/> configuration</param>
        /// <returns><see cref="SqlServerDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreDefinition cacheStoreInfo)
        {
            return new SqlServerDistributedCacheStore
            {
                CacheOptions = !string.IsNullOrWhiteSpace(cacheStoreInfo.CacheOptions)
                ? JsonConvert.DeserializeObject<SqlServerCacheOptions>(cacheStoreInfo.CacheOptions)
                : new SqlServerCacheOptions()
            };
        }
    }
}