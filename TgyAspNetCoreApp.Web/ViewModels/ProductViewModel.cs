using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace TgyAspNetCoreApp.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Remote(action: "HasProductName", controller: "Products")]
        [Required,StringLength(50,ErrorMessage ="Max 50 Character")]
        public string? Name { get; set; } 

        [Required]
        public decimal? Price { get; set; }

        [Required,Range(1,200)]
        public int? Stock { get; set; } 

        [Required(ErrorMessage = "Color selection is required")]
        public string? Color { get; set; }

        public bool IsPublish { get; set; }

        [Required(ErrorMessage = "Expire time selection is required")]
        public int? Expire { get; set; }

        [Required, StringLength(maximumLength:300,MinimumLength =50, ErrorMessage = "Min 50 - Max 300 Character")]
        public string? Description { get; set; }

        [Required]
        public DateTime? PublishDate { get; set; }

        [ValidateNever]
        public IFormFile? Image{ get; set; }
        [ValidateNever]
        public string? ImagePath { get; set; }
        [Required(ErrorMessage = "Category selection is required")]
        public int CategoryId { get; set; }
        public string? CategoryName { get; set; }
    }
}
