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
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.InMemory.Sample
{
    public class Program
    {
        private const string InMemoryStorePartition = "FrequentData";
        private const string DistributedMemoryStorePartition = "DistibutedFrequentData";

        public static void Main()
        {
            ICacheManager cacheManager = CreateCacheManager();

            object result;
            string key = "Key";
            object newObject = new object();
            object state = new object();

            // Basic CRUD operations:

            // Create / Overwrite
            AddToCache(cacheManager, InMemoryStorePartition, key, newObject);
            AddToCache(cacheManager, DistributedMemoryStorePartition, key, newObject);

            // Retrieve, null if not found
            result = cacheManager.Get(InMemoryStorePartition, key);
            result = cacheManager.Get(DistributedMemoryStorePartition, key);

            // Retrieve
            bool found = cacheManager.TryGetValue(InMemoryStorePartition, key, out result);
            found = cacheManager.TryGetValue(DistributedMemoryStorePartition, key, out result);

            // Store and Get using weak references
            result = cacheManager.SetWeak(InMemoryStorePartition, key, newObject);
            result = cacheManager.GetWeak<object>(InMemoryStorePartition, key);

            result = cacheManager.SetWeak(DistributedMemoryStorePartition, key, newObject);
            result = cacheManager.GetWeak<object>(DistributedMemoryStorePartition, key);

            // Delete
            cacheManager.Remove(InMemoryStorePartition, key);
            cacheManager.Remove(DistributedMemoryStorePartition, key);


            // Callback when evicted
            PostEvictionCallbackRegistration postEvictionCallbackRegistration = new PostEvictionCallbackRegistration
            {
                EvictionCallback = (echoKey, value, reason, substate) =>
                {
                    Console.WriteLine($"{echoKey} : {value} was evicted due to {reason}");
                }
            };
            cacheManager.Set(InMemoryStorePartition, key, new object(), null, postEvictionCallbackRegistration);
            cacheManager.Set(DistributedMemoryStorePartition, key, new object(), null, postEvictionCallbackRegistration);

            AddPostEvictionPolicy(cacheManager, InMemoryStorePartition, key, postEvictionCallbackRegistration);
            AddPostEvictionPolicy(cacheManager, DistributedMemoryStorePartition, key, postEvictionCallbackRegistration);
        }

        private static void AddPostEvictionPolicy(ICacheManager cacheManager, string partitionName, string key, PostEvictionCallbackRegistration postEvictionCallbackRegistration)
        {
            // Remove on token expiration
            var cts = new CancellationTokenSource();
            cacheManager.Set(InMemoryStorePartition, key, new object(), new CancellationChangeToken(cts.Token), postEvictionCallbackRegistration);

            // Fire the token to see the registered callback being invoked
            cts.Cancel();

            // Expire an entry if the dependent entry expires            
            cacheManager.Set(InMemoryStorePartition, "key1", "value1");

            // expire this entry if the entry with key "key2" expires.
            cts = new CancellationTokenSource();
            cacheManager.Set(InMemoryStorePartition, "key2", "value2", new CancellationChangeToken(cts.Token), postEvictionCallbackRegistration);

            // Fire the token to see the registered callback being invoked
            cts.Cancel();
        }

        private static void AddToCache(ICacheManager cacheManager, string partitionName, string key, object newObject)
        {
            cacheManager.Set(partitionName, key, newObject);
            cacheManager.Set(partitionName, key, new object());
        }

        private static ICacheManager CreateCacheManager()
        {
            IServiceCollection services = new ServiceCollection();
            
            services.AddOptions();

            services.AddCaching(CreateCacheSettings())
                .AddInMemoryCache();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ICacheManager cacheManager = serviceProvider.GetService<ICacheManager>();
            return cacheManager;
        }

        private static CacheSettings CreateCacheSettings()
        {
            return new CacheSettings
            {
                Stores = new List<CacheStoreDefinition>
                {
                    new CacheStoreDefinition
                    {
                        Name = "InMemory",
                        Type = typeof(InMemoryCacheStoreType).AssemblyQualifiedName,
                        StoreConfig = JsonConvert.SerializeObject( new MemoryCacheOptions
                        {
                            ExpirationScanFrequency = new TimeSpan(0,5,0)
                        })
                    },
                    new CacheStoreDefinition
                    {
                        Name = "DistributedInMemory",
                        Type = typeof(MemoryDistributedCacheStoreType).AssemblyQualifiedName,
                        SerializerType = typeof(Serializers.JsonSerializer).AssemblyQualifiedName,
                        StoreConfig = JsonConvert.SerializeObject( new MemoryDistributedCacheOptions
                        {
                            ExpirationScanFrequency = new TimeSpan(0,5,0)
                        })
                    }
                },
                Partitions = new List<CachePartitionDefinition>
                {
                    new CachePartitionDefinition
                    {
                        Name = "FrequentData",
                        StoreName = "InMemory",
                        SlidingExpiration = new TimeSpan(0,5,0)
                    },
                    new CachePartitionDefinition
                    {
                        Name = "DistibutedFrequentData",
                        StoreName = "DistributedInMemory",
                        SlidingExpiration = new TimeSpan(0,5,0)
                    }
                }
            };
        }
    }
}
