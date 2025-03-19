using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.user;
using SharghPc.DataLayer.DTOs.Account;

namespace SharghPc.Web.Areas.User.Controllers
{
    public class AcountController : UserBaseController
    {
        #region Ctor

        private IUserServices _userServices;

        public AcountController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        #endregion

        #region ChangePassword

        [HttpGet]
        public async Task<IActionResult> ChangePassword()
        {
            return View();
        }


        [HttpPost,ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto passwordDto)
        {
            if (!ModelState.IsValid)
            {
                return View(passwordDto);
            }

            var res = await _userServices.ChangePassword(passwordDto, User.GetUserId());

            if (res)
            {
                TempData[SuccessMessage] = "کلمه عبور شما با موفقیت تغییر پیدا کرد";
                TempData[InfoMessage] = "مجددا وارد اکانت خود شوید";
                await HttpContext.SignOutAsync();
                return Redirect("/Account/Login");
            }
            else
            {
                TempData[WarningMessage] = "عملیات با خطا مواجه شد";
                TempData[InfoMessage] = "لطفا از کلمه عبور جدیدی استفاده بکنید";

                return View();
            }

        }

        #endregion

        #region Profile

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user =await _userServices.GetUserProfile(User.GetUserId());

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Profile(EditUserProfileDto userProfileDto)
        {
            if (!ModelState.IsValid)
            { 
                return View(userProfileDto);
            }

            var res = await _userServices.EditUserProfile(userProfileDto,User.GetUserId());

            switch (res)
            {
                case EditUserProfileResult.NotFound:
                    TempData[WarningMessage] = "کاربری با مشخصات وارد شده یافت نشد";
                    break;
                case EditUserProfileResult.IsNotActiveMobile:
                    TempData[WarningMessage] = "حساب کاربری شما فعال نشده است";

                    break;
                case EditUserProfileResult.Blocked:
                    TempData[WarningMessage] = "حساب کاربری شما مسدود شده است";

                    break;
                case EditUserProfileResult.Error:
                    TempData[WarningMessage] = "عملیات ناموفق بود";

                    break;
                case EditUserProfileResult.Success:
                    TempData[SuccessMessage] = "حساب کاربری شما با موفقیت ویرایش شد";
                    
                    break;
            }

            return View(userProfileDto);
        }

        #endregion

    }

}
