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
    ///                 "StoreConfig": "{\"ExpiredItemsDeletionInterval\":\"00:05:00\", \"ConnectionString\":\"Data Source=localhost;Initial Catalog=DistributedCache;Integrated Security=True\", \"SchemaName\":\"store\", \"TableName\":\"Cache\", \"DefaultSlidingExpiration\":\"00:05:00\"}"
    ///             }    
    ///         ],
    ///         "Partitions":
    ///         [
    ///             {
    ///                 "Name": "FrequentData",
    ///                 "StoreName": "InMemory",
    ///                 "SlidingExpiration": "00:05:00"
    ///             },
    ///             {
    ///                 "Name": "LocalizationData",
    ///                 "StoreName": "SqlServer",
    ///                 "Priority": "NeverRemove"
    ///             },
    ///             {
    ///                 "Name": "MasterData",
    ///                 "StoreName": "Redis",
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
