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
using Microsoft.Extensions.Caching.Memory;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// {
    ///     "Caching" : {
    ///         "Stores" : [
    ///             { "Name" : "DistributedInMemory", "Type" : "Memory", "StoreConfig" : "{\"ExpirationScanFrequency\":\"00:05:00\"}"},
    ///             { "Name" : "DistributedRedis", "Type" : "Redis", "StoreConfig" : "{\"Configuration\":\"00:05:00\", \"InstanceName\":\"localhost\"}"}
    ///         ],
    ///         "Partitions" : [
    ///             { "Name" : "MasterData", "StoreName" : "DistributedInMemory", "SlidingExpiration" : "00:00:30" },
    ///             { "Name" : "LocalizationData", "StoreName" : "DistributedInMemory", "SlidingExpiration" : "00:00:30" },
    ///             { "Name" : "IncrementalData", "StoreName" : "DistributedInMemory", "SlidingExpiration" : "00:00:30" }
    ///         ]
    ///     }
    /// }
    /// </summary>
    public class CacheSettings
    {
        public List<CacheStoreInfo> Stores { get; set; }
        public List<CachePartitionInfo> Partitions { get; set; }
    }

    public class CacheStoreInfo
    {
        public StoreType Type { get; set; }
        public string Name { get; set; }
        public string StoreConfig { get; set; }
        public string TypeInfo { get; set; }
    }

    public class MemoryStoreInfo
    {
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1.0);
        public long? SizeLimit { get; set; }
        public double CompactionPercentage { get; set; }

    }

    public class RedisStoreInfo
    {
        /// <summary>The configuration used to connect to Redis.</summary>
        public string Configuration { get; set; }

        /// <summary>The Redis instance name.</summary>
        public string InstanceName { get; set; }
    }

    public class SqlServerStoreInfo
    {
        /// <summary>
        /// The periodic interval to scan and delete expired items in the cache. Default is 30 minutes.
        /// </summary>
        public TimeSpan? ExpiredItemsDeletionInterval { get; set; }

        /// <summary>The connection string to the database.</summary>
        public string ConnectionString { get; set; }

        /// <summary>The schema name of the table.</summary>
        public string SchemaName { get; set; }

        /// <summary>Name of the table where the cache items are stored.</summary>
        public string TableName { get; set; }

        /// <summary>
        /// The default sliding expiration set for a cache entry if neither Absolute or SlidingExpiration has been set explicitly.
        /// By default, its 20 minutes.
        /// </summary>
        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromMinutes(20.0);
    }


    public class CachePartitionInfo
    {
        public string Name { get; set; }
        public DateTimeOffset? AbsoluteExpiration { get; set; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }
        public TimeSpan? SlidingExpiration { get; set; }
        public string StoreName { get; set; }
        public CacheItemPriority Priority { get; set; }
        public long? Size { get; set; }
    }

    public enum StoreType
    {
        Memory,
        Redis,
        SqlServer,
        Custom
    }
}
