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

namespace Pavalisoft.Caching.Interfaces
{
    public interface ICachePartition
    {
        string Name { get; }
        DateTimeOffset? AbsoluteExpiration { get; }
        TimeSpan? AbsoluteExpirationRelativeToNow { get; }
        TimeSpan? SlidingExpiration { get; }
        object Store { get; }
        CacheItemPriority Priority { get; }
        long? Size { get; }
        ICache Cache { get; set; }
    }
}
