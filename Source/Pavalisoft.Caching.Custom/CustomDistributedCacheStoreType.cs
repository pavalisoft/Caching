using System;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Custom
{
    /// <summary>
    /// 
    /// </summary>
    public class CustomDistributedCacheStoreType : ICacheStoreType
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Creates an instance of <see cref="CustomDistributedCacheStoreType"/> with <see cref="IServiceProvider"/>
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> instance. </param>
        public CustomDistributedCacheStoreType(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Creates <see cref="RedisDistributedCacheStore"/> from <see cref="CacheStoreDefinition"/> configuration
        /// </summary>
        /// <param name="cacheStoreInfo">Redis <see cref="CacheStoreDefinition"/> configuration</param>
        /// <returns><see cref="CustomDistributedCacheStore"/> object</returns>
        public ICacheStore CreateCacheStore(CacheStoreDefinition cacheStoreInfo)
        {
            ICacheStore cacheStore = null;
            if (!string.IsNullOrWhiteSpace(cacheStoreInfo.StoreConfig))
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