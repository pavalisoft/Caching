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
using Microsoft.Extensions.Caching.Redis;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Redis
{
    /// <summary>
    /// Provides <see cref="ExtendedRedisCache"/> version of the <see cref="ICacheStore{T}"/>
    /// </summary>
    public class RedisDistributedCacheStore : ICacheStore<RedisCacheOptions>
    {
        /// <summary>
        /// Gets or Sets <see cref="RedisCacheOptions"/>
        /// </summary>
        public Action<RedisCacheOptions> CacheOptions { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ICachePartition> CachePartitions { get; } = new Dictionary<string, ICachePartition>();

        /// <summary>
        /// Gets Cache Type <see cref="ExtendedRedisCache"/>
        /// </summary>
        public Type CacheType => typeof(ExtendedRedisCache);
    }
}
