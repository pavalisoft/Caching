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

using Pavalisoft.Caching.Interfaces;
using System;

namespace Pavalisoft.Caching.InMemory.Sample
{
    public static class CacheManagerWeakReferenceExtensions
    {
        public static TItem GetWeak<TItem>(this ICacheManager cacheManager, string partitionName, string key) where TItem : class
        {
            if (cacheManager.TryGetValue(partitionName, key, out WeakReference<TItem> reference))
            {
                reference.TryGetTarget(out TItem value);
                return value;
            }
            return null;
        }

        public static TItem SetWeak<TItem>(this ICacheManager cacheManager,string partitionName, string key, TItem value) where TItem : class
        {
            var reference = new WeakReference<TItem>(value);
            cacheManager.Set(partitionName, key, reference, new WeakToken<TItem>(reference));
            return value;
        }
    }
}
