using Business.Abstract;
using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        // Loosely coupled = gevşek bağımlılık
        IProductService _productService;

        // IoC (Inversion of Control (değişimin kontrolü)) Container >>>> productService newlemek lazım ama nasıl?
        // --> WebAPI.Startup.cs.ConfigureServices >> 

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public List<Product> Get()
        {
            //Dependency chain...
            //IProductService productService = new ProductManager(new EfProductDAL());
            var result = _productService.GetAll();
            //return result.Message; // saar 23ise ProductManager içinde bakım yapılyor mesajı
            return result.Data;
        }
    }
}
