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

using System.Collections.Generic;
using Microsoft.Extensions.Options;
using Pavalisoft.Caching.Cache;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Serializers;
using Pomelo.Extensions.Caching.MySql;

namespace Pavalisoft.Caching.MySql
{
    /// <summary>
    /// Provides <see cref="ExtendedMySqlCache"/> version of the <see cref="ICacheStore{T}"/>
    /// </summary>
    public class MySqlDistributedCacheStore : ICacheStore<MySqlCacheOptions>
    {
        /// <summary>
        /// Creates an instance of <see cref="MySqlDistributedCacheStore"/> with <paramref name="serializer"/> serializer.
        /// </summary>
        /// <param name="serializer">The <see cref="ISerializer"/>serializer. </param>
        public MySqlDistributedCacheStore(ISerializer serializer = default)
        {
            if (serializer != null)
                Serializer = serializer;
        }

        /// <summary>
        /// Gets or Sets <see cref="MySqlCacheOptions"/>
        /// </summary>
        public MySqlCacheOptions CacheOptions { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ICachePartition> CachePartitions { get; } = new Dictionary<string, ICachePartition>();

        /// <inheritdoc />
        public ISerializer Serializer { get; } = new DefaultSerializer();

        /// <summary>
        /// Creates <see cref="CachePartition"/> in <see cref="MySqlDistributedCacheStore"/> using <see cref="CachePartitionDefinition"/>
        /// </summary>
        /// <returns><see cref="CachePartition"/> object created in <see cref="MySqlDistributedCacheStore"/></returns>
        public ICachePartition CreatePartition(CachePartitionDefinition cachePartitionInfo)
        {
            ICachePartition cachePartition = new CachePartition(cachePartitionInfo.Name, cachePartitionInfo.AbsoluteExpiration,
                cachePartitionInfo.AbsoluteExpirationRelativeToNow, cachePartitionInfo.SlidingExpiration,
                new DistributedCache(new ExtendedMySqlCache(Options.Create(CacheOptions)),
                    this), cachePartitionInfo.Priority, cachePartitionInfo.Size);
            CachePartitions[cachePartitionInfo.Name] = cachePartition;
            return cachePartition;
        }
    }
}
