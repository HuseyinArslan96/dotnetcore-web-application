using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{

    public class ProductController : Controller
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        public IActionResult Index()
        {
            var products = _productService.GetProductsAsync().Result;
            return View(products);
        }

        public IActionResult Detail(int id)
        {
            var product = _productService.GetProductDetail(id);
            return View(product);
        }

        [HttpPost]
        public IActionResult AddToBasket(BasketItem item)
        {
            var success = _productService.AddProductToBasket(item);
            if (!success)
            {
                TempData["Message"] = "Not enough stock for this product.";
            }

            return RedirectToAction("Detail", new { id = item.ProductId });
        }

        [HttpPost]
        public IActionResult MakePayment()
        {
            var success = _productService.MakePayment();
            if (success)
            {
                TempData["Message"] = "Payment successful!";
            }
            else
            {
                TempData["Message"] = "Payment failed. Please try again later.";
            }

            return RedirectToAction("Index");
        }
    }

}
