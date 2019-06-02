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

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pavalisoft.Caching.Interfaces;
using Pavalisoft.Caching.StoreTypes;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides <see cref="IServiceCollection"/> extensions for Cache Manger integration
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Caching Middleware to pipeline with Cache Manager functionality
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> instance</param>
        /// <param name="configuration"><see cref="IConfiguration"/> instance</param>
        /// <returns><see cref="IServiceCollection"/> add with Cache Manager</returns>
        public static IServiceCollection AddCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICache, Cache.Cache>();
            services.AddTransient<ICachePartition, CachePartition>();

            services.AddSingleton<InMemoryCacheStoreType>();
            services.AddSingleton<MemoryDistributedCacheStoreType>();
            services.AddSingleton<MySqlDistributedCacheStoreType>();
            services.AddSingleton<RedisDistributedCacheStoreType>();
            services.AddSingleton<SqlServerDistributedCacheStoreType>();
            services.AddSingleton<CustomDistributedCacheStoreType>();

            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<ICacheSettingsProvider, ConfigurationCacheSettingsProvider>();
            return services;
        }
    }
}
