using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BulkyBook.Areas.Admin.Controllers
{

    [Area("Admin")]
    [Authorize]
    public class OrderController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public OrderDetailsVM OrderVM { get; set; }
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Details(int id)
        {
            OrderVM = new OrderDetailsVM()
            {

                OrderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault
                  (u => u.Id == id, includeProperties: "ApplicationUser"),
                OrderDetails = _unitOfWork.OrderDetails.GetAll
                  (o => o.OrderId == id, includeProperties: "Product")
            };
            return View(OrderVM);
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetOrderList(string status)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            IEnumerable<OrderHeader> orderHeadersList;
            if(User.IsInRole(SD.Role_Admin) || User.IsInRole(SD.Role_Employee))
            {
                orderHeadersList = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");

            }
            else
            {
                orderHeadersList = _unitOfWork.OrderHeader.GetAll(
                    u=>u.ApplicationUserId == claim.Value,includeProperties: "ApplicationUser");

            }
            switch (status)
            {

                case "pending":
                    orderHeadersList = orderHeadersList.Where(o => o.PaymentStatus == SD.PaymentStautsDelayedPayment);
                    break;
                case "inprocess":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StautsApproved ||
                                                             o.OrderStatus == SD.StautsInProcess ||
                                                             o.OrderStatus == SD.StautsPending 
                                                             );
                    break;
                case "completed":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StautsShipped);
                    break;
                case "rejected":
                    orderHeadersList = orderHeadersList.Where(o => o.OrderStatus == SD.StautsCancelled ||
                                                             o.OrderStatus == SD.StautsRefund ||
                                                             o.OrderStatus == SD.PaymentStautsRejected
                                                             );
                    break;
                default:
                    
                    break;
            }
            return Json(new { data = orderHeadersList });
        }

        #endregion
    }
}