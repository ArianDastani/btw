using EndPoint.Site.Utilities;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Cart;
using SharghPc.DataLayer.DTOs.Cart;
using SharghPc.Web.PresentationExtensions;

namespace SharghPc.Web.Controllers
{
    public class CartController : Controller
    {
        #region ctor

        private ICartServices _cartServices;

        public CartController(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }

        #endregion

        #region List
        [HttpGet("Cart")]
        public async Task<IActionResult> Index()
        {
            CookiesManeger cm = new CookiesManeger();

            var res = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            return View(res);
        }

        [HttpGet("CartPartial")]
        public async Task<IActionResult> GetCartPartial()
        {
            CookiesManeger cm = new CookiesManeger();

            var res = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            return PartialView(res);
        }

        #endregion

        #region AddtoCart

        public async Task<IActionResult> AddToCart(AddProductToCartDto cartDto)
        {
            CookiesManeger cm = new CookiesManeger();

            var res = await _cartServices.AddToCart(cartDto.ProductId, cm.GetBrowserId(HttpContext), User.GetUserId());

            switch (res)
            {
                case AddToCartResult.success:

                    return JsonResponseStatus.StatusResult(JsonResponseStatusType.Success,
                        "محصول مورد نظر با موفقیت به سبد خرید اضافه شد", null);
                    break;
                case AddToCartResult.error:

                    return JsonResponseStatus.StatusResult(JsonResponseStatusType.Error,
                        "محصول مورد نظر با موفقیت به سبد خرید اضافه شد", null);
                    break;
                case AddToCartResult.OutOfStock:

                    return JsonResponseStatus.StatusResult(JsonResponseStatusType.Warning,
                        "محصول مورد نظر با موفقیت به سبد خرید اضافه شد", null);
                    break;
            }

            return Redirect("/");
        }

        #endregion

        #region DeleteCart

        public async Task<IActionResult> RemoveProductFromCart(long Id)
        {
            if (Id == 0 || Id == null)
            {
                return Redirect("/");
            }

            CookiesManeger cm = new CookiesManeger();

            var res = await _cartServices.RemoveCartItemFromCart(Id, cm.GetBrowserId(HttpContext),
                User.GetUserId());

            if (res)
            {
                return JsonResponseStatus
                    .StatusResult(JsonResponseStatusType.Success, "محصول با موفقیت از سبد شما حفظ شد", null);
            }
            return JsonResponseStatus
                .StatusResult(JsonResponseStatusType.Error, "عملیات با خطا مواجه شد", null);

        }

        #endregion



        public async Task<IActionResult> AddCount(int Id)
        {
            var res = await _cartServices.AddCount(Id);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> LowOffCount(int Id)
        {
            var res = await _cartServices.LowOffCount(Id);

            return RedirectToAction("Index");
        }
    }
}
