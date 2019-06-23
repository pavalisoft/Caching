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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.ViewFeatures.Internal;
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
        /// <summary>
        /// Prefix used by <see cref="CacheTagHelper"/> instances when creating entries in <see cref="ICache"/>.
        /// </summary>
        public static readonly string CacheKeyPrefix = nameof(CacheTagHelper);

        //private const string NameAttributeName = "name";

        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Creates a new <see cref="CacheTagHelper"/>.
        /// </summary>
        /// <param name="cacheManager">The <see cref="ICacheManager"/> instance
        /// used by the <see cref="CacheTagHelper"/>.</param>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/> to use.</param>
        public CacheTagHelper(ICacheManager cacheManager,
            HtmlEncoder htmlEncoder)
            : base(htmlEncoder)
        {
            _cacheManager = cacheManager;
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
                if (!_cacheManager.TryGetValue(CachePartition, cacheKey.GenerateHashedKey(), out Task<IHtmlContent> cachedResult))
                {
                    // There is either some value already not cached (as a Task) or a worker processing the output.                    
                    cachedResult = ProcessContentAsync(output);
                    _cacheManager.Set(CachePartition, cacheKey.GenerateHashedKey(), cachedResult);
                }
                content = await cachedResult;                
            }
            else
            {
                content = await output.GetChildContentAsync();
            }

            // Clear the contents of the "cache" element since we don't want to render it.
            output.SuppressOutput();
            output.Content.SetHtmlContent(content);
        }

        private async Task<IHtmlContent> ProcessContentAsync(TagHelperOutput output)
        {
            var content = await output.GetChildContentAsync();

            using (var writer = new CharBufferTextWriter())
            {
                content.WriteTo(writer, HtmlEncoder);
                return new CharBufferHtmlContent(writer.Buffer);
            }
        }

        private class CharBufferTextWriter : TextWriter
        {
            public CharBufferTextWriter()
            {
                Buffer = new PagedCharBuffer(CharArrayBufferSource.Instance);
            }

            public override Encoding Encoding => Null.Encoding;

            public PagedCharBuffer Buffer { get; }

            public override void Write(char value)
            {
                Buffer.Append(value);
            }

            public override void Write(char[] buffer, int index, int count)
            {
                Buffer.Append(buffer, index, count);
            }

            public override void Write(string value)
            {
                Buffer.Append(value);
            }
        }

        private class CharBufferHtmlContent : IHtmlContent
        {
            private readonly PagedCharBuffer _buffer;

            public CharBufferHtmlContent(PagedCharBuffer buffer)
            {
                _buffer = buffer;
            }

            public PagedCharBuffer Buffer => _buffer;

            public void WriteTo(TextWriter writer, HtmlEncoder encoder)
            {
                var length = Buffer.Length;
                if (length == 0)
                {
                    return;
                }

                for (var i = 0; i < Buffer.Pages.Count; i++)
                {
                    var page = Buffer.Pages[i];
                    var pageLength = Math.Min(length, page.Length);
                    writer.Write(page, index: 0, count: pageLength);
                    length -= pageLength;
                }

                Debug.Assert(length == 0);
            }
        }
    }
}
