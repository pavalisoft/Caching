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

using System.Collections.Generic;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Options;
using Pavalisoft.Caching.Cache;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Serializers;

namespace Pavalisoft.Caching.Redis
{
    /// <summary>
    /// Provides <see cref="ExtendedRedisCache"/> version of the <see cref="ICacheStore{T}"/>
    /// </summary>
    public class RedisDistributedCacheStore : ICacheStore<RedisCacheOptions>
    {
        /// <summary>
        /// Creates an instance of <see cref="RedisDistributedCacheStore"/> with <paramref name="serializer"/> serializer.
        /// </summary>
        /// <param name="serializer">The <see cref="ISerializer"/>serializer. </param>
        public RedisDistributedCacheStore(ISerializer serializer = default)
        {
            if (serializer != null)
                Serializer = serializer;
        }

        /// <summary>
        /// Gets or Sets <see cref="RedisCacheOptions"/>
        /// </summary>
        public RedisCacheOptions CacheOptions { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ICachePartition> CachePartitions { get; } = new Dictionary<string, ICachePartition>();

        /// <inheritdoc />
        public ISerializer Serializer { get; } = new DefaultSerializer();

        /// <summary>
        /// Creates <see cref="CachePartition"/> in <see cref="RedisDistributedCacheStore"/> using <see cref="CachePartitionDefinition"/>
        /// </summary>
        /// <returns><see cref="CachePartition"/> object created in <see cref="RedisDistributedCacheStore"/></returns>
        public ICachePartition CreatePartition(CachePartitionDefinition cachePartitionInfo)
        {
            ICachePartition cachePartition = new CachePartition(cachePartitionInfo.Name, cachePartitionInfo.AbsoluteExpiration,
                cachePartitionInfo.AbsoluteExpirationRelativeToNow, cachePartitionInfo.SlidingExpiration,
                new DistributedCache(new ExtendedRedisCache(Options.Create(CacheOptions)),
                    this), cachePartitionInfo.Priority, cachePartitionInfo.Size);
            CachePartitions[cachePartitionInfo.Name] = cachePartition;
            return cachePartition;
        }
    }
}
