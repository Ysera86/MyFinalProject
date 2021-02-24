using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    // SOLID -> O :  Open Closed Principle
    // DTO : Data Transformation Object (tabloda join olma durumundaki nesneler. Örn. : ürün adı - kategori adı gibi)
    // Yazılıma yeni bir özellik eklyorsan mevcuttaki hiçbir koduna dokunamazsın : EntityFramework eklendi hiçbir koda dokunulmadı
    class Program
    {
        static void Main(string[] args)
        {
            //ProductTestByUnitPrice();

            //CategoryTest();

            ProductTest();

        }

        private static void ProductTest()
        {
            ProductManager productManager = new ProductManager(new EfProductDAL(), new CategoryManager(new EfCategoryDAL()));

            var result = productManager.GetProductDetails();
            if (result.Success)
            {
                foreach (var item in result.Data)
                {
                    Console.WriteLine(item.ProductName + " / " + item.Categoryname);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }

        private static void CategoryTest()
        {
            //IoC ile artık newlenmicek böle
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDAL());
            foreach (var category in categoryManager.GetAll().Data)
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTestByUnitPrice()
        {
            //ProductManager productManager = new ProductManager(new InMemoryProductDAL());
            ProductManager productManager = new ProductManager(new EfProductDAL(), new CategoryManager(new EfCategoryDAL()));

            var result = productManager.GetProductDetails();

            if (result.Success)
            {
                foreach (var item in productManager.GetAllByUnitPrice(50, 100).Data)
                {
                    Console.WriteLine(item.ProductName);
                }
            }
            else
            {
                Console.WriteLine(result.Message);
            }
        }
    }
}
