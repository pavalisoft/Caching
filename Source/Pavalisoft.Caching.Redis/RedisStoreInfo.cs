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

namespace Pavalisoft.Caching.Redis
{
    /// <summary>
    /// Represents Redis Cache Store Configuration Information
    /// </summary>
    public class RedisStoreInfo
    {
        /// <summary>
        /// Gets or Sets the configuration used to connect to Redis.
        /// </summary>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or Sets the Redis instance name.
        /// </summary>
        public string InstanceName { get; set; }
    }
}