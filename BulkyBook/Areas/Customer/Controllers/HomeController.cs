using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor
            ,IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productList = _unitOfWork.Product.GetAll(includeProperties: "Category,CoverType");
            
            var currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //if the user is already logged in
            if (currentUserName != null)
            {
                var currentUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == currentUserName);
                //get user's cart and store the count in session
                var allCarts = _unitOfWork.ShoppingCart
                    .GetAll(cart => cart.ApplicationUserId == currentUser.Id);
                var count = 0;
                foreach (var cart in allCarts)
                {
                    count += cart.Count;
                }
                _httpContextAccessor.HttpContext.Session
                    .SetString(Utility.GlobalUti.ShoppingCartSession, count.ToString());
            }

            return View(productList);
        }
        
        public IActionResult Details(int id)
        {
            var productFromDb = _unitOfWork.Product.GetFirstOrDefault(prod => prod.Id == id, "CoverType,Category");
            var cartObj = new ShoppingCart()
            {
                Product = productFromDb,
                ProductId = id
            };
            return View(cartObj);
        }
        

        public IActionResult Privacy()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            if (ModelState.IsValid)
            {
                //prevent inserting primary key error in db
                shoppingCart.Id = 0;
                var currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
                //if the user is already logged in
                if (currentUserName != null)
                {
                    var currentUser = _unitOfWork.ApplicationUser
                        .GetFirstOrDefault(u => u.UserName == currentUserName);
                    shoppingCart.ApplicationUserId = currentUser.Id;

                    var existingCartObj = _unitOfWork.ShoppingCart
                        .GetFirstOrDefault(cart => cart.ApplicationUserId == shoppingCart.ApplicationUserId
                                                   && cart.ProductId == shoppingCart.ProductId
                                                    ,"Product");
                    //update the existing shopping cart
                    if (existingCartObj != null)
                    {
                        existingCartObj.Count += shoppingCart.Count;
                    }
                    //update the new shopping cart
                    else
                    {
                        _unitOfWork.ShoppingCart.Add(shoppingCart);
                    }
                    _unitOfWork.Save();
                    
                    //get user's cart and store the count in session
                    var allCarts = _unitOfWork.ShoppingCart
                        .GetAll(cart => cart.ApplicationUserId == currentUser.Id);
                    var count = 0;
                    foreach (var cart in allCarts)
                    {
                        count += cart.Count;
                    }
                    _httpContextAccessor.HttpContext.Session
                        .SetString(Utility.GlobalUti.ShoppingCartSession, count.ToString());
                    // var cartFromSession = _httpContextAccessor.HttpContext.Session
                    //     .GetObject<ShoppingCart>(GlobalVar.ShoppingCartSession);
                    RedirectToAction(nameof(Index));
                }
                RedirectToAction(nameof(Error));
            }
            //if the form fields are not valid, return the shopping cart    
                var productFromDb = _unitOfWork.Product
                    .GetFirstOrDefault(prod => prod.Id == shoppingCart.ProductId
                        ,"CoverType,Category");
                var cartObj = new ShoppingCart()
                {
                    Product = productFromDb,
                    ProductId = productFromDb.Id
                };
                return View(cartObj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}