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

using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Pavalisoft.Caching.InMemory
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
        /// <returns><see cref="IServiceCollection"/> add with Cache Manager</returns>
        public static IServiceCollection AddInMemoryCache(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryCacheStoreType>();
            services.AddSingleton<MemoryDistributedCacheStoreType>();
            services.AddTransient<ExtendedMemoryCache>();
            services.AddTransient<ExtendedMemoryDistributedCache>();
            return services;
        }

        private static byte[] ToArray(this object obj)
        {
            if (obj == null)
            {
                return null;
            }

            using (var memoryStream = new MemoryStream())
            {
                var binaryFormatter = new BinaryFormatter();

                binaryFormatter.Serialize(memoryStream, obj);

                return memoryStream.ToArray();
            }
        }

        private static object ToObject(this byte[] arrBytes)
        {
            using (var memoryStream = new MemoryStream())
            {
                memoryStream.Write(arrBytes, 0, arrBytes.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);

                var binaryFormatter = new BinaryFormatter();
                return binaryFormatter.Deserialize(memoryStream);
            }
        }
    }
}
