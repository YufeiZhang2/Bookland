using System;
using System.Linq;
using System.Threading.Tasks;
using BulkyBook.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult GetAll()
        { 
            //get all users from User table
            var userList = _unitOfWork.ApplicationUser.GetAll(includeProperties: "Company").ToList();
            //get user id and role id mapping from UserRoles table
            var userRole = _unitOfWork.GetUserRoleMapList();
            //get all roles from Roles table
            var roleList = _unitOfWork.GetAllUserRoles();
            
            //fill the associated role for each user
            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roleList.FirstOrDefault(r => r.Id == roleId).Name;
                if (user.Company == null)
                {
                    user.Company = new Company()
                    {
                        Name = ""
                    };
                }
            }
            
            return Json(new {data = userList});
        }


        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var objFromDb = _unitOfWork.Category.Get(id);
            if (objFromDb == null)
            {
                return Json(new
                    {success = false, message = "There was an error for deleting this item. Please try again later."});
            }
            _unitOfWork.Category.Remove(objFromDb);
            _unitOfWork.Save();
            return Json(new {success = true, message = "The item was deleted."});
        }
    }
}