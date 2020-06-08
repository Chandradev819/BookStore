using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class SubCategoryRepository : Repository<SubCategory>, ISubCategoryRepository
    {
        private readonly ApplicationDbContext _db;

        public SubCategoryRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<SubCategory> GetAll(int catId)
        {
            var query = _db.SubCategories.Where(m => m.CategoryId == catId);
            return query.ToList();
        }

        public void Update(SubCategory subcategory)
        {
            var objFromDb = _db.SubCategories.FirstOrDefault(s => s.Id == subcategory.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = subcategory.Name;
                objFromDb.CategoryId = subcategory.CategoryId;
            }
        }
    }
}
