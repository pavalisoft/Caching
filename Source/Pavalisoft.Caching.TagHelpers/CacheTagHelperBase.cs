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
using System.Globalization;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace Pavalisoft.Caching.TagHelpers
{
    /// <summary>
    /// <see cref="TagHelper"/> base implementation for caching elements.
    /// </summary>
    public abstract class CacheTagHelperBase : TagHelper
    {
        private const string VaryByAttributeName = "vary-by";
        private const string VaryByHeaderAttributeName = "vary-by-header";
        private const string VaryByQueryAttributeName = "vary-by-query";
        private const string VaryByRouteAttributeName = "vary-by-route";
        private const string VaryByCookieAttributeName = "vary-by-cookie";
        private const string VaryByUserAttributeName = "vary-by-user";
        private const string VaryByCultureAttributeName = "vary-by-culture";
        private const string EnabledAttributeName = "enabled";
        private const string CachePartitionAttributeName = "cache-partition";

        /// <summary>
        /// Creates a new <see cref="CacheTagHelperBase"/>.
        /// </summary>
        /// <param name="htmlEncoder">The <see cref="HtmlEncoder"/> to use.</param>
        public CacheTagHelperBase(HtmlEncoder htmlEncoder)
        {
            HtmlEncoder = htmlEncoder;
        }

        /// <inheritdoc />
        public override int Order => -1000;

        /// <summary>
        /// Gets the <see cref="System.Text.Encodings.Web.HtmlEncoder"/> which encodes the content to be cached.
        /// </summary>
        protected HtmlEncoder HtmlEncoder { get; }

        /// <summary>
        /// Gets or sets the <see cref="ViewContext"/> for the current executing View.
        /// </summary>
        [HtmlAttributeNotBound]
        [ViewContext]
        public ViewContext ViewContext { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string" /> to vary the cached result by.
        /// </summary>
        [HtmlAttributeName(VaryByAttributeName)]
        public string VaryBy { get; set; }

        /// <summary>
        /// Gets or sets a comma-delimited set of HTTP request headers to vary the cached result by.
        /// </summary>
        [HtmlAttributeName(VaryByHeaderAttributeName)]
        public string VaryByHeader { get; set; }

        /// <summary>
        /// Gets or sets a comma-delimited set of query parameters to vary the cached result by.
        /// </summary>
        [HtmlAttributeName(VaryByQueryAttributeName)]
        public string VaryByQuery { get; set; }

        /// <summary>
        /// Gets or sets a comma-delimited set of route data parameters to vary the cached result by.
        /// </summary>
        [HtmlAttributeName(VaryByRouteAttributeName)]
        public string VaryByRoute { get; set; }

        /// <summary>
        /// Gets or sets a comma-delimited set of cookie names to vary the cached result by.
        /// </summary>
        [HtmlAttributeName(VaryByCookieAttributeName)]
        public string VaryByCookie { get; set; }

#pragma warning disable CS1574 // XML comment has cref attribute that could not be resolved
        /// <summary>
        /// Gets or sets a value that determines if the cached result is to be varied by the Identity for the logged in
        /// <see cref="Http.HttpContext.User"/>.
        /// </summary>
        [HtmlAttributeName(VaryByUserAttributeName)]
#pragma warning restore CS1574 // XML comment has cref attribute that could not be resolved
        public bool VaryByUser { get; set; }

        /// <summary>
        /// Gets or sets a value that determines if the cached result is to be varied by request culture.
        /// <para>
        /// Setting this to <c>true</c> would result in the result to be varied by <see cref="CultureInfo.CurrentCulture" />
        /// and <see cref="CultureInfo.CurrentUICulture" />.
        /// </para>
        /// </summary>
        [HtmlAttributeName(VaryByCultureAttributeName)]
        public bool VaryByCulture { get; set; }

        /// <summary>
        /// Gets or sets the cache partition name wehere the cache entry should be created.
        /// </summary>
        [HtmlAttributeName(CachePartitionAttributeName)]
        public string CachePartition { get; set; }

        /// <summary>
        /// Gets or sets the value which determines if the tag helper is enabled or not.
        /// </summary>
        [HtmlAttributeName(EnabledAttributeName)]
        public bool Enabled { get; set; } = true;
    }
}
