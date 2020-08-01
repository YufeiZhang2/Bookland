using System;
using System.Linq;
using BulkyBook.DataAccess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Initialiser
{
    public class DBInitialiser: IDbInitialiser
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DBInitialiser(ApplicationDbContext db, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public void Initialise()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0) _db.Database.Migrate();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            if (_db.Roles.Any(role => role.Name == GlobalUti.Role_Admin)) return;
            
            //Create seed user data and make it synchronously
            _roleManager.CreateAsync(new IdentityRole(GlobalUti.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(GlobalUti.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(GlobalUti.Role_User_Individual)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(GlobalUti.Role_User_Company)).GetAwaiter().GetResult();

            _userManager.CreateAsync(new ApplicationUser
            {
                UserName = "yufei.z222+admin@gmail.com",
                Email = "yufei.z222+admin@gmail.com",
                EmailConfirmed = true,
                Name = "Ivan Zhang"
            }, "Admin123*").GetAwaiter().GetResult();

            ApplicationUser user = _db.ApplicationUsers.FirstOrDefault(user => user.Email == "yufei.z222+admin@gmail.com");

            _userManager.AddToRoleAsync(user, GlobalUti.Role_Admin).GetAwaiter().GetResult();
        }
    }
}