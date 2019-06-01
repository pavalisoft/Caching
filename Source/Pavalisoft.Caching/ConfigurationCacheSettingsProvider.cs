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
using Microsoft.Extensions.Configuration;

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides Json based <see cref="CacheSettings"/> configuration
    /// </summary>
    public class ConfigurationCacheSettingsProvider : CacheSettingsProvider
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Creates an instance of <see cref="ConfigurationCacheSettingsProvider"/> with <see cref="IConfiguration"/>
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/> object to read appSettings.json</param>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
        public ConfigurationCacheSettingsProvider(IConfiguration configuration, IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Loads <see cref="CacheSettings"/> object from "Caching" configuration section in appSettings.json
        /// </summary>
        /// <returns></returns>
        public override CacheSettings LoadCacheSettings()
        {
            return _configuration.GetSection("Caching").Get<CacheSettings>();
        }
    }
}
