using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Product;
using SharghPc.DataLayer.DTOs.Product;

namespace SharghPc.Web.Controllers
{
    public class ProductController : SiteBaseController
    {
        #region Ctorf

        private IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        #endregion

        #region Products

        [HttpGet("products")]
        [HttpGet("products/{Category}")]
        public async Task<IActionResult> FilterProduct(FilterProductDto filterProductDto)
        {
            var product = await _productServices.FilterProduct(filterProductDto);

            ViewBag.ProductCategory = await _productServices.GetAllActiveProductCategories();

            return View(product);
        }

        public async Task<IActionResult> searchProduct(string title)
        {
            var product = await _productServices.FilterProduct(new FilterProductDto() { Title = title });

            ViewBag.ProductCategory = await _productServices.GetAllActiveProductCategories();

            return View(product);
        }

        #endregion

        #region Detail
        [HttpGet("product/{id}")]
        public async Task<IActionResult> ProductDetail(long id)
        {
            if (id == 0 || id == null) return NotFound();

            var res = await _productServices.GetProductDetailById(id);

            if (res == null) return NotFound();

            return View(res);
        }

        #endregion

        #region Get product json

        [HttpGet("product-json")]
        public async Task<IActionResult> GetProductJson(string productName)
        {
            var data = await _productServices.GetProductNameForSearch(productName);

            return new JsonResult(data);

        }

        #endregion
    }
}
