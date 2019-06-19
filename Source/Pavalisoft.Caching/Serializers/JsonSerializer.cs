
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

using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.Serializers
{
    /// <summary>
    /// Json implementation of Cache object <see cref="ISerializer"/>
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        private static readonly Encoding encoding = Encoding.UTF8;

        private readonly JsonSerializerSettings _settings;

        /// <summary>
        /// Creates an instance of <see cref="JsonSerializer"/> class with json serialization <paramref name="settings"/>
        /// </summary>
        /// <param name="settings">Optional. The <see cref="JsonSerializerSettings"/></param>
        public JsonSerializer(JsonSerializerSettings settings = default)
        {
            _settings = settings ?? new JsonSerializerSettings();
        }

        /// <inheritdoc />
        public byte[] Serialize<T>(T item)
        {
            var jsonString = JsonConvert.SerializeObject(item, typeof(T), _settings);
            return encoding.GetBytes(jsonString);
        }

        /// <inheritdoc />
        public Task<byte[]> SerializeAsync<T>(T item)
        {
            return Task.FromResult(Serialize(item));
        }

        /// <inheritdoc />
        public T Deserialize<T>(byte[] bytes)
        {
            var jsonString = encoding.GetString(bytes);
            return JsonConvert.DeserializeObject<T>(jsonString, _settings);
        }

        /// <inheritdoc />
        public Task<T> DeserializeAsync<T>(byte[] bytes)
        {
            return Task.FromResult(Deserialize<T>(bytes));
        }
    }
}