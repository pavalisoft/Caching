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

using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Cache
{
    public class Cache : ICache
    {
        private readonly IExtendedDistributedCache _distributedCache;
        private object _cacheStore;

        public Cache(IExtendedDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public TItem Get<TItem>(string key)
        {
            byte[] cache = _distributedCache.Get(key);
            if (cache == null) return default;
            string str = Encoding.UTF8.GetString(cache);
            return JsonConvert.DeserializeObject<TItem>(str);
            //return JObject.Parse(str).ToObject<TItem>();
        }

        public async Task<TItem> GetAsync<TItem>(string key, CancellationToken token = default)
        {
            byte[] cache = await _distributedCache.GetAsync(key, token);
            if (cache == null) return default;
            string str = Encoding.UTF8.GetString(cache);
            return JsonConvert.DeserializeObject<TItem>(str);
            //return JObject.Parse(str).ToObject<TItem>();
        }

        public void Set<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options)
        {
            //string str = JObject.FromObject(value).ToString();
            string str = JsonConvert.SerializeObject(value);
            byte[] val = Encoding.UTF8.GetBytes(str);
            _distributedCache.Set(key, val, options);
        }

        public async Task SetAsync<TItem>(string key, TItem value, ExtendedDistributedCacheEntryOptions options,
            CancellationToken token = default)
        {
            //string str = JObject.FromObject(value).ToString();
            string str = JsonConvert.SerializeObject(value);
            byte[] val = Encoding.UTF8.GetBytes(str);
            await _distributedCache.SetAsync(key, val, options, token);
        }

        public void Refresh(string key)
        {
            _distributedCache.Refresh(key);
        }

        public async Task RefreshAsync(string key, CancellationToken token = default)
        {
            await _distributedCache.RefreshAsync(key, token);
        }

        public void Remove(string key)
        {
            _distributedCache.Remove(key);
        }

        public async Task RemoveAsync(string key, CancellationToken token = default)
        {
            await _distributedCache.RemoveAsync(key, token);
        }

        public ICacheStore<T> GetCacheStore<T>()
        {
            return _cacheStore as ICacheStore<T>;
        }

        public void SetCacheStore<T>(ICacheStore<T> cacheStore)
        {
            _cacheStore = cacheStore;
        }
    }
}
