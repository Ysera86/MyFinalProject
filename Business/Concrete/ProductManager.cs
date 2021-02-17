using Business.Abstract;
using Business.Contants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    //[LogAspect] --> AOP // örn. classı logla,  yeni method ekleyince ona bu attr ekledim mi eklemedm mi düşünmeye gerek kalmaz

    // Bir iş sınıfı diğer sınıfları new lemez! Injection yap!
    public class ProductManager : IProductService
    {
        IProductDAL _productDAL;

        public ProductManager(IProductDAL productDAL)
        {
            _productDAL = productDAL; // Injection > burada asla InMemory, eF vs tipler olmaz!!
        }

        //[LogAspect] --> AOP
        //[Validate]
        [ValidationAspect(typeof(ProductValidator))]
        //[RemoveCache]
        //[Transaction] // örn. hata olursa geri al
        //[Performance] // örn. çalışması 5sn geçerse uyar
        public IResult Add(Product product)
        {
            // Business  codes  
            // Validation  
            // üst 2si AYRI olmalı!

            // fluent validation ile bu doğrulamaları yapıp burdan kaldırıcaz. :  product.UnitPrice <= 0, product.ProductName.Length < 2 vs hep validation 
            // Business. add folder : ValidationRules :  ProductValidator yazdık.. Aşağıyı kapat.
            ////if (product.UnitPrice <= 0)
            ////{
            ////    return new ErrorResult(Messages.UnitPriceInvalid);
            ////}

            ////if (product.ProductName.Length < 2)
            ////{
            ////    // magic string : öyle elle metinsel standardı olmayan şeyler
            ////    // return new ErrorResult("Ürün ismi en az 2 karakter olmalıdır");
            ////    return new ErrorResult(Messages.ProductNameInvalid);
            ////}
            ///
            //var context = new ValidationContext<Product>(product);
            //ProductValidator productValidator = new ProductValidator();
            //var result = productValidator.Validate(context);
            //if (!result.IsValid)
            //{
            //    throw new ValidationException(result.Errors);
            //}
            // üsttekinin yerine
            //ValidationTool.Validate(new ProductValidator(), product); // 1e1 aynı anlam attribute artık :   [ValidationAspect(typeof(ProductValidator)]
            // Loglama
            // Cacheremove
            // performance
            // transaction
            // authorization
            //...... > bunlar iş mantığı değil.. buraya eklenirlerse de burası ÇORBA : AOP> attribute ekleyerek!!örn. method üstüne    [ValidationAspect(typeof(ProductValidator)]

            _productDAL.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        //[Cache]
        public IDataResult<List<Product>> GetAll()
        {
            // İş Kodları

            // yetkisi var mı vsvsvssv
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>(Messages.MaintenanceTime);
            }
            return new SuccessDataResult<List<Product>>(_productDAL.GetAll(), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByCategoryId(int id)
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>();
            }
            return new SuccessDataResult<List<Product>>(_productDAL.GetAll(p => p.CategoryId == id), Messages.ProductsListed);
        }

        public IDataResult<List<Product>> GetAllByUnitPrice(decimal min, decimal max)
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<Product>>();
            }
            return new SuccessDataResult<List<Product>>(_productDAL.GetAll(p => p.UnitPrice >= min && p.UnitPrice <= max), Messages.ProductListed);
        }

        public IDataResult<Product> GetById(int productId)
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<Product>();
            }
            return new SuccessDataResult<Product>(_productDAL.Get(p => p.ProductId == productId));
        }

        public IDataResult<List<ProductDetailDTO>> GetProductDetails()
        {
            if (DateTime.Now.Hour == 22)
            {
                return new ErrorDataResult<List<ProductDetailDTO>>();
            }
            return new SuccessDataResult<List<ProductDetailDTO>>(_productDAL.GetProductDetails());
        }
    }
}
