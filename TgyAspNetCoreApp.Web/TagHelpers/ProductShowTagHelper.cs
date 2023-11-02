﻿using Microsoft.AspNetCore.Razor.TagHelpers;
using TgyAspNetCoreApp.Web.Models;

namespace TgyAspNetCoreApp.Web.TagHelpers
{
    public class ProductShowTagHelper : TagHelper
    {
        public Product Product { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";

            output.Content.SetHtmlContent(@$"<ul class='list-group'>
            <li class='list-group-item'>{Product.Id}</li>
            <li class='list-group-item'>{Product.Name}</li>
            <li class='list-group-item'>{Product.Price}</li>
            <li class='list-group-item'>{Product.Stock}</li>
            </ul>");
        }
    }
}
