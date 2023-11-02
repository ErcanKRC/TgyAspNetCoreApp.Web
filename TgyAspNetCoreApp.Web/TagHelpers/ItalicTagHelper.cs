using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TgyAspNetCoreApp.Web.TagHelpers
{
    public class ItalicTagHelper :TagHelper
    {
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.PreContent.SetHtmlContent("<i>");
            output.PostContent.SetHtmlContent("</li>");
        }
    }
}
