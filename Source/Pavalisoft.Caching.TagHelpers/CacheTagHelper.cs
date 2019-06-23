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
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Pavalisoft.Caching.Interfaces;

namespace Pavalisoft.Caching.TagHelpers
{
    /// <summary>
    /// <see cref="TagHelper"/> implementation targeting &lt;pavalisoft-cache&gt; elements.
    /// </summary>
    /// <example> The below is the usage example
    /// <pavalisoft-cache enabled="true" cache-partition="FrequentData">
    ///    Current Time Inside Cache Tag Helper: @DateTime.Now
    /// </pavalisoft-cache>
    /// </example>
    [HtmlTargetElement("pavalisoft-cache")]
    public class CacheTagHelper : CacheTagHelperBase
    {
        private readonly ConcurrentDictionary<CacheTagKey, Task<IHtmlContent>> _workers;
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Creates a new <see cref="CacheTagHelper"/>.
        /// </summary>
        /// <param name="cacheManager">The <see cref="ICacheManager"/> instance
        /// used by the <see cref="CacheTagHelper"/>.</param>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/> to use.</param>
        public CacheTagHelper(ICacheManager cacheManager, HtmlEncoder htmlEncoder)
            : base(htmlEncoder)
        {
            _cacheManager = cacheManager;
            _workers = new ConcurrentDictionary<CacheTagKey, Task<IHtmlContent>>();
        }

        /// <inheritdoc />
        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (output == null)
            {
                throw new ArgumentNullException(nameof(output));
            }

            IHtmlContent content;
            if (Enabled)
            {
                var cacheKey = new CacheTagKey(this, context);

                content = await ProcessContentAsync(output, cacheKey);
            }
            else
            {
                content = await output.GetChildContentAsync();
            }

            output.SuppressOutput();
            output.Content.SetHtmlContent(content);
        }

        private async Task<IHtmlContent> ProcessContentAsync(TagHelperOutput output, CacheTagKey key)
        {
            IHtmlContent content = null;
            while (content == null)
            {
                Task<IHtmlContent> result;
                if (!_workers.TryGetValue(key, out result))
                {
                    var tcs = new TaskCompletionSource<IHtmlContent>(creationOptions: TaskCreationOptions.RunContinuationsAsynchronously);

                    _workers.TryAdd(key, tcs.Task);

                    try
                    {
                        var serializedKey = Encoding.UTF8.GetBytes(key.GenerateKey());
                        var storageKey = key.GenerateHashedKey();
                        var value = await GetAsync(storageKey);

                        if (value == null)
                        {
                            var processedContent = await output.GetChildContentAsync();

                            var stringBuilder = new StringBuilder();
                            using (var writer = new StringWriter(stringBuilder))
                            {
                                processedContent.WriteTo(writer, HtmlEncoder);
                            }

                            HtmlString htmlString = new HtmlString(stringBuilder.ToString());

                            value = await SerializeAsync(htmlString);

                            var encodeValue = Encode(value, serializedKey);

                            await SetAsync(storageKey, encodeValue);

                            content = htmlString;
                        }
                        else
                        {
                            byte[] decodedValue = Decode(value, serializedKey);

                            try
                            {
                                if (decodedValue != null)
                                {
                                    content = await DeserializeAsync(decodedValue);
                                }
                            }
                            catch
                            {
                                throw;
                            }
                            finally
                            {
                                if (content == null)
                                {
                                    content = await output.GetChildContentAsync();
                                }
                            }
                        }
                    }
                    catch
                    {
                        content = null;
                        throw;
                    }
                    finally
                    {
                        _workers.TryRemove(key, out result);
                        tcs.TrySetResult(content);
                    }
                }
            }
            return content;
        }

        #region ICacheManager
        private Task<byte[]> GetAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            string htmlString = _cacheManager.GetAsync<string>(CachePartition, key).Result;
            return Task.FromResult(string.IsNullOrWhiteSpace(htmlString) ? (byte[])null : Convert.FromBase64String(htmlString));
        }

        private Task SetAsync(string key, byte[] value)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            return _cacheManager.SetAsync(CachePartition, key, Convert.ToBase64String(value));
        }
        #endregion

        #region HtmlString Serialization
        private Task<byte[]> SerializeAsync(HtmlString html)
        {
            if (html == null)
            {
                throw new ArgumentNullException(nameof(html));
            }

            var serialized = Encoding.UTF8.GetBytes(html.ToString());
            return Task.FromResult(serialized);
        }

        private Task<HtmlString> DeserializeAsync(byte[] value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            var content = Encoding.UTF8.GetString(value);
            return Task.FromResult(new HtmlString(content));
        }
        #endregion

        #region HtmlString Formating
        private byte[] Encode(byte[] value, byte[] serializedKey)
        {
            using (var buffer = new MemoryStream())
            {
                var keyLength = BitConverter.GetBytes(serializedKey.Length);

                buffer.Write(keyLength, 0, keyLength.Length);
                buffer.Write(serializedKey, 0, serializedKey.Length);
                buffer.Write(value, 0, value.Length);

                return buffer.ToArray();
            }
        }

        private byte[] Decode(byte[] value, byte[] expectedKey)
        {
            byte[] decoded = null;

            using (var buffer = new MemoryStream(value))
            {
                var keyLengthBuffer = new byte[sizeof(int)];
                buffer.Read(keyLengthBuffer, 0, keyLengthBuffer.Length);

                var keyLength = BitConverter.ToInt32(keyLengthBuffer, 0);
                var serializedKeyBuffer = new byte[keyLength];
                buffer.Read(serializedKeyBuffer, 0, serializedKeyBuffer.Length);

                if (serializedKeyBuffer.SequenceEqual(expectedKey))
                {
                    decoded = new byte[value.Length - keyLengthBuffer.Length - serializedKeyBuffer.Length];
                    buffer.Read(decoded, 0, decoded.Length);
                }
            }

            return decoded;
        }
        #endregion
    }
}