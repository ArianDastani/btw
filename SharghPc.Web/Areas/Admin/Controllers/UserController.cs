using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.user;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class UserController : AdminBaseController
    {
        #region Ctor

        private IUserServices _userServices;

        public UserController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        #endregion

        #region List

        public async Task<IActionResult> Index()
        {
            var res = await _userServices.GetAllUserForAdmin();

            return View(res);
        }

        #endregion

        #region AddUser

        public async Task<IActionResult> LoadUser(long UserId)
        {
            var user = await _userServices.LoadUserForAdmin(UserId);

            return View(user);
        }

        public async Task<IActionResult> AddOrEditUser(AddOrEditUserDto userDto)
        {
            var res = await _userServices.AddOrEditUserForAdmin(userDto);

            if (res)
            {
                TempData[SuccessMessage] = "با موفقیت انجام شد";
                return RedirectToAction("Index");
            }

            TempData[WarningMessage] = "عملیات با خطا مواجه شد";
            return RedirectToAction("Index");
        }

        #endregion

        #region Remove

        public async Task<IActionResult> RemoveUser(long Id)
        {
            var res = await _userServices.RemoveUser(Id);

            return Json(res);
        }

        #endregion

        public async Task<IActionResult> ActiveUser(long Id)
        {
            var res = await _userServices.ActiveMobileUserForAdmin(Id);

            return Json(res);
        }
    }
}
