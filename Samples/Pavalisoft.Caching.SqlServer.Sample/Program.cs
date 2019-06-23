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

namespace Pavalisoft.Caching.SqlServer.Sample
{
    /// <summary>
    /// This sample requires setting up a Microsoft SQL Server based cache database.
    /// 1. Install the latest .NET Core sql-cache tool globally by installing the dotnet-sql-cache package from https://www.nuget.org/packages/dotnet-sql-cache/.
    /// 2. Create a new database in the SQL Server or use an existing one.
    /// 3. Run the command "dotnet sql-cache create <connectionstring> <schemaName> <tableName>" to setup the table and indexes.  
    /// 4. Update the connectionstring, SchemaName and TableName in the appsettings.json file
    /// 5. Run this sample by doing "dotnet run"
    /// </summary>
    public class Program
    {
        private const string SqlServerStorePartition = "LocalizationData";

        public static void Main()
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

            await cacheManager.SetAsync(SqlServerStorePartition, key, message);
            Console.WriteLine("Set");

            Console.WriteLine("Getting value from cache");
            message = await cacheManager.GetAsync<string>(SqlServerStorePartition, key);
            if (!string.IsNullOrWhiteSpace(message))
            {
                Console.WriteLine("Retrieved: " + message);
            }
            else
            {
                Console.WriteLine("Not Found");
            }

            Console.WriteLine("Refreshing value in cache");
            await cacheManager.RefreshAsync(SqlServerStorePartition, key);
            Console.WriteLine("Refreshed");

            Console.WriteLine("Removing value from cache");
            await cacheManager.RemoveAsync(SqlServerStorePartition, key);
            Console.WriteLine("Removed");

            Console.WriteLine("Getting value from cache again");
            message = await cacheManager.GetAsync<string>(SqlServerStorePartition, key);
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

            services.AddCaching().AddSqlServerCache();

            IServiceProvider serviceProvider = services.BuildServiceProvider();
            ICacheManager cacheManager = serviceProvider.GetService<ICacheManager>();
            return cacheManager;
        }
    }
}
