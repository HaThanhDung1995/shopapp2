using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShopApplication.Application.Interfaces;

namespace ShopApplication.Controllers
{
    public class ProductController : Controller
    {
        IProductService _productService;
        IBillService _billService;
        IProductCategoryService _productCategoryService;
        IConfiguration _configuration;
        public ProductController(IProductService productService, IConfiguration configuration,
            IBillService billService,
            IProductCategoryService productCategoryService)
        {
            _productService = productService;
            _productCategoryService = productCategoryService;
            _configuration = configuration;
            _billService = billService;
        }
        [Route("products.html")]
        public IActionResult Index()
        {
            return View();
        }

        [Route("{alias}-c.{id}.html")]
        public IActionResult Catalog(int id, string keyword, int? pageSize, string sortBy, int page = 1)
        {
            return View();
        }
    }
}