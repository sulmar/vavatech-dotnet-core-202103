using Microsoft.AspNetCore.Razor.TagHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Vavatech.DotnetCore.MVCWebApp.TagHelpers
{
    public class CurrentTimeTagHelper : TagHelper
    {
        [HtmlAttributeName("project-title")]
        public string Text { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            string html = $"<b>{Text}</b>";

            output.Attributes.SetAttribute("class", "mb-4");
            output.TagName = "div";
            output.Content.SetHtmlContent(html);
        }
    }
}
