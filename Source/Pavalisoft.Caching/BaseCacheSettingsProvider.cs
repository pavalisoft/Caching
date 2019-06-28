﻿/* 
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

namespace Pavalisoft.Caching
{
    /// <summary>
    /// Provides Base implementation <see cref="CacheSettingsProvider"/> of <see cref="CacheSettings"/> configuration
    /// </summary>
    public class BaseCacheSettingsProvider : CacheSettingsProvider
    {
        /// <summary>
        /// Creates an instance of <see cref="ConfigurationCacheSettingsProvider"/> with <see cref="IServiceProvider"/>
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/></param>
        public BaseCacheSettingsProvider(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {            
        }

        /// <summary>
        /// Loads <see cref="CacheSettings"/> object from "Caching" configuration section in appSettings.json
        /// </summary>
        /// <returns></returns>
        public override CacheSettings LoadCacheSettings()
        {
            return ServiceProvider.GetService(typeof(CacheSettings)) as CacheSettings;
        }
    }
}