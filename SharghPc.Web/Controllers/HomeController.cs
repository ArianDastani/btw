using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata;
using SharghPc.Application.Services;
using SharghPc.Application.Services.Index;
using SharghPc.Application.Services.Product;
using SharghPc.Application.Services.Site;
using SharghPc.Web.Models;

namespace SharghPc.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        private ISiteInfoServices _siteInfoServices;
        private IIndexServices _indexServices;
        private IProductServices _productServices;
        private Itestservices _testServices;

        public HomeController(ISiteInfoServices siteInfoServices, IIndexServices indexServices, IProductServices productServices, Itestservices testServices)
        {
	        _siteInfoServices = siteInfoServices;
	        _indexServices = indexServices;
	        _productServices = productServices;
	        _testServices = testServices;
        }
        public async Task<IActionResult> Index()
        {
            IndexViewModel vm = new IndexViewModel()
            {
                ProductsForIndex = await _productServices.GetAllProductForIndexDto(),
                ProductsStorageForIndex = await _productServices.GetProductForIndexDto("storage"),
                ProductsCableForIndex = await _productServices.GetProductForIndexDto("Cables-and-conversions"),
                CategoryAreas = await _indexServices.GetIndexCategoryArea(),
                SpecialProduct = await _productServices.GetSpecialProduct()
            };

             //_testServices.Get();

            return Redirect("/admin");
        }


        [Route("about-us")]
        public async Task<IActionResult> AboutUs()
        {
            //var res = await _siteInfoServices.GetSiteInfoAsync();
            //return View(res);
            return View();
        }

        [Route("contact-us")]
        public async Task<IActionResult> ContactUs()
        {
            //var res = await _siteInfoServices.GetSiteInfoAsync();
            //return View(res);
            return View();
        }


        [HttpGet("terms-and-condition")]
        public IActionResult TermsAndCondition()
        {
            return View();
        }

        [HttpGet("404")]
        public IActionResult View404()
        {
            return View();
        }
    }
}