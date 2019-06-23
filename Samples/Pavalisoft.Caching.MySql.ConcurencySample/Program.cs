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
using Pavalisoft.Caching.MySql;

namespace Pavalisoft.Caching.SqlServer.ConcurencySample
{
    /// <summary>
    /// This sample requires setting up a Microsoft SQL Server based cache database.
    /// 1. Install the latest .NET Core mysql-cache tool globally by installing the dotnet-mysql-cache package from https://www.nuget.org/packages/Pomelo.Extensions.Caching.MySqlConfig.Tools/.
    /// 2. Create a new database in the MySQL or use an existing one.
    /// 3. Run the command "dotnet mysql-cache create <connectionstring> <schemaName> <tableName>" to setup the table and indexes.  
    /// 4. Update the connectionstring, SchemaName and TableName in the appsettings.json file
    /// 5. Run this sample by doing "dotnet run"
    /// </summary>
    public class Program
    {
        private const string MySqlStorePartition = "MySqlLocalizationData";
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
            _cacheManager.Set(MySqlStorePartition, Key, value, null, CreatePostEvictionCallbackRegistration());
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

            services.AddCaching().AddMySqlCache();

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
                        if (!_cacheManager.TryGetValue(MySqlStorePartition, Key, out object result))
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
                    _cacheManager.Remove(MySqlStorePartition, Key);
                }
            });
        }
    }
}
