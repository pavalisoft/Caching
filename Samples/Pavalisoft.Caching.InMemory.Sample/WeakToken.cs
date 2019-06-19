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
using Microsoft.Extensions.Primitives;

namespace Pavalisoft.Caching.InMemory.Sample
{
    public class WeakToken<T> : IChangeToken where T : class
    {
        private WeakReference<T> _reference;

        public WeakToken(WeakReference<T> reference)
        {
            _reference = reference;
        }

        public bool ActiveChangeCallbacks
        {
            get { return false; }
        }

        public bool HasChanged
        {
            get
            {
                return !_reference.TryGetTarget(out T ignored);
            }
        }

        public IDisposable RegisterChangeCallback(Action<object> callback, object state)
        {
            throw new NotSupportedException();
        }
    }
}
