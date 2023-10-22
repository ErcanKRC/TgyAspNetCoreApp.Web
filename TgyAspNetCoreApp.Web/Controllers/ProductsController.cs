﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Composition;
using System.Drawing;
using TgyAspNetCoreApp.Web.Helpers;
using TgyAspNetCoreApp.Web.Models;
using TgyAspNetCoreApp.Web.ViewModels;

namespace TgyAspNetCoreApp.Web.Controllers
{
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
        public IActionResult Index()
        {
            var products = _context.Products.ToList();

            return View(_mapper.Map<List<ProductViewModel>>(products));
        }
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

            //2.Töntem Parameter
            /*
            string Name,decimal Price,int Stock,string Color --> fonksiyon parametreleri
            Product newProduct = new Product() { Name = Name, Price = Price, Stock = Stock, Color = Color };
            */

            if (ModelState.IsValid)
            {

                _context.Products.Add(_mapper.Map<Product>(newProduct));
                _context.SaveChanges();

                TempData["Status"] = "Product added successfully";

                return RedirectToAction("Index");
            }
            else
            {
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

                return View();
            }

        }

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

            return View(product);
        }

        [HttpPost]
        public IActionResult Update(Product updateProduct, int productId, string type)
        {
            updateProduct.Id = productId;
            _context.Products.Update(updateProduct);
            _context.SaveChanges();

            TempData["Status"] = "Product updated successfully";

            return RedirectToAction("Index");
        }
    }
}