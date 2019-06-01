using System;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Mvc.TagHelpers;

namespace Pavalisoft.Caching.TagHelpers
{
    /// <summary>
    /// Cache Tag Helper https://github.com/aspnet/AspNetCore/blob/c565386a3ed135560bc2e9017aa54a950b4e35dd/src/Mvc/Mvc.TagHelpers/src/CacheTagHelper.cs
    /// </summary>
    public class CacheTagHelper : CacheTagHelperBase
    {
        /// <inheritdoc />
        public CacheTagHelper(HtmlEncoder htmlEncoder) : base(htmlEncoder)
        {
        }
    }
}
