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

namespace Pavalisoft.Caching.Interfaces
{
    /// <summary>
    /// Represents <see cref="ICacheStore{T}"/> implementation
    /// </summary>
    /// <typeparam name="T">Cache Store type</typeparam>
    public interface ICacheStore<T>
    {
        /// <summary>
        /// Gets or Sets Cache Options
        /// </summary>
        Action<T> CacheOptions { get; set; }
        
        /// <summary>
        /// Gets Cache Type
        /// </summary>
        Type CacheType { get; }
    }
}
