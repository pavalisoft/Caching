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
        public ICacheStore CreateCacheStore(CacheStoreDefinition cacheStoreInfo)
        {
            ICacheStore cacheStore = null;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.CacheOptions))
            {
                //CustomStoreInfo sqlServerStoreInfo =
                //    JsonConvert.DeserializeObject<CustomStoreInfo>(cacheStoreInfo.CacheOptions);
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
                //cacheStore = new CustomDistributedCacheStore<Type>() { CacheOptions = options => { } };
            }
            return cacheStore;
        }
    }
}