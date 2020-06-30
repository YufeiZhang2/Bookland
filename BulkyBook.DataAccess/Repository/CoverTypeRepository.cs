using System.Linq;
using BulkyBook.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public class CoverTypeRepository:  Repository<CoverType>, ICoverTypeRepository
    {
        private readonly ApplicationDbContext _db;
        
        public CoverTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(CoverType coverType)
        {
            var objFromDb = _db.CoverTypes.FirstOrDefault(ele => ele.Id == coverType.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = coverType.Name;
            }
        }
    }
}