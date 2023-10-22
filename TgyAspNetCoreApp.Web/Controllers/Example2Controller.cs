using Microsoft.AspNetCore.Mvc;

namespace TgyAspNetCoreApp.Web.Controllers
{
    public class Example2Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult NoLayout()
        {
            return View();
        }
    }
}
