using Business.Concrete;
using DataAccess.Concrete.EntityFramework;
using DataAccess.Concrete.InMemory;
using System;

namespace ConsoleUI
{
    // SOLID
    // Open Closed Principle
    // Yazılıma yeni bir özellik eklyorsan mevcuttaki hiçbir koduna dokunamazsın : EntityFramework eklendi hiçbir koda dokunulmadı
    class Program
    {
        static void Main(string[] args)
        {
            //ProductTest();

            //IoC ile artık newlenmicek böle
            CategoryManager categoryManager = new CategoryManager(new EfCategoryDAL());
            foreach (var category in categoryManager.GetAll())
            {
                Console.WriteLine(category.CategoryName);
            }
        }

        private static void ProductTest()
        {
            //ProductManager productManager = new ProductManager(new InMemoryProductDAL());
            ProductManager productManager = new ProductManager(new EfProductDAL());

            foreach (var item in productManager.GetAllByUnitPrice(50, 100))
            {
                Console.WriteLine(item.ProductName);
            }
        }
    }
}
