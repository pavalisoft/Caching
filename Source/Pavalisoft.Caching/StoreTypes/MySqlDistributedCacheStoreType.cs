using Newtonsoft.Json.Linq;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Stores;

namespace Pavalisoft.Caching.StoreTypes
{
    /// <summary>
    /// Provides implementation to create <see cref="MySqlDistributedCacheStore"/> from <see cref="MySqlStoreInfo"/>
    /// </summary>
    public class MySqlDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="MySqlDistributedCacheStore"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo">MySQL <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="MySqlDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                MySqlStoreInfo sqlServerStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<MySqlStoreInfo>();
                cacheStore = new MySqlDistributedCacheStore
                {
                    CacheOptions = options =>
                    {
                        options.ConnectionString = sqlServerStoreInfo.ConnectionString;
                        options.DefaultSlidingExpiration = sqlServerStoreInfo.DefaultSlidingExpiration;
                        options.ExpiredItemsDeletionInterval = sqlServerStoreInfo.ExpiredItemsDeletionInterval;
                        options.SchemaName = sqlServerStoreInfo.SchemaName;
                        options.TableName = sqlServerStoreInfo.TableName;
                    }
                };
            }
            else
            {
                cacheStore = new MySqlDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}