using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        
        public ShoppingCartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager, IEmailSender emailSender, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public IActionResult Index()
        {
            var currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //if the user is already logged in
            if (currentUserName != null)
            {
                var currentUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == currentUserName);
                
                var shoppingCartViewModel = new ShoppingCartViewModel()
                {
                    ListOfProductsInCart =
                        _unitOfWork.ShoppingCart
                            .GetAll(cart => cart.ApplicationUserId == currentUser.Id,null, "Product"),
                    OrderHeader = new OrderHeader()
                };
                
                shoppingCartViewModel.OrderHeader.OrderTotal = 0;
                shoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(user => user.Id == currentUser.Id, "Company");
            

                foreach (var productInCart in shoppingCartViewModel.ListOfProductsInCart)
                {
                    //get the price based on quantity
                    productInCart.Price = GlobalUti.GetPriceBasedOnQuantity(productInCart.Count, productInCart.Product.Price,
                        productInCart.Product.Price50, productInCart.Product.Price100);
                    //calculate the total price of this product and add it to order total
                    shoppingCartViewModel.OrderHeader.OrderTotal += productInCart.Price * productInCart.Count;
                    
                    //Convert the product description to HTML and cut it within 100 characters
                    productInCart.Product.Description = GlobalUti.ConvertRawHtmlToPlainText(productInCart.Product.Description);
                    if (productInCart.Product.Description.Length > 100)
                    {
                        productInCart.Product.Description = productInCart.Product.Description.Substring(0, 99) + "...";
                    }
                }
                return View(shoppingCartViewModel);
            }
            //if the user is not logged in
            return View();
        }

        public IActionResult Summary(ShoppingCart shoppingCart)
        {
            // return View();
            return null;
        }

        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexPost()
        {
            var currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //if the user is already logged in
            if (currentUserName != null)
            {
                var currentUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == currentUserName);
                
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(currentUser);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { area = "Identity", userId = currentUser.Id, code = code},
                    protocol: Request.Scheme);
                
                await _emailSender.SendEmailAsync(currentUser.Email, "Confirm your email",
                    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Verification Email is empty.");
            }
            ModelState.AddModelError(string.Empty, "Confirmation email is sent. Please check your email.");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Plus(int cartProdId)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId, includeProperties:"Product");
            prodInCart.Count += 1;
            prodInCart.Price = GlobalUti.GetPriceBasedOnQuantity(prodInCart.Count, prodInCart.Product.Price,
                prodInCart.Product.Price50, prodInCart.Product.Price100);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Minus(int cartProdId)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId, includeProperties:"Product");
            prodInCart.Count -= 1;
            prodInCart.Price = GlobalUti.GetPriceBasedOnQuantity(prodInCart.Count, prodInCart.Product.Price,
                prodInCart.Product.Price50, prodInCart.Product.Price100);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Trash(int cartProdId)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId, includeProperties:"Product");
            _unitOfWork.ShoppingCart.Remove(prodInCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}