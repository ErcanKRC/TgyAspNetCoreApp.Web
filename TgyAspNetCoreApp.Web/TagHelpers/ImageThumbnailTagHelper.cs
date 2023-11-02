using Microsoft.AspNetCore.Razor.TagHelpers;

namespace TgyAspNetCoreApp.Web.TagHelpers
{
    [HtmlTargetElement("thumbnails")]
    public class ImageThumbnailTagHelper : TagHelper
    {
        public string ImageSrc { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "img";

            string fileName = ImageSrc.Split('.')[0];
            string fileExtensions = Path.GetExtension(ImageSrc);

            output.Attributes.SetAttribute("src", $"{fileName}-100x100{fileExtensions}");
        }
    }
}
