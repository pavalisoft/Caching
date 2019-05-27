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

using System.Threading;
using System.Threading.Tasks;
using Pavalisoft.Caching.Cache;

namespace Pavalisoft.Caching.Interfaces
{
    public interface ICache
    {
        TItem Get<TItem>(string key);

        Task<TItem> GetAsync<TItem>(string key, CancellationToken token = default);

        void Set<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options);

        Task SetAsync<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options, CancellationToken token = default);

        void Refresh(string key);

        Task RefreshAsync(string key, CancellationToken token = default);

        void Remove(string key);

        Task RemoveAsync(string key, CancellationToken token = default);

        ICacheStore<T> GetCacheStore<T>();

        void SetCacheStore<T>(ICacheStore<T> cacheStore);
    }
}
