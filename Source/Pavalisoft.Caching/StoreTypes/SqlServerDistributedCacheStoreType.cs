using Newtonsoft.Json.Linq;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.Stores;

namespace Pavalisoft.Caching.StoreTypes
{
    /// <summary>
    /// Provides implementation to create <see cref="SqlServerDistributedCacheStore"/> from <see cref="SqlServerStoreInfo"/>
    /// </summary>
    public class SqlServerDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// Creates <see cref="SqlServerDistributedCacheStore"/> from <see cref="CacheStoreInfo"/>
        /// </summary>
        /// <param name="cacheStoreInfo">SQL Server <see cref="CacheStoreInfo"/> configuration</param>
        /// <returns><see cref="SqlServerDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                SqlServerStoreInfo sqlServerStoreInfo =
                    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<SqlServerStoreInfo>();
                cacheStore = new SqlServerDistributedCacheStore
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
                cacheStore = new SqlServerDistributedCacheStore { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}