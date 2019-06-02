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

using System;
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
        private IReadOnlyDictionary<string, ICacheStore> _cacheStores;
        private IReadOnlyDictionary<string, ICachePartition> _cachePartitions;
        private readonly IServiceProvider _serviceProvider;
        private CacheSettings CacheSettings => _cacheSettings ?? (_cacheSettings = LoadCacheSettings());

        /// <summary>
        /// Creates an instance of <see cref="CacheSettingsProvider"/>
        /// </summary>
        /// <param name="serviceProvider">Dependency Service Provider</param>
        protected CacheSettingsProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
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
        public IEnumerable<ICacheStore> GetCacheStores()
        {
            LoadCachePartitions();
            return _cacheStores.Values;
        }

        /// <summary>
        /// Gets Cache Store having cache store name <paramref name="storeName"/>
        /// </summary>
        /// <param name="storeName">Cache Store Name</param>
        /// <returns>Cache Store</returns>
        public ICacheStore GetCacheStore(string storeName)
        {
            LoadCachePartitions();
            return _cacheStores.TryGetValue(storeName, out ICacheStore cacheValue) ? cacheValue : null;
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
            LoadCacheStores();
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
            ICacheStore cacheStore = GetCacheStore(partitionInfo.StoreName);
            ICachePartition cachePartition = new CachePartition(partitionInfo.Name, partitionInfo.AbsoluteExpiration,
                partitionInfo.AbsoluteExpirationRelativeToNow, partitionInfo.SlidingExpiration,
                new Cache.Cache(_serviceProvider.GetService(cacheStore.CacheType) as IExtendedDistributedCache,
                    cacheStore), partitionInfo.Priority, partitionInfo.Size);
            cacheStore.CachePartitions[partitionInfo.Name] = cachePartition;
            return cachePartition;
        }

        /// <summary>
        /// Loads Cache Stores from <see cref="CacheSettings"/> configuration
        /// </summary>
        private void LoadCacheStores()
        {
            if (_cacheStores == null || !_cacheStores.Any())
            {
                Dictionary<string, ICacheStore> cacheStores = new Dictionary<string, ICacheStore>();
                foreach (var storeInfo in CacheSettings.Stores)
                {
                    cacheStores.Add(storeInfo.Name, ConstructCacheStore(storeInfo));
                }
                _cacheStores = new ReadOnlyDictionary<string, ICacheStore>(cacheStores);
            }
        }

        /// <summary>
        /// Creates <see cref="ICacheStore{T}"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo"><see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="ICacheStore{T}"/> object</returns>
        private ICacheStore ConstructCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStoreType cacheStoreType =
                _serviceProvider.GetService(Type.GetType(cacheStoreInfo.TypeInfo)) as ICacheStoreType;
            return cacheStoreType?.CreateCacheStore(cacheStoreInfo);
        }
    }
}
