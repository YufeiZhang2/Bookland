using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BulkyBook.Areas.Customer.Controllers;
using BulkyBook.DataAccess.Migrations;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace BulkyBook.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OrderController: Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;
        
        public OrderController(IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        //pass order header id
        public IActionResult Details(int id)
        {
            var orderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = _unitOfWork.OrderHeader
                    .GetFirstOrDefault(orderHeader => 
                        orderHeader.Id == id, includeProperties:"ApplicationUser"),
                OrderDetailList = _unitOfWork.OrderDetail
                    .GetAll(orderDetails => orderDetails.OrderHeaderId == id, includeProperties:"Product")
            };
            return View(orderDetailsViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Details")]
        public IActionResult Details(string stripToken, OrderDetailsViewModel orderDetailsViewModel)
        {
            var orderHeader =
                _unitOfWork.OrderHeader.GetFirstOrDefault(order => order.Id == orderDetailsViewModel.OrderHeader.Id);
            
            // var options = new ChargeCreateOptions()
            // {
            //     Amount = Convert.ToInt32(orderHeader.OrderTotal * 100),
            //     Currency = "aud",
            //     Description = "Order ID: " + orderHeader.Id,
            //     Source = stripeToken
            // };
            //
            // var service = new ChargeService();
            // var charge = service.Create(options);
            //
            // if (charge.BalanceTransactionId == null)
            //     orderHeader.PaymentStatus = GlobalUti.PaymentStatusRejected;
            // else
            // {
            orderHeader.TransactionId = "HardCodeBalanceTransactionId";
            // orderHeader.TransactionId = charge.BalanceTransactionId;
            // }
            //
            // if (charge.Status.ToLower() == "succeeded")
            // {
            orderHeader.PaymentStatus = GlobalUti.PaymentStatusApproved;
            orderHeader.PaymentDate = DateTime.Now;
            // }
            
            _unitOfWork.Save();
            return RedirectToAction("Details", "Order", new {id = orderHeader.Id});
        }

        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var currentUserName = _httpContextAccessor.HttpContext.User.Identity.Name;
            IEnumerable<OrderHeader> orderHeaderList;
            
            //if the user is already logged in
            if (currentUserName != null)
            {
                var currentUser = _unitOfWork.ApplicationUser
                    .GetFirstOrDefault(u => u.UserName == currentUserName);
                
                if (User.IsInRole(GlobalUti.Role_Admin) || User.IsInRole(GlobalUti.Role_Employee))
                {
                    orderHeaderList =
                        _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");
                }
                else
                {
                    orderHeaderList =
                        _unitOfWork.OrderHeader
                            .GetAll(orderHeader => orderHeader.ApplicationUser.Id == currentUser.Id
                                ,includeProperties: "ApplicationUser");
                }
                
                //filter orders list by status
                switch (status)
                {
                    case  "pending":
                        orderHeaderList = orderHeaderList.Where(orderHeaders =>
                            orderHeaders.PaymentStatus == GlobalUti.PaymentStatusDelayedPayment);
                        break;
                    case  "inprocess":
                        orderHeaderList = orderHeaderList.Where(orderHeaders =>
                            orderHeaders.OrderStatus == GlobalUti.StatusApproved
                            || orderHeaders.OrderStatus == GlobalUti.StatusPending
                                || orderHeaders.OrderStatus == GlobalUti.StatusPending);
                        break;
                    case  "completed":
                        orderHeaderList = orderHeaderList.Where(orderHeaders =>
                            orderHeaders.OrderStatus == GlobalUti.StatusShipped);
                        break;
                    case  "rejected":
                        orderHeaderList = orderHeaderList.Where(orderHeaders =>
                            orderHeaders.OrderStatus == GlobalUti.StatusCancelled
                            || orderHeaders.OrderStatus == GlobalUti.StatusRefunded
                            || orderHeaders.OrderStatus == GlobalUti.PaymentStatusRejected);
                        break;
                }
                
                //return the orders according to the status
                return Json(new {data = orderHeaderList});
            }
            return RedirectToAction(nameof(Index), "Home");
        }
        
        [HttpGet]
        [Authorize(Roles = GlobalUti.Role_Admin+","+GlobalUti.Role_Employee)]
        public IActionResult StartProcessing(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(order => order.Id == id);
            orderHeader.OrderStatus = GlobalUti.StatusInProcess;
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpPost]
        [Authorize(Roles = GlobalUti.Role_Admin+","+GlobalUti.Role_Employee)]
        public IActionResult ShipOrder(OrderDetailsViewModel orderDetailsViewModel)
        {
            var orderHeader = _unitOfWork.OrderHeader
                .GetFirstOrDefault(order => order.Id == orderDetailsViewModel.OrderHeader.Id);
            orderHeader.TrackingNumber = orderDetailsViewModel.OrderHeader.TrackingNumber;
            orderHeader.Carrier = orderDetailsViewModel.OrderHeader.Carrier;
            orderHeader.OrderStatus = GlobalUti.StatusShipped;
            orderHeader.ShippingDate = DateTime.Now;
            
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
        
        [HttpGet]
        [Authorize(Roles = GlobalUti.Role_Admin+","+GlobalUti.Role_Employee)]
        public IActionResult CancelOrder(int id)
        {
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(order => order.Id == id);
            if (orderHeader.PaymentStatus == GlobalUti.StatusApproved)
            {
                // var options = new RefundCreateOptions()
                // {
                //     Amount = Convert.ToInt32(orderHeader.OrderTotal*100),
                //     Reason = RefundReasons.RequestedByCustomer,
                //     Charge = orderHeader.TransactionId
                // };
                // var service = new RefundService();
                // var refund = service.Create(options);

                orderHeader.OrderStatus = GlobalUti.StatusRefunded;
                orderHeader.PaymentStatus = GlobalUti.StatusRefunded;
            }
            else
            {
                orderHeader.OrderStatus = GlobalUti.StatusCancelled;
                orderHeader.PaymentStatus = GlobalUti.StatusCancelled;
            }
            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }
    }
}