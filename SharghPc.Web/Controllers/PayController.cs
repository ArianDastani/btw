using EndPoint.Site.Utilities;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Cart;
using SharghPc.Application.Services.Order;
using SharghPc.Application.Services.RequestPay;
using SharghPc.Application.Services.Shipment;
using SharghPc.Application.Services.user;
using SharghPc.DataLayer.DTOs.Order;
using SharghPc.Web.Models;

namespace SharghPc.Web.Controllers
{
    [Authorize(Policy = "CustomerRole")]
    public class PayController : SiteBaseController
    {
        #region Ctor

        private IRequestPayServices _requestPay;
        private ICartServices _cartServices;
        private IUserServices _userServices;
        private IShipmentServices _shipmentsServices;
        private IOrderServices _orderServices;

        public PayController(IRequestPayServices requestPay, ICartServices cartServices, IUserServices userServices, IShipmentServices shipmentsServices, IOrderServices orderServices)
        {
            _requestPay = requestPay;
            _cartServices = cartServices;
            _userServices = userServices;
            _shipmentsServices = shipmentsServices;
            _orderServices = orderServices;
        }

        #endregion

        #region info-verify

        [HttpGet("InfoVerify")]
        public async Task<IActionResult> InfoVerify()
        {
            CookiesManeger cm = new CookiesManeger();

            var userShipment = await _shipmentsServices.GetShipment(User.GetUserId());

            //if(userShipment==null)

            var cart = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            if (cart.CartItems.Count <= 0)
            {
                return Redirect("/cart");
            }

            InfoVerifyViewModel verify = new InfoVerifyViewModel()
            {
                Shipment = userShipment,
                Cart = cart
            };

            return View(verify);
        }


        #endregion
        [HttpGet]
        public async Task<IActionResult> PaymentMethods()
        {
            var userShipment = await _shipmentsServices.GetShipment(User.GetUserId());

            if (userShipment == null)
            {
                return RedirectToAction("InfoVerify");
            }

            CookiesManeger cm = new CookiesManeger();

            var cart = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            if (cart.CartItems.Count <= 0)
            {
                return Redirect("/cart");
            }

            PaymentMethodsViewModels payment = new PaymentMethodsViewModels()
            {
                Shipment = userShipment,
                Cart = cart
            };

            return View(payment);
        }
        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> PaymentMethods(IFormFile imagepay)
        {
            if (imagepay == null)
            {
                TempData[WarningMessage] = "لطفا رسید مبلغ پرداختی را وارد کنید";
                return RedirectToAction("PaymentMethods");
            }

            CookiesManeger cm = new CookiesManeger();

            var cart = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            if (cart == null || !cart.CartItems.Any())
            {
                TempData[WarningMessage] = "سبد خرید شما خالی میباشد";
                return Redirect("/cart");
            }

            int sumAmount = cart.CartItems.Sum(x => x.Price * x.Count);

            var pay = await _requestPay.AddRequestPay(sumAmount, User.GetUserId(), cm.GetBrowserId(HttpContext), imagepay);

            var order = await _orderServices.AddNewOrder(new AddNewOrderDto()
            {
                CartId = cart.Id,
                RequestpayId = pay.RequestPayId,
                UserId = User.GetUserId()
            });

            if (order)
            {
                TempData[SuccessMessage] = "سفارش با موفقیت ثبت شد";
                return Redirect("/DashBoard");
            }

            //todo: درگاه پرداخت

            return Redirect("/DashBoard");
        }

        #region RequestPay

        public async Task<IActionResult> Pay()
        {
            CookiesManeger cm = new CookiesManeger();

            var cart = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            if (cart == null || !cart.CartItems.Any())
            {
                TempData[WarningMessage] = "سبد خرید شما خالی میباشد";
                return Redirect("/cart");
            }

            int sumAmount = cart.CartItems.Sum(x => x.Price * x.Count);

            //var pay = await _requestPay.AddRequestPay(sumAmount, User.GetUserId(), cm.GetBrowserId(HttpContext),);

            //todo: درگاه پرداخت

            return RedirectToAction("", "");
        }

        #endregion


    }
}
