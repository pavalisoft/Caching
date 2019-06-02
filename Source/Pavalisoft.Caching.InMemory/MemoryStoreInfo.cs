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

namespace Pavalisoft.Caching.InMemory
{
    /// <summary>
    /// Represents in-Memory Store Configuration Information
    /// </summary>
    public class MemoryStoreInfo
    {
        /// <summary>
        /// Gets or Sets expiration scan frequency of the cache items
        /// </summary>
        public TimeSpan ExpirationScanFrequency { get; set; } = TimeSpan.FromMinutes(1.0);

        /// <summary>
        /// Gets or Sets the In-Memory Store size
        /// </summary>
        public long? SizeLimit { get; set; }

        /// <summary>
        /// Gets or Sets the cache items compaction percentage
        /// </summary>
        public double CompactionPercentage { get; set; }
    }
}