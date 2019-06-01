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
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Stores
{
    /// <summary>
    /// Provides extension to implement custom <see cref="ICacheStore{T}"/>
    /// </summary>
    /// <typeparam name="T">Custom Cache Store Type</typeparam>
    public class CustomDistributedCacheStore<T> : ICacheStore<T>
    {
        /// <summary>
        /// Creates an instance of <see cref="CustomDistributedCacheStore{T}"/>
        /// </summary>
        /// <param name="cacheType">Cache Store type</param>
        public CustomDistributedCacheStore(Type cacheType)
        {
            CacheType = cacheType;
        }

        /// <summary>
        /// Gets or Sets Custom Distributed Cache Options
        /// </summary>
        public Action<T> CacheOptions { get; set; }

        /// <inheritdoc />
        public IDictionary<string, ICachePartition> CachePartitions { get; } = new Dictionary<string, ICachePartition>();

        /// <summary>
        /// Gets Cache Type
        /// </summary>
        public Type CacheType { get; }
    }
}
