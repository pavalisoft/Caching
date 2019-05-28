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
    /// Represents <see cref="ICacheSettingsProvider"/>
    /// </summary>
    public interface ICacheSettingsProvider
    {
        /// <summary>
        /// Gets Cache Manager Settings
        /// </summary>
        /// <returns><see cref="CacheSettings"/></returns>
        CacheSettings GetCacheSettings();

        /// <summary>
        /// Gets Cache Stores in Cache Manager
        /// </summary>
        /// <returns>List of Cache Stores</returns>
        IEnumerable<object> GetCacheStores();

        /// <summary>
        /// Gets Cache store having <paramref name="storeName"/>
        /// </summary>
        /// <param name="storeName">Cache Store Name</param>
        /// <returns>Cache Store</returns>
        object GetCacheStore(string storeName);

        /// <summary>
        /// Gets <see cref="ICachePartition"/>s in the Cache Manager
        /// </summary>
        /// <returns></returns>
        IEnumerable<ICachePartition> GetCachePartitions();

        /// <summary>
        /// Gets <see cref="ICachePartition"/> having partition <paramref name="name"/>
        /// </summary>
        /// <param name="name">Cache Partition Name</param>
        /// <returns><see cref="ICachePartition"/></returns>
        ICachePartition GetCachePartition(string name);
    }
}
