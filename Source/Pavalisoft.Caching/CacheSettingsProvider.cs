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
    public abstract class CacheSettingsProvider : ICacheSettingsProvider
    {
        private CacheSettings _cacheSettings;
        private IReadOnlyDictionary<string, object> _cacheStores;
        private IReadOnlyDictionary<string, ICachePartition> _cachePartitions;

        private CacheSettings CacheSettings => _cacheSettings ?? (_cacheSettings = LoadCacheSettings());

        public abstract CacheSettings LoadCacheSettings();

        public CacheSettings GetCacheSettings()
        {
            return CacheSettings;
        }
        public IEnumerable<object> GetCacheStores()
        {
            LoadCacheStores();
            return _cacheStores.Values;
        }

        public object GetCacheStore(string storeName)
        {
            LoadCacheStores();
            return _cacheStores.TryGetValue(storeName, out object cacheValue) ? cacheValue : null;
        }

        public IEnumerable<ICachePartition> GetCachePartitions()
        {
            LoadCachePartitions();
            return _cachePartitions.Values;
        }

        public ICachePartition GetCachePartition(string name)
        {
            LoadCachePartitions();
            return _cachePartitions.TryGetValue(name, out ICachePartition partitionValue) ? partitionValue : null;
        }

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

        private ICachePartition ConstructCachePartition(CachePartitionInfo partitionInfo)
        {
            return new CachePartition(partitionInfo.Name, partitionInfo.AbsoluteExpiration,
                partitionInfo.AbsoluteExpirationRelativeToNow, partitionInfo.SlidingExpiration,
                GetCacheStore(partitionInfo.StoreName), partitionInfo.Priority, partitionInfo.Size);
        }

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
                    cacheStore = GetSqlCacheStore(cacheStoreInfo);
                    break;
                    // TODO : Custom Cache Store related issues needs to be fixed.
                    //case StoreType.Custom:
                    //    return Activator.CreateInstance(Type.GetType(cacheStoreInfo.TypeInfo),
                    //        JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<object[]>());
            }
            return cacheStore;
        }

        private object GetSqlCacheStore(CacheStoreInfo cacheStoreInfo)
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
