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
using Microsoft.Extensions.Caching.Memory;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching
{
    public class CachePartition : ICachePartition
    {
        public CachePartition(string name, DateTimeOffset? absoluteExpiration,
            TimeSpan? absoluteExpirationRelativeToNow, TimeSpan? slidingExpiration, object store,
            CacheItemPriority priority, long? size)
        {
            Name = name;
            AbsoluteExpiration = absoluteExpiration;
            AbsoluteExpirationRelativeToNow = absoluteExpirationRelativeToNow;
            SlidingExpiration = slidingExpiration;
            Store = store;
            Priority = priority;
            Size = size;
        }

        public string Name { get; }
        public DateTimeOffset? AbsoluteExpiration { get; }
        public TimeSpan? AbsoluteExpirationRelativeToNow { get; }
        public TimeSpan? SlidingExpiration { get; }
        public object Store { get; }
        public CacheItemPriority Priority { get; }
        public long? Size { get; }
        public ICache Cache { get; set; }
    }
}
