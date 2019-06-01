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
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json.Linq;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Stores;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides base implementation for <see cref="ICacheSettingsProvider"/>
    /// </summary>
    public abstract class CacheSettingsProvider : ICacheSettingsProvider
    {
        private CacheSettings _cacheSettings;
        private IReadOnlyDictionary<string, object> _cacheStores;
        private IReadOnlyDictionary<string, ICachePartition> _cachePartitions;

        private CacheSettings CacheSettings => _cacheSettings ?? (_cacheSettings = LoadCacheSettings());

        /// <summary>
        /// Loads Cache Settings Configuration
        /// </summary>
        /// <returns><see cref="CacheSettings"/> object</returns>
        public abstract CacheSettings LoadCacheSettings();

        /// <summary>
        /// Gets <see cref="CacheSettings"/> from configuration
        /// </summary>
        /// <returns><see cref="CacheSettings"/> object</returns>
        public CacheSettings GetCacheSettings()
        {
            return CacheSettings;
        }

        /// <summary>
        /// Gets <see cref="ICacheStore{T}"/> from <see cref="CacheSettings"/>
        /// </summary>
        /// <returns>Cache Stores</returns>
        public IEnumerable<object> GetCacheStores()
        {
            LoadCacheStores();
            return _cacheStores.Values;
        }

        /// <summary>
        /// Gets Cache Store having cache store name <paramref name="storeName"/>
        /// </summary>
        /// <param name="storeName">Cache Store Name</param>
        /// <returns>Cache Store</returns>
        public object GetCacheStore(string storeName)
        {
            LoadCacheStores();
            return _cacheStores.TryGetValue(storeName, out object cacheValue) ? cacheValue : null;
        }

        /// <summary>
        /// Gets Cache Partitions in the <see cref="CacheSettings"/>
        /// </summary>
        /// <returns>List of <see cref="ICachePartition"/>s</returns>
        public IEnumerable<ICachePartition> GetCachePartitions()
        {
            LoadCachePartitions();
            return _cachePartitions.Values;
        }

        /// <summary>
        /// Gets Cache Partition having cache partition name <paramref name="name"/>
        /// </summary>
        /// <param name="name">Cache Partition Name</param>
        /// <returns><see cref="ICachePartition"/></returns>
        public ICachePartition GetCachePartition(string name)
        {
            LoadCachePartitions();
            return _cachePartitions.TryGetValue(name, out ICachePartition partitionValue) ? partitionValue : null;
        }

        /// <summary>
        /// Loads Cache Partitions from <see cref="CacheSettings"/> configuration
        /// </summary>
        private void LoadCachePartitions()
        {
            if (_cachePartitions == null || !_cachePartitions.Any())
            {
                Dictionary<string, ICachePartition> cachePartitions = new Dictionary<string, ICachePartition>();
                foreach (var partitionInfo in CacheSettings.Partitions)
                {
                    cachePartitions.Add(partitionInfo.Name, ConstructCachePartition(partitionInfo));
                }
                _cachePartitions = new ReadOnlyDictionary<string, ICachePartition>(cachePartitions);
            }
        }

        /// <summary>
        /// Creates <see cref="ICachePartition"/> instance from <see cref="CachePartitionInfo"/> configuration
        /// </summary>
        /// <param name="partitionInfo"><see cref="CachePartitionInfo"/> configuration</param>
        /// <returns><see cref="ICachePartition"/> object</returns>
        private ICachePartition ConstructCachePartition(CachePartitionInfo partitionInfo)
        {
            return new CachePartition(partitionInfo.Name, partitionInfo.AbsoluteExpiration,
                partitionInfo.AbsoluteExpirationRelativeToNow, partitionInfo.SlidingExpiration,
                GetCacheStore(partitionInfo.StoreName), partitionInfo.Priority, partitionInfo.Size);
        }

        /// <summary>
        /// Loads Cache Stores from <see cref="CacheSettings"/> configuration
        /// </summary>
        private void LoadCacheStores()
        {
            if (_cacheStores == null || !_cacheStores.Any())
            {
                Dictionary<string, object> cacheStores = new Dictionary<string, object>();
                foreach (var storeInfo in CacheSettings.Stores)
                {
                    cacheStores.Add(storeInfo.Name, ConstructCacheStore(storeInfo));
                }
                _cacheStores = new ReadOnlyDictionary<string, object>(cacheStores);
            }
        }

        /// <summary>
        /// Creates <see cref="ICacheStore{T}"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo"><see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="ICacheStore{T}"/> object</returns>
        private object ConstructCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            object cacheStore = null;
            switch (cacheStoreInfo.Type)
            {
                case StoreType.Memory:
                    cacheStore = GetMemoryCacheStore(cacheStoreInfo);
                    break;
                case StoreType.Redis:
                    cacheStore = GetRedisCacheStore(cacheStoreInfo);
                    break;
                case StoreType.SqlServer:
                    cacheStore = GetSqlServerCacheStore(cacheStoreInfo);
                    break;
                case StoreType.MySql:
                    cacheStore = GetMySqlCacheStore(cacheStoreInfo);
                    break;
                    // TODO : Custom Cache Store related issues needs to be fixed.
                    //case StoreType.Custom:
                    //    return Activator.CreateInstance(Type.GetType(cacheStoreInfo.TypeInfo),
                    //        JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<object[]>());
            }
            return cacheStore;
        }

        /// <summary>
        /// Creates <see cref="SqlServerDistributedCacheStore"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo">SQL Server <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="SqlServerDistributedCacheStore"/> object</returns>
        private object GetSqlServerCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            object cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                SqlServerStoreInfo sqlServerStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<SqlServerStoreInfo>();
                cacheStore = new SqlServerDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.ConnectionString = sqlServerStoreInfo.ConnectionString;
                        options.DefaultSlidingExpiration = sqlServerStoreInfo.DefaultSlidingExpiration;
                        options.ExpiredItemsDeletionInterval = sqlServerStoreInfo.ExpiredItemsDeletionInterval;
                        options.SchemaName = sqlServerStoreInfo.SchemaName;
                        options.TableName = sqlServerStoreInfo.TableName;
                    }
                };
            }
            else
            {
                cacheStore = new SqlServerDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }

        /// <summary>
        /// Creates <see cref="MySqlDistributedCacheStore"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo">MySQL <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="MySqlDistributedCacheStore"/> object</returns>
        private object GetMySqlCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            object cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                MySqlStoreInfo sqlServerStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<MySqlStoreInfo>();
                cacheStore = new MySqlDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.ConnectionString = sqlServerStoreInfo.ConnectionString;
                        options.DefaultSlidingExpiration = sqlServerStoreInfo.DefaultSlidingExpiration;
                        options.ExpiredItemsDeletionInterval = sqlServerStoreInfo.ExpiredItemsDeletionInterval;
                        options.SchemaName = sqlServerStoreInfo.SchemaName;
                        options.TableName = sqlServerStoreInfo.TableName;
                    }
                };
            }
            else
            {
                cacheStore = new MySqlDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }

        /// <summary>
        /// Creates <see cref="RedisDistributedCacheStore"/> from <see cref="CacheStoreInfo"/> configuration
        /// </summary>
        /// <param name="cacheStoreInfo">Redis <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="RedisDistributedCacheStore"/> object</returns>
        private object GetRedisCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            object cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                RedisStoreInfo redisStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<RedisStoreInfo>();
                cacheStore = new RedisDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.Configuration = redisStoreInfo.Configuration;
                        options.InstanceName = redisStoreInfo.InstanceName;
                    }
                };
            }
            else
            {
                cacheStore = new RedisDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }

        /// <summary>
        /// Creates <see cref="MemoryDistributedCacheStore"/> from <see cref="CacheStoreInfo"/> configuration
        /// </summary>
        /// <param name="cacheStoreInfo">In-Memory <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns></returns>
        private object GetMemoryCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            object cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                MemoryStoreInfo memoryStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<MemoryStoreInfo>();
                cacheStore = new MemoryDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.CompactionPercentage = memoryStoreInfo.CompactionPercentage;
                        options.ExpirationScanFrequency = memoryStoreInfo.ExpirationScanFrequency;
                        options.SizeLimit = memoryStoreInfo.SizeLimit;
                    }
                };
            }
            else
            {
                cacheStore = new MemoryDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}
