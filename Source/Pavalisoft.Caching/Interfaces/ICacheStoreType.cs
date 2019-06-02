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

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents <see cref="ICacheStore"/> creation implementation from <see cref="CacheStoreInfo"/> 
    /// </summary>
    public interface ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="ICacheStore"/> instance from the provided <paramref name="cacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo"><see cref="CacheStoreInfo"/> to be used to create <see cref="ICacheStore"/></param>
        /// <returns><see cref="ICacheStore"/> object</returns>
        ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo);
    }
}