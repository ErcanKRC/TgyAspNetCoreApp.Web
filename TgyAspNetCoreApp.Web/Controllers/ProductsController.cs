using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Composition;
using System.Drawing;
using TgyAspNetCoreApp.Web.Filters;
using TgyAspNetCoreApp.Web.Helpers;
using TgyAspNetCoreApp.Web.Models;
using TgyAspNetCoreApp.Web.ViewModels;

namespace TgyAspNetCoreApp.Web.Controllers
{
    [Route("[controller]/[action]")]
    public class ProductsController : Controller
    {
        private AppDbContext _context;
        private readonly ProductRepository _productRepository;
        private readonly IMapper _mapper;
        public ProductsController(AppDbContext context, IMapper mapper)
        {
            _productRepository = new ProductRepository();
            _context = context;
            _mapper = mapper;

            
            //if(!_context.Products.Any())
            //{
            //    _context.Products.Add(new Product { Name = "Kalem 1", Price = 100, Stock = 58 ,Color="Red"});
            //    _context.Products.Add(new Product { Name = "Kalem 2", Price = 150, Stock = 34 ,Color = "Red"});
            //    _context.Products.Add(new Product { Name = "Kalem 3", Price = 500, Stock = 10 ,Color = "Red" });

            //    _context.SaveChanges();
            //}
        }

        [CacheResourceFilter]
        [Route("/products")]
        [Route("/products/index")]
        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }

        [Route("[action]/{page}/{pageSize}", Name = "productpage")]
        public IActionResult Pages(int page,int pageSize)
        {

            //page = 1 pagesize=3 => ilk 3 kayıt
            //page = 2 pagesize=3 => ikinci 3 kayıt
            //page = 3 pagesize=3 => üçüncü 3 kayıt

            var products = _context.Products.Skip((page-1)*pageSize).Take(pageSize).ToList();

            ViewBag.page = page;
            ViewBag.pageSize = pageSize;


            return View(_mapper.Map<List<ProductViewModel>>(products));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [Route("/product/{productid}", Name ="product")]
        public IActionResult GetById(int productid)
        {
            var product = _context.Products.Find(productid);
            return View(_mapper.Map<ProductViewModel>(product));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        public IActionResult Remove(int id)
        {
            var product = _context.Products.Find(id);

            _context.Products.Remove(product!);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Add()
        {

            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Month", 1 },
                {"3 Month", 3 },
                {"6 Month", 6 },
                {"12 Month", 12},
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new (){Data = "Blue",Value ="Blue"},
                new (){Data = "Red",Value ="Red"},
                new (){Data = "Yellow",Value ="Yellow"}
            }, "Value", "Data");

            return View();
        }
        [HttpPost]
        public IActionResult Add(ProductViewModel newProduct)
        {

            //1.yöntem
            /*
            var name = HttpContext.Request.Form["Name"].ToString();
            var price = decimal.Parse(HttpContext.Request.Form["Price"].ToString());
            var stock = int.Parse(HttpContext.Request.Form["Stock"].ToString());
            var color = HttpContext.Request.Form["Color"].ToString();

            Product newProduct = new Product(){Name = name, Price = price, Stock = stock, Color = color};
            */

            //2.yöntem Parameter
            /*
            string Name,decimal Price,int Stock,string Color --> fonksiyon parametreleri
            Product newProduct = new Product() { Name = Name, Price = Price, Stock = Stock, Color = Color };
            */

            ViewBag.Expire = new Dictionary<string, int>()
                {
                    {"1 Month", 1 },
                    {"3 Month", 3 },
                    {"6 Month", 6 },
                    {"12 Month", 12},
                };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>()
                {
                    new (){Data = "Blue",Value ="Blue"},
                    new (){Data = "Red",Value ="Red"},
                    new (){Data = "Yellow",Value ="Yellow"}
                }, "Value", "Data");

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Products.Add(_mapper.Map<Product>(newProduct));
                    _context.SaveChanges();

                    TempData["Status"] = "Product added successfully";

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occured while product saving. Please try again later.");
                    return View();
                }
            }
            else
            {
                return View();
            }

        }

        [ServiceFilter(typeof(NotFoundFilter))]
        public IActionResult Update(int id)
        {
            var product = _context.Products.Find(id);

            ViewBag.ExpireValue = product.Expire;

            ViewBag.Expire = new Dictionary<string, int>()
            {
                {"1 Month", 1 },
                {"3 Month", 3 },
                {"6 Month", 6 },
                {"12 Month", 12},
            };

            ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new (){Data = "Blue",Value ="Blue"},
                new (){Data = "Red",Value ="Red"},
                new (){Data = "Yellow",Value ="Yellow"}
            }, "Value", "Data", product.Color);

            return View(_mapper.Map<ProductViewModel>(product));
        }

        [HttpPost]
        public IActionResult Update(ProductViewModel updateProduct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ExpireValue = updateProduct.Expire;

                ViewBag.Expire = new Dictionary<string, int>()
                {
                    {"1 Month", 1 },
                    {"3 Month", 3 },
                    {"6 Month", 6 },
                    {"12 Month", 12},
                };

                ViewBag.ColorSelect = new SelectList(new List<ColorSelectList>() {
                new (){Data = "Blue",Value ="Blue"},
                new (){Data = "Red",Value ="Red"},
                new (){Data = "Yellow",Value ="Yellow"}
                }, "Value", "Data", updateProduct.Color);

                return View();
            }

            _context.Products.Update(_mapper.Map<Product>(updateProduct));
            _context.SaveChanges();

            TempData["Status"] = "Product updated successfully";

            return RedirectToAction("Index");
        }

        [HttpGet, HttpPost]
        public IActionResult HasProductName(string Name)
        {
            
            var anyProduct = _context.Products.Any(x => x.Name.ToLower() == Name.ToLower());

            if (anyProduct)
            {
                return Json($"Database has a product named \"{Name}\"");
            }
            else
            {
                return Json(true);
            }
        }
    }
}
