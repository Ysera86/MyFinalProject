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

        [HttpGet("getall")]
        //public List<Product> Get()
        public IActionResult GetAll()
        {
            //Swagger
            //Dependency chain...
            //IProductService productService = new ProductManager(new EfProductDAL());
            var result = _productService.GetAll();
            //return result.Message; // saar 23ise ProductManager içinde bakım yapılyor mesajı
            //return result.Data;

            if (result.Success)
            {
                //return Ok(result.Data);
                return Ok(result);
            }
            else
            {
                //return BadRequest(result.Message);
                return BadRequest(result);
            }
        }

        [HttpPost("add")]
        // put : update
        // delete : delete
        // >> kullanılabilecekleri gibi sektörde %90 bunlar için de  post kullanılabiliyormuş.
        public IActionResult Add(Product product)
        {
            var result = _productService.Add(product);
            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("getbyid")]
        public IActionResult GetById(int id)
        {
            var result = _productService.GetById(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
