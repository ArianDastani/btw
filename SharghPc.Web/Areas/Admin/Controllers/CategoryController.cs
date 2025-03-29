using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Category;
using SharghPc.DataLayer.DTOs.Category;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class CategoryController : AdminBaseController
    {
        #region Ctor

        private ICategoryServices _categoryServices;

        public CategoryController(ICategoryServices categoryServices)
        {
            _categoryServices = categoryServices;
        }

        #endregion

        #region List


        public async Task<IActionResult> Index(long? parentId)
        {
            var res = await _categoryServices.GetAllProductCategoriesByParentId(parentId);

            return View(res);
        }
        #endregion

        #region Add

        [HttpGet]
        public async Task<IActionResult> AddNewCategory(long? parentId)
        {
            ViewBag.ParentId = parentId;

            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddNewCategory(AddNewCategoryDto addNewCategoryDto)
        {
            var res = await _categoryServices.AddNewCategory(addNewCategoryDto);

            if (res)
            {
                TempData[SuccessMessage] = "با موفقیت اضافه شد";
            }
            else
            {
                TempData[WarningMessage] = "عملیات با خطا مواجه شد";
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Edit
        [HttpGet]
        public async Task<IActionResult> EditCategory(long Id)
        {
            var res = await _categoryServices.GetCategoriesForEdit(Id);
            if (res == null)
            {
                TempData[WarningMessage] = "یافت نشد";
                return RedirectToAction("Index");
            }

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoriesDto categoriesDto)
        {
            var res = await _categoryServices.EditCategories(categoriesDto);

            if (res == true)
            {
                TempData[SuccessMessage] = "با موفقیت ویرایش شد";
            }
            else
            {
                TempData[WarningMessage] = "عملیات با خطا مواجه شد";
            }

            return RedirectToAction("Index");
        }

        #endregion

        #region Remove

        public async Task<IActionResult> Delete(long id)
        {
            var res = await _categoryServices.DeleteCategory(id);

            return Json(res);
        }

        #endregion
    }
}
