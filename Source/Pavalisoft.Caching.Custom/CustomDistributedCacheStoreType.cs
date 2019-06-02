using System;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Custom
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDistributedCacheStoreType : ICacheStoreType
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cacheStoreInfo"></param>
        /// <returns></returns>
        public ICacheStore CreateCacheStore(CacheStoreInfo cacheStoreInfo)
        {
            ICacheStore cacheStore = null;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
            {
                //CustomStoreInfo sqlServerStoreInfo =
                //    JObject.Parse(cacheStoreInfo.StoreConfig).ToObject<CustomStoreInfo>();
                //cacheStore = new CustomDistributedCacheStore<T>(Type.GetType(cacheStoreInfo.TypeInfo))
                //{
                //    CacheOptions = options =>
                //    {
                //        options.ConnectionString = sqlServerStoreInfo.ConnectionString;
                //        options.DefaultSlidingExpiration = sqlServerStoreInfo.DefaultSlidingExpiration;
                //        options.ExpiredItemsDeletionInterval = sqlServerStoreInfo.ExpiredItemsDeletionInterval;
                //        options.SchemaName = sqlServerStoreInfo.SchemaName;
                //        options.TableName = sqlServerStoreInfo.TableName;
                //    }
                //};
            }
            else
            {
                cacheStore = new CustomDistributedCacheStore<Type>(typeof(Type)) { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}