using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace DataAccess.Concrete.InMemory
{
    public class InMemoryProductDAL : IProductDAL
    {
        List<Product> _products;
        public InMemoryProductDAL()
        {
            // Oracle, SQL Server, Postgres, MongoDB
            _products = new List<Product> {
                new Product{ProductId =1, CategoryId=1, ProductName="Bardak", UnitPrice =15, UnitsInStock=15},
                new Product{ProductId =2, CategoryId=1, ProductName="Kamera", UnitPrice =15, UnitsInStock=3},
                new Product{ProductId =3, CategoryId=2, ProductName="Telefon", UnitPrice =1500, UnitsInStock=2},
                new Product{ProductId =4, CategoryId=2, ProductName="Klavye", UnitPrice =150, UnitsInStock=65},
                new Product{ProductId =5, CategoryId=2, ProductName="Mouse", UnitPrice =85, UnitsInStock=1},
            };
        }

        public void Add(Product product)
        {
            _products.Add(product);
        }

        public void Delete(Product product)
        {   
            // I ıh
            //Product productToDelete = null;
            //foreach (var p in _products)
            //{
            //    if (product.ProductId==p.ProductId)
            //    {
            //        productToDelete = p;
            //    }
            //}

            // LINQ = Language INtegrated Query
            // Lambda (=>)
            Product  productToDelete = _products.SingleOrDefault(x => x.ProductId == product.ProductId);
            // 2 tane gelirse SingleOrDefault hata verir. Id li yapılarda bu kontrol de sağlamış olur!

            _products.Remove(productToDelete);

            //_products.Remove(_products.Find(x => x.ProductId == product.ProductId));       
        }

        public Product Get(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public List<Product> GetAll()
        {
            return _products;
        }

        public List<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        {
            return _products;//.Where(filter).Toli;
        }

        public List<Product> GetAllByCategory(int categoryId)
        {
            return _products.Where(x => x.CategoryId == categoryId).ToList();
        }

        public List<ProductDetailDTO> GetProductDetails()
        {
            throw new NotImplementedException();
        }

        public void Update(Product product)
        {
            Product productToUpdate = _products.SingleOrDefault(x => x.ProductId == product.ProductId);
            productToUpdate.CategoryId = product.CategoryId;
            productToUpdate.ProductName = product.ProductName;
            productToUpdate.UnitPrice = product.UnitPrice;
            productToUpdate.UnitsInStock = product.UnitsInStock;
        }
    }
}
