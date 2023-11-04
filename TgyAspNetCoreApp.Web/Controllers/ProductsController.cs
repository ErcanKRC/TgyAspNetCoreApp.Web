using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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
        private readonly IFileProvider _fileProvider;
        public ProductsController(AppDbContext context, IMapper mapper, IFileProvider fileProvider)
        {
            _productRepository = new ProductRepository();
            _context = context;
            _mapper = mapper;
            _fileProvider = fileProvider;


            //if(!_context.Products.Any())
            //{
            //    _context.Products.Add(new Product { Name = "Kalem 1", Price = 100, Stock = 58 ,Color="Red"});
            //    _context.Products.Add(new Product { Name = "Kalem 2", Price = 150, Stock = 34 ,Color = "Red"});
            //    _context.Products.Add(new Product { Name = "Kalem 3", Price = 500, Stock = 10 ,Color = "Red" });

            //    _context.SaveChanges();
            //}
        }

        //[CacheResourceFilter]
        [Route("/products")]
        [Route("/products/index")]
        public IActionResult Index()
        {
            List<ProductViewModel> products = _context.Products.Include(x => x.Category).Select(x => new ProductViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                CategoryName = x.Category.Name,
                Color = x.Color,
                Expire = x.Expire,
                ImagePath = x.ImagePath,
                IsPublish = x.IsPublish,
                Price = x.Price,
                PublishDate = x.PublishDate,
                Stock = x.Stock
            }).ToList();


            return View(products);
        }

        [Route("[action]/{page}/{pageSize}", Name = "productpage")]
        public IActionResult Pages(int page, int pageSize)
        {

            //page = 1 pagesize=3 => ilk 3 kayıt
            //page = 2 pagesize=3 => ikinci 3 kayıt
            //page = 3 pagesize=3 => üçüncü 3 kayıt

            var products = _context.Products.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            ViewBag.page = page;
            ViewBag.pageSize = pageSize;


            return View(_mapper.Map<List<ProductViewModel>>(products));
        }

        [ServiceFilter(typeof(NotFoundFilter))]
        [Route("/product/{productid}", Name = "product")]
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

            var categories = _context.Category.ToList();
            ViewBag.Categories = new SelectList(categories,"Id","Name");

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

            IActionResult result = null;


            if (ModelState.IsValid)
            {
                try
                {

                    var product = _mapper.Map<Product>(newProduct);

                    if (newProduct.Image!= null && newProduct.Image.Length >0)
                    {
                        var root = _fileProvider.GetDirectoryContents("wwwroot");
                        var images = root.First(x => x.Name == "images");

                        var randomImageName = Guid.NewGuid() + Path.GetExtension(newProduct.Image.FileName);

                        var path = Path.Combine(images.PhysicalPath, randomImageName);
                        using var stream = new FileStream(path, FileMode.Create);
                        newProduct.Image.CopyTo(stream);

                        product.ImagePath = randomImageName;
                    }

                    _context.Products.Add(product);
                    _context.SaveChanges();

                    TempData["Status"] = "Product added successfully";

                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError(string.Empty, "An error occured while product saving. Please try again later.");
                    result = View();
                }
            }
            else
            {
                result = View();
            }
            var categories = _context.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

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

            return result;

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

            var categories = _context.Category.ToList();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");

            return View(_mapper.Map<ProductUpdateViewModel>(product));
        }

        [HttpPost]
        public IActionResult Update(ProductUpdateViewModel updateProduct)
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

                var categories = _context.Category.ToList();
                ViewBag.Categories = new SelectList(categories, "Id", "Name",updateProduct.CategoryId);

                return View();
            }

            if (updateProduct.Image != null && updateProduct.Image.Length > 0)
            {
                var root = _fileProvider.GetDirectoryContents("wwwroot");
                var images = root.First(x => x.Name == "images");

                var randomImageName = Guid.NewGuid() + Path.GetExtension(updateProduct.Image.FileName);

                var path = Path.Combine(images.PhysicalPath, randomImageName);
                using var stream = new FileStream(path, FileMode.Create);
                updateProduct.Image.CopyTo(stream);

                updateProduct.ImagePath = randomImageName;
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
