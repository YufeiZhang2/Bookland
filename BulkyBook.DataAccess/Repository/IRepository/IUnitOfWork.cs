using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork: IDisposable
    {
        ICategoryRepository Category { get;}
        
        ICoverTypeRepository CoverType { get; }
        
        ICompanyRepository Company { get; }
        
        IProductRepository Product { get; }
        
        IApplicationUserRepository ApplicationUser { get; }

        public List<IdentityUserRole<string>> GetUserRoleMapList();
        
        public List<IdentityRole> GetAllUserRoles();

        void Dispose();
        
        void Save();
        
        
    }
}