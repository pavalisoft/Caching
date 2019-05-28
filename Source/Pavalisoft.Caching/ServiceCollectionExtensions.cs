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

using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Caching.SqlServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pavalisoft.Caching.Interfaces;

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
            services.AddSingleton<ICacheManager, CacheManager>();
            services.AddSingleton<ICacheSettingsProvider, ConfigurationCacheSettingsProvider>();

            IServiceProvider serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(services);
            if (serviceProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            ICacheSettingsProvider cacheSettingsProvider = serviceProvider.GetService<ICacheSettingsProvider>();
            if (cacheSettingsProvider == null)
                throw new ArgumentNullException(nameof(serviceProvider));

            //services.AddOptions();
            foreach (var cacheStore in cacheSettingsProvider.GetCacheStores())
            {
                if (cacheStore is ICacheStore<MemoryDistributedCacheOptions> memoryAction)
                {
                    services.AddSingleton(memoryAction.CacheType);
                    services.Configure(memoryAction.CacheOptions);
                }
                else if (cacheStore is ICacheStore<RedisCacheOptions> redisAction)
                {
                    services.AddSingleton(redisAction.CacheType);
                    services.Configure(redisAction.CacheOptions);
                }
                else if (cacheStore is ICacheStore<SqlServerCacheOptions> sqlAction)
                {
                    services.AddSingleton(sqlAction.CacheType);
                    services.Configure(sqlAction.CacheOptions);
                }
                // TODO : Needs to implement the custom Cache Store feature properly.
                //else if (cacheStore is Action<CustomCacheOptions> customAction)
                //    services.Configure(customAction);
            }
            return services;
        }
    }
}
