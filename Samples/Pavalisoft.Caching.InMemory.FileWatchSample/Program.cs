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
using System.Threading;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.InMemory.FileWatchSample
{
    public class Program
    {
        private const string InMemoryStorePartition = "FrequentData";

        public static void Main()
        {
            ICacheManager cacheManager = CreateCacheManager();
            var greeting = "";
            var cacheKey = "cache_key";
            var fileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "WatchedFiles"));
            
            do
            {
                if (!cacheManager.TryGetValue(InMemoryStorePartition, cacheKey, out greeting))
                {
                    using (var streamReader = new StreamReader(fileProvider.GetFileInfo("example.txt").CreateReadStream()))
                    {
                        greeting = streamReader.ReadToEnd();
                        PostEvictionCallbackRegistration postEvictionCallbackRegistration = new PostEvictionCallbackRegistration
                        {
                            EvictionCallback = (echoKey, value, reason, substate) =>
                            {
                                Console.WriteLine($"{echoKey} : {value} was evicted due to {reason}");
                            }
                        };
                                                
                        cacheManager.Set(InMemoryStorePartition, cacheKey, new object(), fileProvider.Watch("example.txt"), postEvictionCallbackRegistration);

                        Console.WriteLine($"{cacheKey} updated from source.");
                    }
                }
                else
                {
                    Console.WriteLine($"{cacheKey} retrieved from cache.");
                }

                Console.WriteLine(greeting);
                Console.WriteLine("Press any key to continue. Press the ESC key to exit");
            }
            while (Console.ReadKey(true).Key != ConsoleKey.Escape);
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

            services.AddCaching().AddInMemoryCache();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ICacheManager cacheManager = serviceProvider.GetService<ICacheManager>();
            return cacheManager;
        }
    }
}
