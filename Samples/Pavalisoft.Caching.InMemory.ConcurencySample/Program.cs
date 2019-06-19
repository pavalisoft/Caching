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
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Pavalisoft.Caching.InMemory.ConcurencySample
{
    public class Program
    {
        private const string Key = "MyKey";
        private static readonly Random Random = new Random();
        private static MemoryCacheEntryOptions _cacheEntryOptions;

        public static void Main()
        {
            _cacheEntryOptions = GetCacheEntryOptions();

            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            SetKey(cache, "0");

            PeriodicallyReadKey(cache, TimeSpan.FromSeconds(1));

            PeriodicallyRemoveKey(cache, TimeSpan.FromSeconds(11));

            PeriodicallySetKey(cache, TimeSpan.FromSeconds(13));

            Console.ReadLine();
            Console.WriteLine("Shutting down");
        }

        private static void SetKey(IMemoryCache cache, string value)
        {
            Console.WriteLine("Setting: " + value);
            cache.Set(Key, value, _cacheEntryOptions);
        }

        private static MemoryCacheEntryOptions GetCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromSeconds(7))
                .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                .RegisterPostEvictionCallback(AfterEvicted, state: null);
        }

        private static void AfterEvicted(object key, object value, EvictionReason reason, object state)
        {
            Console.WriteLine("Evicted. Value: " + value + ", Reason: " + reason);
        }

        private static void PeriodicallySetKey(IMemoryCache cache, TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);

                    SetKey(cache, "A");
                }
            });
        }

        private static void PeriodicallyReadKey(IMemoryCache cache, TimeSpan interval)
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
                        if (!cache.TryGetValue(Key, out object result))
                        {
                            result = cache.Set(Key, "B", _cacheEntryOptions);
                        }
                        Console.WriteLine("Read: " + (result ?? "(null)"));
                    }
                }
            });
        }

        private static void PeriodicallyRemoveKey(IMemoryCache cache, TimeSpan interval)
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(interval);

                    Console.WriteLine("Removing...");
                    cache.Remove(Key);
                }
            });
        }
    }
}
