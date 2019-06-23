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
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Redis.Sample
{
    /// <summary>
    /// This sample assumes that a redis server is running on the local machine. You can set this up by doing the following:
    /// 1. Install this chocolatey package: http://chocolatey.org/packages/redis-64/
    /// 2. run "redis-server" from command prompt.
    /// OR
    /// 1. Install Redis on windows 10 using https://redislabs.com/blog/redis-on-windows-10/  
    /// 2. run "redis-server" from command prompt.
    /// </summary>
    public class Program
    {
        private const string RedisStorePartition = "MasterData";

        public static void Main(string[] args)
        {
            RunSampleAsync().Wait();
        }
        
        public static async Task RunSampleAsync()
        {
            var key = Guid.NewGuid().ToString();
            var message = "Hello, World!";

            Console.WriteLine("Connecting to cache");
            ICacheManager cacheManager = CreateCacheManager();
            Console.WriteLine("Connected");

            Console.WriteLine("Cache item key: {0}", key);
            Console.WriteLine($"Setting value '{message}' in cache");

            await cacheManager.SetAsync(RedisStorePartition, key, message);
            Console.WriteLine("Set");

            Console.WriteLine("Getting value from cache");
            message = await cacheManager.GetAsync<string>(RedisStorePartition, key);
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Retrieved: " + message);
            }
            else
            {
                Console.WriteLine("Not Found");
            }

            Console.WriteLine("Refreshing value in cache");
            await cacheManager.RefreshAsync(RedisStorePartition, key);
            Console.WriteLine("Refreshed");

            Console.WriteLine("Removing value from cache");
            await cacheManager.RemoveAsync(RedisStorePartition, key);
            Console.WriteLine("Removed");

            Console.WriteLine("Getting value from cache again");
            message = await cacheManager.GetAsync<string>(RedisStorePartition, key);
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Retrieved: " + message);
            }
            else
            {
                Console.WriteLine("Not Found");
            }

            Console.ReadLine();
        }

        private static ICacheManager CreateCacheManager()
        {
            IServiceCollection services = new ServiceCollection();

            // build configuration
            IConfiguration configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", true, true)
              .Build();
            services.AddOptions();
            services.AddSingleton(configuration);

            services.AddCaching().AddRedisCache();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ICacheManager cacheManager = serviceProvider.GetService<ICacheManager>();
            return cacheManager;
        }
    }
}
