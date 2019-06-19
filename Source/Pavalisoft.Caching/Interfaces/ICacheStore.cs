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

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents <see cref="ICacheStore{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">Cache Store type</typeparam>
    public interface ICacheStore<T> : ICacheStore
    {
        /// <summary>
        /// Gets or Sets Cache Options
        /// </summary>
        T CacheOptions { get; set; }
    }

    /// <summary>
    /// Represents <see cref="ICacheStore"/>
    /// </summary>
    public interface ICacheStore
    {
        /// <summary>
        /// Gets all available <see cref="ICachePartition"/>s
        /// </summary>
        IDictionary<string, ICachePartition> CachePartitions { get; }

        /// <summary>
        /// Creates <see cref="ICachePartition"/> in <see cref="ICacheStore"/> using <see cref="CachePartitionDefinition"/>
        /// </summary>
        /// <returns><see cref="ICachePartition"/> object created in <see cref="ICachePartition"/></returns>
        ICachePartition CreatePartition(CachePartitionDefinition cachePartitionInfo);

        /// <summary>
        /// Gets <see cref="ISerializer"/> to be used for Cached Object serialization
        /// </summary>
        ISerializer Serializer { get; }
    }
}
