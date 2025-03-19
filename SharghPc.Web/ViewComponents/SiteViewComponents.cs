using EndPoint.Site.Utilities;
using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Cart;
using SharghPc.Application.Services.Product;
using SharghPc.Application.Services.Site;

namespace MarketPlace.Web.ViewComponents
{


    #region site header

    public class SiteHeaderViewComponent : ViewComponent
    {
        private readonly IProductServices _productServices;
        private readonly ISiteInfoServices _siteInfoServices;


        public SiteHeaderViewComponent(IProductServices productServices, ISiteInfoServices siteInfoServices)
        {
            _productServices = productServices;
            _siteInfoServices = siteInfoServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            ViewBag.ProductCategories = await _productServices.GetAllActiveProductCategories();
            var res = await _siteInfoServices.GetContatsDtoForSite();

            return View("SiteHeader", res);
        }
    }

    #endregion

    #region site footer

    public class SiteFooterViewComponent : ViewComponent
    {
        private readonly ISiteInfoServices _siteInfoServices;

        public SiteFooterViewComponent(ISiteInfoServices siteInfoServices)
        {
            _siteInfoServices = siteInfoServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var info = await _siteInfoServices.GetContatsDtoForSite();


            return View("SiteFooter", info);
        }
    }

    #endregion

    #region Cart

    public class GetCartViewComponent : ViewComponent
    {
        private readonly ICartServices _cartServices;

        public GetCartViewComponent(ICartServices cartServices)
        {
            _cartServices = cartServices;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            CookiesManeger cm = new CookiesManeger();
            var id = cm.GetBrowserId(HttpContext);
            var cart = await _cartServices.GetLastOpenCart(cm.GetBrowserId(HttpContext), User.GetUserId());

            return View("GetCart", cart);
        }
    }

    #endregion
}
