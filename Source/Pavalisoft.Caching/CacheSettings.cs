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
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides configuration structure for the Cache manager and its partitions including Cache stores.
    /// </summary>
    /// <example> The below is the sample configuration
    /// {
    ///     "Caching":
    ///     {
    ///         "Stores":
    ///         [
    ///             {
    ///                 "Name": "InMemory",
    ///                 "Type": "Memory",
    ///                 "StoreConfig": "{\"ExpirationScanFrequency\":\"00:05:00\"}"
    ///             },
    ///             {
    ///                 "Name": "SqlServer",
    ///                 "Type": "SqlServer",
    ///                 "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost\SQLEXPRESS;Initial Catalog=DistributedCache;Integrated Security=True\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
    ///             },
    ///             {
    ///                 "Name": "Redis",
    ///                 "Type": "Redis",
    ///                 "StoreConfig": "{\"Configuration\":\"00:05:00\", \"InstanceName\":\"localhost\"}"
    ///             },
    ///             {
    ///                 "Name": "MySql",
    ///                 "Type": "MySql",
    ///                 "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost\SQLEXPRESS;Initial Catalog=DistributedCache;Integrated Security=True\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
    ///             }    
    ///         ],
    ///         "Partitions":
    ///         [
    ///             {
    ///                 "Name": "FrequentData",
    ///                 "StoreName": "DistributedInMemory",
    ///                 "SlidingExpiration": "00:05:00"
    ///             },
    ///             {
    ///                 "Name": "LocalizationData",
    ///                 "StoreName": "DistributedSqlServer",
    ///                 "Priority": "NeverRemove"
    ///             },
    ///             {
    ///                 "Name": "MasterData",
    ///                 "StoreName": "DistributedRedis",
    ///                 "SlidingExpiration": "00:05:00"
    ///             }
    ///         ]
    ///    }
    /// }
    /// </example>
    public class CacheSettings
    {
        /// <summary>
        /// Gets or Sets Cache Stores Information
        /// </summary>
        public List<CacheStoreInfo> Stores { get; set; }

        /// <summary>
        /// Gets or Sets Cache Partition Configuration Information
        /// </summary>
        public List<CachePartitionInfo> Partitions { get; set; }
    }

    /// <summary>
    /// Represents Cache Store Information
    /// </summary>
    public class CacheStoreInfo
    {
        /// <summary>
        /// Gets or Sets Cache Store Type
        /// </summary>
        public StoreType Type { get; set; }

        /// <summary>
        /// Gets or Sets Cache Store Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Cache Store parameters configuration
        /// </summary>
        public string StoreConfig { get; set; }

        /// <summary>
        /// Gets or Sets Cache Store Type Information
        /// </summary>
        public string TypeInfo { get; set; }
    }

    /// <summary>
    /// Represents in-Memory Store Configuration Information
    /// </summary>
    public class MemoryStoreInfo
    {
        /// <summary>
        /// Gets or Sets expiration scan frequency of the cache items
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1.0);

        /// <summary>
        /// Gets or Sets the In-Memory Store size
        /// </summary>
        public long? SizeLimit { get; set; }

        /// <summary>
        /// Gets or Sets the cache items compaction percentage
        /// </summary>
        public double CompactionPercentage { get; set; }
    }

    /// <summary>
    /// Represents Redis Cache Store Configuration Information
    /// </summary>
    public class RedisStoreInfo
    {
        /// <summary>
        /// Gets or Sets the configuration used to connect to Redis.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or Sets the Redis instance name.
        /// </summary>
        public string InstanceName { get; set; }
    }

    /// <summary>
    /// Represents SQL Server Cache Store Configuration Information
    /// </summary>
    public class SqlServerStoreInfo
    {
        /// <summary>
        /// Gets ot Sets the periodic interval to scan and delete expired items in the cache. Default is 30 minutes.
        /// </summary>
        public TimeSpan? ExpiredItemsDeletionInterval { get; set; }

        /// <summary>
        /// Gets or Sets the connection string to the database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or Sets the schema name of the cache table.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or Sets the Name of the table where the cache items are stored.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// The default sliding expiration set for a cache entry if neither Absolute or SlidingExpiration has been set explicitly.
        /// By default, its 20 minutes.
        /// </summary>
        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromMinutes(20.0);
    }

    /// <summary>
    /// Represents MySQL Cache Store Configuration Information
    /// </summary>
    public class MySqlStoreInfo
    {
        /// <summary>
        /// Gets ot Sets the periodic interval to scan and delete expired items in the cache. Default is 30 minutes.
        /// </summary>
        public TimeSpan? ExpiredItemsDeletionInterval { get; set; }

        /// <summary>
        /// Gets or Sets the connection string to the database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or Sets the schema name of the cache table.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or Sets the Name of the table where the cache items are stored.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// The default sliding expiration set for a cache entry if neither Absolute or SlidingExpiration has been set explicitly.
        /// By default, its 20 minutes.
        /// </summary>
        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromMinutes(20.0);
    }

    /// <summary>
    /// Represents the Cache Partition Configuration Information
    /// </summary>
    public class CachePartitionInfo
    {
        /// <summary>
        /// Gets <see cref="ICachePartition"/> name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets an absolute expiration date for the cache entry in this <see cref="ICachePartition"/>.
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; set; }

        /// <summary>
        /// Gets an absolute expiration time, relative to now in this <see cref="ICachePartition"/>.
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

        /// <summary>
        /// Gets how long a cache entry can be inactive (e.g. not accessed) before it will be removed in this <see cref="ICachePartition"/>.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration { get; set; }

        /// <summary>
        /// Gets the Cache Store name in this <see cref="ICachePartition"/>
        /// </summary>
        public string StoreName { get; set; }

        /// <summary>
        /// Gets the <see cref="CacheItemPriority"/> applicable for this <see cref="ICachePartition"/>
        /// </summary>
        public CacheItemPriority Priority { get; set; }

        /// <summary>
        /// Gets the Size of the <see cref="ICachePartition"/>
        /// </summary>
        public long? Size { get; set; }
    }

    /// <summary>
    /// Represents the Cache Store Type
    /// </summary>
    public enum StoreType
    {
        /// <summary>
        /// Represents In-Memory Cache Store
        /// </summary>
        Memory,
        /// <summary>
        /// Represents Redis Cache Store
        /// </summary>
        Redis,
        /// <summary>
        /// Represents SQLServer Cache Store
        /// </summary>
        SqlServer,
        /// <summary>
        /// Represents MySql Cache Store
        /// </summary>
        MySql,
        /// <summary>
        /// Represents the Custom Cache Store
        /// </summary>
        Custom
    }
}
