using BulkyBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ISubssRepository : IRepository<Subss>
    {
        void Update(Subss subss);
        IEnumerable<Subss> GetSubCate(int subCateId);
    }
}
