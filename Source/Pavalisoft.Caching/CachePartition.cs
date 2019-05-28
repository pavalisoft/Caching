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
using Microsoft.Extensions.Caching.Memory;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides Api to interact with <see cref="ICachePartition"/>
    /// </summary>
    public class CachePartition : ICachePartition
    {
        /// <summary>
        /// Creates an instance of <see cref="CachePartition"/>
        /// </summary>
        /// <param name="name">Cache Partition Name</param>
        /// <param name="absoluteExpiration">An absolute expiration date for the cache entry in this <see cref="ICachePartition"/>.</param>
        /// <param name="absoluteExpirationRelativeToNow">An absolute expiration time, relative to now in this <see cref="ICachePartition"/>.</param>
        /// <param name="slidingExpiration">Sliding expiration time for the cache entry in this <see cref="ICachePartition"/>.</param>
        /// <param name="store"><see cref="ICacheStore{T}"/> where the partition should be created</param>
        /// <param name="priority"><see cref="CacheItemPriority"/> to be applied to the cache items in this <see cref="ICachePartition"/></param>
        /// <param name="size"><see cref="ICachePartition"/> size</param>
        public CachePartition(string name, DateTimeOffset? absoluteExpiration,
            TimeSpan? absoluteExpirationRelativeToNow, TimeSpan? slidingExpiration, object store,
            CacheItemPriority priority, long? size)
        {
            Name = name;
            AbsoluteExpiration = absoluteExpiration;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            SlidingExpiration = slidingExpiration;
            Store = store;
            Priority = priority;
            Size = size;
        }

        /// <summary>
        /// Gets <see cref="ICachePartition"/> name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets an absolute expiration date for the cache entry in this <see cref="ICachePartition"/>.
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; }

        /// <summary>
        /// Gets an absolute expiration time, relative to now in this <see cref="ICachePartition"/>.
        /// </summary>
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; }

        /// <summary>
        /// Gets how long a cache entry can be inactive (e.g. not accessed) before it will be removed in this <see cref="ICachePartition"/>.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration { get; }

        /// <summary>
        /// Gets the Cache Store in this <see cref="ICachePartition"/>
        /// </summary>
        public object Store { get; }

        /// <summary>
        /// Gets the <see cref="CacheItemPriority"/> applicable for this <see cref="ICachePartition"/>
        /// </summary>
        public CacheItemPriority Priority { get; }

        /// <summary>
        /// Gets the Size of the <see cref="ICachePartition"/>
        /// </summary>
        public long? Size { get; }

        /// <summary>
        /// Gets or Sets the <see cref="ICache"/> instance.
        /// </summary>
        public ICache Cache { get; set; }
    }
}
