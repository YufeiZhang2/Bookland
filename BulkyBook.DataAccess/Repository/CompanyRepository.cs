using System.Linq;
using BulkyBook.DataAccess.Data;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public class CompanyRepository: Repository<Company>, ICompanyRepository
    {
        private readonly ApplicationDbContext _db;
        
        public CompanyRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(Company company)
        {
            var objFromDb = _db.Companies.FirstOrDefault(c => c.Id == company.Id);
            if (objFromDb != null)
            {
                objFromDb.Name = company.Name;

                if (company.StreetAddress != null) objFromDb.StreetAddress = company.StreetAddress;
                
                if (company.PostalCode != null) objFromDb.PostalCode = company.PostalCode;
                
                if (company.PhoneNumber != null) objFromDb.PhoneNumber = company.PhoneNumber;

                if (company.State != null) objFromDb.State = company.State;

                if (company.City != null) objFromDb.City = company.City;

                if (company.IsAuthorisedCompany != null) objFromDb.IsAuthorisedCompany = company.IsAuthorisedCompany;
            }
        }
    }
}