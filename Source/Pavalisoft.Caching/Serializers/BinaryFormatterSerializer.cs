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

using Pavalisoft.Caching.Interfaces;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Pavalisoft.Caching.Serializers
{
    /// <summary>
    /// <see cref="BinaryFormatter"/> implementation of Cache object <see cref="ISerializer"/>
    /// </summary>
    public class BinaryFormatterSerializer : ISerializer
    {
        private readonly BinaryFormatter _binaryFormatter = new BinaryFormatter();

        /// <inheritdoc />
        public T Deserialize<T>(byte[] serializedObject)
        {
            using (var ms = new MemoryStream(serializedObject))
            {
                return (T)_binaryFormatter.Deserialize(ms);
            }
        }

        /// <inheritdoc />
        public Task<T> DeserializeAsync<T>(byte[] serializedObject)
        {
            return Task.Factory.StartNew(() => Deserialize<T>(serializedObject));
        }

        /// <inheritdoc />
        public byte[] Serialize<T>(T item)
        {
            using (var ms = new MemoryStream())
            {
                _binaryFormatter.Serialize(ms, item);
                return ms.ToArray();
            }
        }

        /// <inheritdoc />
        public Task<byte[]> SerializeAsync<T>(T item)
        {
            return Task.Factory.StartNew(() => Serialize(item));
        }
    }
}