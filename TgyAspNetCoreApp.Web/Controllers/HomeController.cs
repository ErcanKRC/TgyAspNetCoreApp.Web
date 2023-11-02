using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TgyAspNetCoreApp.Web.Models;
using TgyAspNetCoreApp.Web.ViewModels;

namespace TgyAspNetCoreApp.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;
        private readonly IMapper _mapper;
        public HomeController(ILogger<HomeController> logger, AppDbContext context, IMapper mapper)
        {
            _logger = logger;
            _appDbContext = context;
            _mapper = mapper;
        }
        [Route("")]
        [Route("home")]
        [Route("home/index")]
        public IActionResult Index()
        {
            var products = _appDbContext.Products.OrderByDescending(x => x.Id).Select(x => new ProductPartialViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
            }).ToList();

            ViewBag.ProductListPartialViewModel = new ProductListPartialViewModel()
            {
               Products = (List<ProductPartialViewModel>)products,
            };

            return View();
        }

        public IActionResult Privacy()
        {
            var products = _appDbContext.Products.OrderByDescending(x => x.Id).Select(x => new ProductPartialViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Stock = x.Stock,
            }).ToList();

            ViewBag.ProductListPartialViewModel = new ProductListPartialViewModel()
            {
                Products = (List<ProductPartialViewModel>)products,
            };

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Visitor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SaveVisitorComment (VisitorViewModel visitorViewModel)
        {
            try
            {
                var visitor = _mapper.Map<Visitor>(visitorViewModel);
                _appDbContext.Visitors.Add(visitor);
                visitor.Created = DateTime.Now;
                _appDbContext.SaveChanges();

                TempData["Result"] = "Comment Saved";
                return RedirectToAction(nameof(HomeController.Visitor));
            }
            catch (Exception)
            {
                TempData["Result"] = "An Error occured while saving the comment";
                return RedirectToAction(nameof(HomeController.Visitor));
            }
        }
    }
}