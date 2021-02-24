using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Contants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistemde bakım yapılıyor";
        public static string ProductsListed = "Ürünler listelendi";
        public static string ProductListed = "Ürün listelendi";
        public static string ProductCountOfcategoryError = "Bir kategoride en fazla 10 ürün olabilir!";
        public static string ProductNameAlreadyExists = "Bu isimde bir ürün zaten var!";

        public static string UnitPriceInvalid = "Birim fiyat geçersiz";

        public static string Updated = "Ürün güncellendi";
        internal static string CategoryCountExceeds = "Kategori sayısı aşımı.";
    }
}
