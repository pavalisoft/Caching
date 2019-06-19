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
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.InMemory.ConcurencySample
{
    public class Program
    {
        private const string InMemoryStorePartition = "FrequentData";
        private const string Key = "SampleKey";
        private static readonly Random Random = new Random();
        private static ICacheManager _cacheManager;

        public static void Main()
        {
            _cacheManager = CreateCacheManager();

            SetKey("0");

            PeriodicallyReadKey(TimeSpan.FromSeconds(1));

            PeriodicallyRemoveKey(TimeSpan.FromSeconds(11));

            PeriodicallySetKey(TimeSpan.FromSeconds(13));

            Console.ReadLine();
            Console.WriteLine("Shutting down");
        }

        private static void SetKey(string value)
        {
            Console.WriteLine("Setting: " + value);
            _cacheManager.Set(InMemoryStorePartition, Key, value, null, CreatePostEvictionCallbackRegistration());
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

        private static PostEvictionCallbackRegistration CreatePostEvictionCallbackRegistration()
        {
            return new PostEvictionCallbackRegistration
            {
                EvictionCallback = (echoKey, value, reason, substate) =>
                {
                    Console.WriteLine($"Evicted. {echoKey} : {value} was evicted due to {reason}");
                }
            };
        }

        private static void PeriodicallySetKey(TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);

                    SetKey("A");
                }
            });
        }

        private static void PeriodicallyReadKey(TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);

                    if (Random.Next(3) == 0) // 1/3 chance
                    {
                        // Allow values to expire due to sliding refresh.
                        Console.WriteLine("Read skipped, random choice.");
                    }
                    else
                    {
                        Console.Write("Reading...");
                        if (!_cacheManager.TryGetValue(InMemoryStorePartition, Key, out object result))
                        {
                            result = "B";
                            SetKey(result.ToString());
                        }
                        Console.WriteLine("Read: " + (result ?? "(null)"));
                    }
                }
            });
        }

        private static void PeriodicallyRemoveKey(TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);

                    Console.WriteLine("Removing...");
                    _cacheManager.Remove(InMemoryStorePartition, Key);
                }
            });
        }
    }
}
