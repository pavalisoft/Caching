using Newtonsoft.Json.Linq;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Stores;

namespace Pavalisoft.Caching.StoreTypes
{
    /// <summary>
    /// Provides implementation to create <see cref="RedisDistributedCacheStore"/> from <see cref="RedisStoreInfo"/>
    /// </summary>
    public class RedisDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="RedisDistributedCacheStore"/> from <see cref="CacheStoreInfo"/> configuration
        /// </summary>
        /// <param name="cacheStoreInfo">Redis <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="RedisDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                RedisStoreInfo redisStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<RedisStoreInfo>();
                cacheStore = new RedisDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.Configuration = redisStoreInfo.Configuration;
                        options.InstanceName = redisStoreInfo.InstanceName;
                    }
                };
            }
            else
            {
                cacheStore = new RedisDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}