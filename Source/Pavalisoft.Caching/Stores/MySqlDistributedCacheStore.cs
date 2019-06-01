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
using System.Collections.Generic;
using Pavalisoft.Caching.Cache;
using Pavalisoft.Caching.Interfaces;
using Pomelo.Extensions.Caching.MySql;

namespace Pavalisoft.Caching.Stores
{
    /// <summary>
    /// Provides <see cref="ExtendedMySqlCache"/> version of the <see cref="ICacheStore{T}"/>
    /// </summary>
    public class MySqlDistributedCacheStore : ICacheStore<MySqlCacheOptions>
    {
        /// <summary>
        /// Gets or Sets <see cref="MySqlCacheOptions"/>
        /// </summary>
        public Action<MySqlCacheOptions> CacheOptions { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ICachePartition> CachePartitions { get; } = new Dictionary<string, ICachePartition>();

        /// <summary>
        /// Gets Cache Type as <see cref="ExtendedSqlServerCache"/>
        /// </summary>
        public Type CacheType => typeof(ExtendedMySqlCache);
    }
}
