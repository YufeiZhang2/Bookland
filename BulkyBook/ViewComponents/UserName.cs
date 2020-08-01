using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.ViewComponents
{
    public class UserName: ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserName(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var currentUsername = _httpContextAccessor.HttpContext.User.Identity.Name;
            var currentUser = _unitOfWork.ApplicationUser.GetFirstOrDefault(user => user.UserName == currentUsername);
            return View(currentUser);
        }
    }
}