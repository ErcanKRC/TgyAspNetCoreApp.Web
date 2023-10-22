using System.ComponentModel.DataAnnotations;

namespace TgyAspNetCoreApp.Web.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }
        [Required,StringLength(50,ErrorMessage ="Max 50 Character")]
        public string? Name { get; set; }
        [Required,RegularExpression(@"![0-9]+(\.[0-9]{1,2})",ErrorMessage ="Price can have 2 digits after dot.")]
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
        [Required,EmailAddress(ErrorMessage ="This adress is not valid!")]
        public string EmailAdress{ get; set; }
    }
}
