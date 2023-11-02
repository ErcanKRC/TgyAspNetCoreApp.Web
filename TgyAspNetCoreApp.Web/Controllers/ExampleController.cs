using Microsoft.AspNetCore.Mvc;
using TgyAspNetCoreApp.Web.Filters;

namespace TgyAspNetCoreApp.Web.Controllers
{
    public class Product2
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    [CustomResultFilter("x-version","1.0")]
    public class ExampleController : Controller
    {
        public IActionResult Index() //Controller Courses
        {
            var productList = new List<Product2>()
            { 
                new Product2 { Id = 1,Name = "Pencil"},
                new Product2 { Id = 2,Name = "Book"},
                new Product2 { Id = 3,Name = "Eraser"}
            };
            
            ViewBag.Name = "Asp.Net Core";
            ViewData["Age"] = 58;
            ViewData["Names"] = new List<string>(){ "Ercan","Şuayip","Yunus"};
            ViewBag.Person = new { Id = 1, Name = "Ercan Enes", Age = 23 };
            TempData["Surname"] = "KARACA";


            return View(productList);
        }
        public IActionResult Index2() //Controller Courses
        {
            return View();
        }
        public IActionResult Index3() //Controller Courses
        {
            return RedirectToAction("Index", "Example");
        }

        public IActionResult Index4 () //View Courses - Razor Syntax
        {
            return View();
        }

        public IActionResult ParameterView(int id)
        {
            return RedirectToAction("JsonResultParameter", "Example", id);
        }

        public IActionResult ContentResult()
        {
            return Content("Content Result");
        }

        public IActionResult JsonResult()
        {
            return Json(new { Id = 1, name = "Pen 1", price = "100" });
        }
        public IActionResult JsonResultParameter(int id)
        {
            return Json(new { Id = id});
        }

        public IActionResult EmptyResult()
        {
            return new EmptyResult();
        }
    }
}
