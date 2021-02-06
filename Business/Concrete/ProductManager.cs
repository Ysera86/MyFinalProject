using Business.Abstract;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    // Bir iş sınıfı diğer sınıfları new lemez! Injection yap!
    public class ProductManager : IProductService
    {
        IProductDAL _productDAL;

        public ProductManager(IProductDAL productDAL)
        {
            _productDAL = productDAL; // Injection > burada asla InMemory, eF vs tipler olmaz!!
        }

        public List<Product> GetAll()
        {
            // İş Kodları

            // yetkisi var mı vsvsvssv
            return _productDAL.GetAll();
        }

        public List<Product> GetAllByCategoryId(int id)
        {
            return _productDAL.GetAll(p => p.CategoryId == id);
        }

        public List<Product> GetAllByUnitPrice(decimal min, decimal max)
        {
            return _productDAL.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max);
        }

        public List<ProductDetailDTO> GetProductDetails()
        {
            return _productDAL.GetProductDetails();
        }
    }
}
