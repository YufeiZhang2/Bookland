using System;
using System.Collections.Generic;
using System.Linq;
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
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace BulkyBook.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IOptions<StripeSettings> _stripe;

        public ShoppingCartViewModel ShoppingCartViewModel { get; set; }
        


        public ShoppingCartController(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,
            IEmailSender emailSender, IHttpContextAccessor httpContextAccessor, IOptions<StripeSettings> stripe)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _emailSender = emailSender;
            _httpContextAccessor = httpContextAccessor;
            _stripe = stripe;
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
                            .GetAll(cart => cart.ApplicationUserId == currentUser.Id, null, "Product"),
                    OrderHeader = new OrderHeader()
                };

                shoppingCartViewModel.OrderHeader.OrderTotal = 0;
                shoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(user => user.Id == currentUser.Id, "Company");


                foreach (var productInCart in shoppingCartViewModel.ListOfProductsInCart)
                {
                    //get the price based on quantity
                    productInCart.Price = GlobalUti.GetPriceBasedOnQuantity(productInCart.Count,
                        productInCart.Product.Price,
                        productInCart.Product.Price50, productInCart.Product.Price100);
                    //calculate the total price of this product and add it to order total
                    shoppingCartViewModel.OrderHeader.OrderTotal += productInCart.Price * productInCart.Count;

                    //Convert the product description to HTML and cut it within 100 characters
                    productInCart.Product.Description =
                        GlobalUti.ConvertRawHtmlToPlainText(productInCart.Product.Description);
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

        public IActionResult Summary()
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
                            .GetAll(cart => cart.ApplicationUserId == currentUser.Id, null, "Product"),
                    OrderHeader = new OrderHeader()
                };

                shoppingCartViewModel.OrderHeader.ApplicationUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(user => user.Id == currentUser.Id, "Company");

                foreach (var productInCart in shoppingCartViewModel.ListOfProductsInCart)
                {
                    //get the price based on quantity
                    productInCart.Price = GlobalUti.GetPriceBasedOnQuantity(productInCart.Count,
                        productInCart.Product.Price,
                        productInCart.Product.Price50, productInCart.Product.Price100);
                    //calculate the total price of this product and add it to order total
                    shoppingCartViewModel.OrderHeader.OrderTotal += Math.Round(productInCart.Price * productInCart.Count, 2);
                }

                shoppingCartViewModel.OrderHeader.Name = shoppingCartViewModel.OrderHeader.ApplicationUser.Name;
                shoppingCartViewModel.OrderHeader.PhoneNumber =
                    shoppingCartViewModel.OrderHeader.ApplicationUser.PhoneNumber;
                shoppingCartViewModel.OrderHeader.StreetAddress =
                    shoppingCartViewModel.OrderHeader.ApplicationUser.StreetAddress;
                shoppingCartViewModel.OrderHeader.City = shoppingCartViewModel.OrderHeader.ApplicationUser.City;
                shoppingCartViewModel.OrderHeader.State = shoppingCartViewModel.OrderHeader.ApplicationUser.State;
                shoppingCartViewModel.OrderHeader.PostalCode =
                    shoppingCartViewModel.OrderHeader.ApplicationUser.PostalCode;
                return View(shoppingCartViewModel);
            }

            return View();
        }

        [HttpPost]
        [ActionName("Summary")]
        [ValidateAntiForgeryToken]
        public IActionResult SummaryPost(string stripeToken, ShoppingCartViewModel shoppingCartVMFromView)
        {
            var currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
            //if the user is already logged in
            if (currentUserName != null)
            {
                var currentUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == currentUserName, "Company");

                var shoppingCartViewModel = new ShoppingCartViewModel()
                {
                    ListOfProductsInCart =
                        _unitOfWork.ShoppingCart
                            .GetAll(cart => cart.ApplicationUserId == currentUser.Id, null, "Product"),
                    OrderHeader = new OrderHeader()
                };
                shoppingCartViewModel.OrderHeader.ApplicationUser = currentUser;
                shoppingCartViewModel.OrderHeader.PaymentStatus = GlobalUti.PaymentStatusPending;
                shoppingCartViewModel.OrderHeader.OrderStatus = GlobalUti.StatusPending;
                shoppingCartViewModel.OrderHeader.ApplicationUserId = currentUser.Id;
                shoppingCartViewModel.OrderHeader.OrderDate = DateTime.Now;

                shoppingCartViewModel.OrderHeader.Name = shoppingCartVMFromView.OrderHeader.Name;
                shoppingCartViewModel.OrderHeader.PhoneNumber = shoppingCartVMFromView.OrderHeader.PhoneNumber;
                shoppingCartViewModel.OrderHeader.StreetAddress = shoppingCartVMFromView.OrderHeader.StreetAddress;
                shoppingCartViewModel.OrderHeader.City = shoppingCartVMFromView.OrderHeader.City;
                shoppingCartViewModel.OrderHeader.State = shoppingCartVMFromView.OrderHeader.State;
                shoppingCartViewModel.OrderHeader.PostalCode = shoppingCartVMFromView.OrderHeader.PostalCode;
                _unitOfWork.OrderHeader.Add(shoppingCartViewModel.OrderHeader);
                _unitOfWork.Save();

                foreach (var productInCart in shoppingCartViewModel.ListOfProductsInCart)
                {
                    productInCart.Price = GlobalUti.GetPriceBasedOnQuantity(productInCart.Count,
                        productInCart.Product.Price, productInCart.Product.Price50, productInCart.Product.Price100);
                    var orderDetail = new OrderDetail()
                    {
                        ProductId = productInCart.Product.Id,
                        OrderHeaderId = shoppingCartViewModel.OrderHeader.Id,
                        Price = productInCart.Price,    
                        Count = productInCart.Count
                    };
                    shoppingCartViewModel.OrderHeader.OrderTotal = Math.Round(orderDetail.Price * orderDetail.Count, 2);
                    _unitOfWork.OrderDetail.Add(orderDetail);
                }

                _unitOfWork.ShoppingCart.RemoveRange(shoppingCartViewModel.ListOfProductsInCart);
                _httpContextAccessor.HttpContext.Session.SetString(GlobalUti.ShoppingCartSession, "0");
                
                if (stripeToken != null)
                {
                    // var options = new ChargeCreateOptions()
                    // {
                    //     Amount = Convert.ToInt32(shoppingCartViewModel.OrderHeader.OrderTotal * 100),
                    //     Currency = "aud",
                    //     Description = "Order ID: " + shoppingCartViewModel.OrderHeader.Id,
                    //     Source = stripeToken
                    // };
                    //
                    // var service = new ChargeService();
                    // var charge = service.Create(options);
                    //
                    // if (charge.BalanceTransactionId == null)
                    //     shoppingCartViewModel.OrderHeader.PaymentStatus = GlobalUti.PaymentStatusRejected;
                    // else
                    // {
                    shoppingCartViewModel.OrderHeader.PaymentStatus = GlobalUti.PaymentStatusApproved;
                    shoppingCartViewModel.OrderHeader.TransactionId = "HardCodeBalanceTransactionId";
                    // shoppingCartViewModel.OrderHeader.TransactionId = charge.BalanceTransactionId;
                    // }
                    //
                    // if (charge.Status.ToLower() == "succeeded")
                    // {
                    shoppingCartViewModel.OrderHeader.OrderStatus = GlobalUti.StatusApproved;
                    shoppingCartViewModel.OrderHeader.PaymentDate = DateTime.Now;
                    // }
                }
                else
                {    //order will be created for authorised company
                    shoppingCartViewModel.OrderHeader.PaymentDueDate = DateTime.Now.AddDays(30);
                    shoppingCartViewModel.OrderHeader.PaymentStatus = GlobalUti.PaymentStatusDelayedPayment;
                    shoppingCartViewModel.OrderHeader.OrderStatus = GlobalUti.StatusApproved;
                }
                
                _unitOfWork.Save();
                return RedirectToAction("OrderConfirmation", "ShoppingCart",
                    new {id = shoppingCartViewModel.OrderHeader.Id});
            }
            //if user is not logged in
            return RedirectToAction("OrderConfirmation", "ShoppingCart");
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
                    values: new {area = "Identity", userId = currentUser.Id, code = code},
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

        [HttpPost]
        public IActionResult UpdateQuantity(int cartProdId, ShoppingCartViewModel shoppingCartViewModel)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId,
                includeProperties: "Product");
            prodInCart.Count = shoppingCartViewModel.NewQuantity;
            prodInCart.Price = GlobalUti.GetPriceBasedOnQuantity(prodInCart.Count, prodInCart.Product.Price,
                prodInCart.Product.Price50, prodInCart.Product.Price100);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Plus(int cartProdId)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId,
                includeProperties: "Product");
            prodInCart.Count += 1;
            prodInCart.Price = GlobalUti.GetPriceBasedOnQuantity(prodInCart.Count, prodInCart.Product.Price,
                prodInCart.Product.Price50, prodInCart.Product.Price100);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Minus(int cartProdId)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId,
                includeProperties: "Product");
            if (prodInCart.Count == 1)
            {
                _unitOfWork.ShoppingCart.Remove(prodInCart);
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            prodInCart.Count -= 1;
            prodInCart.Price = GlobalUti.GetPriceBasedOnQuantity(prodInCart.Count, prodInCart.Product.Price,
                prodInCart.Product.Price50, prodInCart.Product.Price100);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Trash(int cartProdId)
        {
            var prodInCart = _unitOfWork.ShoppingCart.GetFirstOrDefault(cartProd => cartProd.Id == cartProdId,
                includeProperties: "Product");
            _unitOfWork.ShoppingCart.Remove(prodInCart);
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult OrderConfirmation(int id)
        {
            return View(id);
        }
    }
}