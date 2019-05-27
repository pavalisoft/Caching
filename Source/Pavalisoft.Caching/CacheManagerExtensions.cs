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

namespace Pavalisoft.Caching
{
    public static class CacheManagerExtensions
    {
        public static object Get(this ICacheManager cacheManager, string partitionName, string key)
        {
            return cacheManager.Get<object>(partitionName, key);
        }

        public static bool TryGetValue<TItem>(this ICacheManager cacheManager, string partitionName, string key,
            out TItem value)
        {
            var obj = cacheManager.Get<TItem>(partitionName, key);
            var defaultObj = default(TItem);
            if (obj != null && !obj.Equals(defaultObj))
            {
                value = obj;
                return true;
            }
            value = defaultObj;
            return false;
        }
    }
}
