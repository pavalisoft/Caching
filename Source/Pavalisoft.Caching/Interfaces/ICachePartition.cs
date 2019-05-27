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

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents <see cref="ICachePartition"/> implementation
    /// </summary>
    public interface ICachePartition
    {
        /// <summary>
        /// Gets <see cref="ICachePartition"/> name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets an absolute expiration date for the cache entry in this <see cref="ICachePartition"/>.
        /// </summary>
        DateTimeOffset? AbsoluteExpiration { get; }

        /// <summary>
        /// Gets an absolute expiration time, relative to now in this <see cref="ICachePartition"/>.
        /// </summary>
        TimeSpan? AbsoluteExpirationRelativeToNow { get; }

        /// <summary>
        /// Gets how long a cache entry can be inactive (e.g. not accessed) before it will be removed in this <see cref="ICachePartition"/>.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        TimeSpan? SlidingExpiration { get; }

        /// <summary>
        /// Gets the Cache Store in this <see cref="ICachePartition"/>
        /// </summary>
        object Store { get; }

        /// <summary>
        /// Gets the <see cref="CacheItemPriority"/> applicable for this <see cref="ICachePartition"/>
        /// </summary>
        CacheItemPriority Priority { get; }

        /// <summary>
        /// Gets the Size of the <see cref="ICachePartition"/>
        /// </summary>
        long? Size { get; }

        /// <summary>
        /// Gets or Sets the <see cref="ICache"/> instance.
        /// </summary>
        ICache Cache { get; set; }
    }
}
