using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BulkyBook.DataAccess.Repository
{
    public class SubssRepository : Repository<Subss>, ISubssRepository
    {
        private readonly ApplicationDbContext _db;

        public SubssRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public IEnumerable<Subss> GetSubCate(int subCateId)
        {
            var query = _db.Subries.Where(m => m.SubCategoryId == subCateId);
            return query;
        }

        public void Update(Subss subss)
        {
            var objFromDb = _db.Subries.FirstOrDefault(s => s.Id == subss.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = subss.Name;
                objFromDb.CategoryId = subss.CategoryId;
                objFromDb.SubCategoryId = subss.SubCategoryId;

            }
        }
    }
}
