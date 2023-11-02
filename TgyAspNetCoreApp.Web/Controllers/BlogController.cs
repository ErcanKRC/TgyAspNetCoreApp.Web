using Microsoft.AspNetCore.Mvc;

namespace TgyAspNetCoreApp.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class BlogController : Controller
    {
        //blog/article/makale-ismi/id
        [Route("/Blog")]
        [Route("/Blog/Article")]
        [Route("/Blog/Article/{name}/{id}")]
        public IActionResult Article(string name,int id)
        {
            //var routes = Request.RouteValues["article"];

            return View();
        }

    }
}
