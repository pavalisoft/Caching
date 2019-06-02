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
using Microsoft.Extensions.Caching.SqlServer;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.SqlServer
{
    /// <summary>
    /// Provides <see cref="ExtendedSqlServerCache"/> version of the <see cref="ICacheStore{T}"/>
    /// </summary>
    public class SqlServerDistributedCacheStore : ICacheStore<SqlServerCacheOptions>
    {
        /// <summary>
        /// Gets or Sets <see cref="SqlServerCacheOptions"/>
        /// </summary>
        public Action<SqlServerCacheOptions> CacheOptions { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ICachePartition> CachePartitions { get; } = new Dictionary<string, ICachePartition>();

        /// <summary>
        /// Gets Cache Type as <see cref="ExtendedSqlServerCache"/>
        /// </summary>
        public Type CacheType => typeof(ExtendedSqlServerCache);
    }
}
