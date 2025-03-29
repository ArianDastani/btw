using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Index;
using SharghPc.DataLayer.DTOs.Index;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class IndexController : AdminBaseController
    {
        #region ctor

        private IIndexServices _indexServices;

        public IndexController(IIndexServices indexServices)
        {
            _indexServices = indexServices;
        }

        #endregion

        #region Category

        #region List

        public async Task<IActionResult> IndesCategoryAreas()
        {
            var res = await _indexServices.GetIndexCategoryArea();

            return View(res);
        }

        #endregion

        #region Add


        [HttpGet]
        public async Task<IActionResult> AddCategoryAreas()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategoryAreas(AddIndexCategoryAreaDto categoryArea, IFormFile image)
        {
            if (!ModelState.IsValid)
            {
                TempData[WarningMessage] = "تمامی موارد خاسته شده را وارد نمایید";
                return View(categoryArea);
            }

            var res = await _indexServices.AddIndexCategoryArea(categoryArea, image);

            if (res)
            {
                TempData[SuccessMessage] = "با موفقیت اضافه شد";
            }
            else
            {
                TempData[WarningMessage] = "خطا در ثبت اطلاعات";
            }

            return RedirectToAction("AddCategoryAreas");
        }

        #endregion

        #region Edit

        [HttpGet]
        public async Task<IActionResult> EditCategoryAreas(long Id)
        {
            var res = await _indexServices.GetIndexCategoryArea(Id);

            if (res == null)
            {
                return RedirectToAction("IndesCategoryAreas");
            }

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategoryAreas(EditIndexCategoryAreaDto categoryAreaDto, IFormFile image)
        {
            var res = await _indexServices.EditCategoryAreas(categoryAreaDto, image);

            if (res)
            {
                TempData[SuccessMessage] = "با موفقیت ویرایش شد";
            }
            else
            {
                TempData[WarningMessage] = "عملیات با خطا مواجه شد";
            }

            return RedirectToAction("IndesCategoryAreas");
        }

        #endregion

        #region Remove

        public async Task<IActionResult> RemoveCategoryArea(long Id)
        {
            var res = await _indexServices.RemoveIndexCategoryArea(Id);

            return Json(res);
        }


        #endregion

        #endregion
    }
}
