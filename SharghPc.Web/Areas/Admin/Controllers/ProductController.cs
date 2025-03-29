using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Product;
using SharghPc.DataLayer.DTOs.Product;
using SharghPc.Web.Models;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class ProductController : AdminBaseController
    {
        #region CtorF
        private IProductServices _productServices;

        public ProductController(IProductServices productServices)
        {
            _productServices = productServices;
        }

        #endregion

        #region List

        public async Task<IActionResult> Index(FilterProductDto filterProductDto)
        {
            var res = await _productServices.FilterProduct(filterProductDto);

            return View(res);
        }

        #endregion

        #region Add
        [HttpGet]
        public async Task<IActionResult> AddProduct()
        {
            ViewBag.Categories = await _productServices.GetAllActiveProductCategories();

            return View();
        }


        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(AddProductDto productDto, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                TempData[WarningMessage] = "تمامی موارد مورد نیاز را وارد کنید";
                ViewBag.Categories = await _productServices.GetAllActiveProductCategories();
                return View(productDto);
            }

            if (image == null)
            {
                TempData[WarningMessage] = "تصویر محصول را وارد کنید";
                ViewBag.MainCategories = await _productServices.GetAllActiveProductCategories();
                return View(productDto);
            }

            ViewBag.Categories = await _productServices.GetAllActiveProductCategories();

            var res = await _productServices.AddProduct(productDto, image);

            if (res)
            {
                TempData[SuccessMessage] = "با موفقیت اضافه شد";
                return RedirectToAction("AddProduct");
            }

            return View(productDto);
        }

        #endregion

        #region Edit

        public async Task<IActionResult> EditProduct(long productId)
        {
            var res = await _productServices.GetProductForEdit(productId);
            if (res == null) return NotFound();

            ViewBag.Categories = await _productServices.GetAllActiveProductCategories();

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(EditProductDto editProduct, IFormFile? image)
        {
            if (!ModelState.IsValid)
            {
                TempData[WarningMessage] = "تمامی موارد را وارد کنید";
                return View(editProduct);
            }

            var res = await _productServices.EditProduct(editProduct, image);

            return RedirectToAction("Index");
        }

        #endregion

        #region Remove

        public async Task<IActionResult> RemoveProduct(long Id)
        {
            var res = await _productServices.RemoveProduct(Id);

            return Json(res);
        }

        #endregion

        #region ProductFeatures

        [HttpGet]
        public async Task<IActionResult> EditProductFeatures(long productId)
        {
            var res = await _productServices.GetProductFeature(productId);

            if (res == null)
            {
                return NotFound();
            }

            AddOrEditProductFeatureViewModels productFeatureViewModels = new AddOrEditProductFeatureViewModels()
            {
                ProductFeatures = res,
                productId = productId

            };

            return View(productFeatureViewModels);
        }

        [HttpPost]
        public async Task<IActionResult> EditProductFeatures(long productId, ProductFeatureDto productFeatureDto)
        {
            var res = await _productServices.AddProductFeature(productId, productFeatureDto);

            if (res)
            {
                return RedirectToAction("EditProductFeatures", new { productId = productId });
            }

            return View();
        }


        public async Task<IActionResult> RemoveProductFeature(long Id)
        {
            var res = await _productServices.RemoveProductFeature(Id);

            return Json(res);
        }

        #endregion

        #region Inventory

        public async Task<IActionResult> EndOfInventory(long Id)
        {
            var res = await _productServices.SetToEndOfInventory(Id);

            return Json(res);
        }

        #endregion

        #region ActiveProduct

        public async Task<IActionResult> ActivatedProduct(long Id)
        {
            var res = await _productServices.ActivatedProduct(Id);
            return Json(res);
        }

        #endregion

        #region ProductSpecial

        public async Task<IActionResult> AddToSpecialProduct(long Id)
        {
            var res = await _productServices.AddToSpecialProduct(Id);
            return RedirectToAction("Index");
        }

        #endregion

        #region Gallery

        public async Task<IActionResult> GetProductGalleries(long Id)
        {
            if (Id == 0) return NotFound();

            var res = await _productServices.GetAllProductGallery(Id);
            if (res == null) return NotFound();
            ViewBag.productId = Id;
            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> AddProductImageGallery(long productId, IFormFile image)
        {
            ViewBag.productId = productId;

            var res = await _productServices.AddProductImageGallery(productId, image);

            switch (res)
            {
                case AddProductImageGalleryResult.NotFound:
                    TempData[WarningMessage] = "محصول مورد نظر یافت نشد";
                    break;
                case AddProductImageGalleryResult.Error:
                    TempData[ErrorMessage] = "خطا در ثبت رخ داد";
                    break;
                case AddProductImageGalleryResult.Success:
                    TempData[SuccessMessage] = "با موفقیت اضافه شد";
                    break;

            }

            return RedirectToAction("GetProductGalleries", "Product", new { Id = productId });
        }

        public async Task<IActionResult> RemoveProductGalleryImage(long Id)
        {
            var res = await _productServices.RemoveProductImageGallery(Id);

            return Json(res);
        }

        #endregion
    }
}
