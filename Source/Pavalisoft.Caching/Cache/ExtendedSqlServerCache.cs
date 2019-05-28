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
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Options;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Cache
{
    /// <summary>
    /// Provides <see cref="SqlServerCache"/> version implementation of <see cref="IExtendedDistributedCache"/>
    /// </summary>
    public class ExtendedSqlServerCache : SqlServerCache, IExtendedDistributedCache
    {
        /// <summary>
        /// Creates an instance of <see cref="ExtendedSqlServerCache"/> with <see cref="SqlServerCacheOptions"/>
        /// </summary>
        /// <param name="options"></param>
        public ExtendedSqlServerCache(IOptions<SqlServerCacheOptions> options) : base(options)
        {
        }

        /// <summary>
        /// Adds the Cache object binary stream to distributed cache
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Cache object in binary stream</param>
        /// <param name="options"><see cref="ExtendedDistributedCacheEntryOptions"/> where the cache object should be added to.</param>
        public void Set(string key, byte[] value, ExtendedDistributedCacheEntryOptions options)
        {
            Set(key, value, options as DistributedCacheEntryOptions);
        }

        /// <summary>
        /// Adds the Cache object binary stream to distributed cache asynchronously
        /// </summary>
        /// <param name="key">Cache key</param>
        /// <param name="value">Cache object in binary stream</param>
        /// <param name="options"><see cref="ExtendedDistributedCacheEntryOptions"/> where the cache object should be added to.</param>
        /// <param name="token"><see cref="CancellationToken"/> to be used while adding cache object to distributed cache.</param>
        public async Task SetAsync(string key, byte[] value, ExtendedDistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            await SetAsync(key, value, options as DistributedCacheEntryOptions, token);
        }
    }
}
