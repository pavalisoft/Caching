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

namespace Pavalisoft.Caching.MySql
{
    /// <summary>
    /// Represents MySQL Cache Store Configuration Information
    /// </summary>
    public class MySqlStoreInfo
    {
        /// <summary>
        /// Gets ot Sets the periodic interval to scan and delete expired items in the cache. Default is 30 minutes.
        /// </summary>
        public TimeSpan? ExpiredItemsDeletionInterval { get; set; }

        /// <summary>
        /// Gets or Sets the connection string to the database.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Gets or Sets the schema name of the cache table.
        /// </summary>
        public string SchemaName { get; set; }

        /// <summary>
        /// Gets or Sets the Name of the table where the cache items are stored.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// The default sliding expiration set for a cache entry if neither Absolute or SlidingExpiration has been set explicitly.
        /// By default, its 20 minutes.
        /// </summary>
        public TimeSpan DefaultSlidingExpiration { get; set; } = TimeSpan.FromMinutes(20.0);
    }
}
