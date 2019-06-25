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

using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;
using System;

namespace Pavalisoft.Caching.InMemory
{
    /// <summary>
    /// Provides implementation to create <see cref="InMemoryStore"/> from <see cref="CacheStoreDefinition"/>
    /// </summary>
    public class InMemoryCacheStoreType : ICacheStoreType
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates an instance of <see cref="InMemoryCacheStoreType"/> with <see cref="IServiceProvider"/>
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance. </param>
        public InMemoryCacheStoreType(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates <see cref="InMemoryStore"/> object
        /// </summary>
        /// <param name="cacheStoreInfo"><see cref="CacheStoreDefinition"/> object</param>
        /// <returns><see cref="InMemoryStore"/></returns>
        public ICacheStore CreateCacheStore(CacheStoreDefinition cacheStoreInfo)
        {
            ISerializer serializer = !string.IsNullOrWhiteSpace(cacheStoreInfo.SerializerType) 
                ? _serviceProvider.GetService(Type.GetType(cacheStoreInfo.SerializerType)) as ISerializer : null;

            return new InMemoryStore(serializer)
            {
                CacheOptions = !string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig)
                ? JsonConvert.DeserializeObject<MemoryCacheOptions>(cacheStoreInfo.StoreConfig)
                : new MemoryCacheOptions()
            };
        }
    }
}