using Newtonsoft.Json.Linq;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Stores;

namespace Pavalisoft.Caching.StoreTypes
{
    /// <summary>
    /// Provides implementation to create <see cref="MemoryDistributedCacheStore"/> from <see cref="MemoryStoreInfo"/>
    /// </summary>
    public class MemoryDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="MemoryDistributedCacheStore"/> object
        /// </summary>
        /// <param name="cacheStoreInfo"><see cref="CacheStoreInfo"/> object</param>
        /// <returns><see cref="MemoryDistributedCacheStore"/></returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                MemoryStoreInfo memoryStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<MemoryStoreInfo>();
                cacheStore = new MemoryDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.CompactionPercentage = memoryStoreInfo.CompactionPercentage;
                        options.ExpirationScanFrequency = memoryStoreInfo.ExpirationScanFrequency;
                        options.SizeLimit = memoryStoreInfo.SizeLimit;
                    }
                };
            }
            else
            {
                cacheStore = new MemoryDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}