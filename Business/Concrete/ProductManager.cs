using Business.Abstract;
using Business.BusinessAspects.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspects.Autofac.Validation;
using Core.Business;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.InMemory;
using Entities.Concrete;
using Entities.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    //[LogAspect] --> AOP // örn. classı logla,  yeni method ekleyince ona bu attr ekledim mi eklemedm mi düşünmeye gerek kalmaz

    // Bir iş sınıfı diğer sınıfları new lemez! Injection yap!
    public class ProductManager : IProductService
    {
        IProductDAL _productDAL;
        ICategoryService _categoryService;

       
        public ProductManager(IProductDAL productDAL, ICategoryService categoryService)
        {
            _productDAL = productDAL;  // Injection > burada asla InMemory, eF vs tipler olmaz!!
            _categoryService = categoryService;
        }


        //Claim
        [SecuredOperation("product.add,admin")] // product.add ve ya admin yetkilerine sahip olanlar 
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

            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId), CheckIfProductNameExists(product.ProductName));
            if (result != null)
            {
                return result;
            }
            _productDAL.Add(product);

            return new SuccessResult(Messages.ProductAdded);
        }

        [ValidationAspect(typeof(ProductValidator))]
        public IResult Update(Product product)
        {
            // kategori sayısı 15i aştıysa ürün eklenemesin
            IResult result = BusinessRules.Run(CheckIfProductCountOfCategoryCorrect(product.CategoryId), 
                CheckIfProductNameExists(product.ProductName),
                CheckIfCategoryCountExceeds());
            if (result != null)
            {
                return result;
            }
            _productDAL.Update(product);

            return new SuccessResult(Messages.Updated);
        }

        private IResult CheckIfProductCountOfCategoryCorrect(int categoryId)
        {
            var productsByCategory = _productDAL.GetAll(p => p.CategoryId == categoryId);
            if (productsByCategory.Count >= 10)
            {
                return new ErrorResult(Messages.ProductCountOfcategoryError);
            }
            return new SuccessResult();
        }

        private IResult CheckIfProductNameExists(string productName)
        {
            var result = _productDAL.GetAll(p => p.ProductName == productName).Any();
            if (result)
            {
                return new ErrorResult(Messages.ProductNameAlreadyExists);
            }
            return new SuccessResult();
        }

        private IResult CheckIfCategoryCountExceeds()
        {
            var result = _categoryService.GetAll().Data.Count;
            if (result >= 15)
            {
                return new ErrorResult(Messages.CategoryCountExceeds);
            }
            return new SuccessResult();
        }

        //[Cache]
        public IDataResult<List<Product>> GetAll()
        {
            // İş Kodları

            // yetkisi var mı vsvsvssv
            if (DateTime.Now.Hour == 15)
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
