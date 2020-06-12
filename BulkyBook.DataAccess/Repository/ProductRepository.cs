using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;

        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public Product GetProductWithImgOnId(int Id)
        {
          var query= _db.Products.Where(m=>m.Id==Id)
                .Include("Category")
                .Include("SubCategory")
                .Include("Subss")
                .Include(m => m.ImgDetails).FirstOrDefault();
            return (Product)query;
        }

        public void Update(Product product)
        {
            var objFromDb = _db.Products.Where(s => s.Id == product.Id)
                .Include(s=>s.ImgDetails).FirstOrDefault();
            if (objFromDb != null)
            {
                if (product.ImageUrl != null)
                {
                    objFromDb.ImageUrl = product.ImageUrl;
                }
                objFromDb.ISBN = product.ISBN;
                objFromDb.Price = product.Price;
                objFromDb.Price50 = product.Price50;
                objFromDb.ListPrice = product.ListPrice;
                objFromDb.Price100 = product.Price100;
                objFromDb.Title = product.Title;
                objFromDb.Name = product.Name;
                objFromDb.Description = product.Description;
                objFromDb.Author = product.Author;
                objFromDb.CategoryId = product.CategoryId;
                objFromDb.SubCategoryId = product.SubCategoryId;
                objFromDb.SubSubId = product.SubSubId;
                objFromDb.ImgDetails = product.ImgDetails;
            }
        }
    }
}
