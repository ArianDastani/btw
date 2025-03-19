using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.user;
using SharghPc.DataLayer.DTOs.Account;
using System.Security.Claims;
using EndPoint.Site.Utilities;
using GoogleReCaptcha.V3.Interface;
using SharghPc.Application.Services.Cart;

namespace SharghPc.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Cunstractor

        private IUserServices _userServices;
        private ICaptchaValidator _captchaValidator;
        private ICartServices _cartServices;

        public AccountController(IUserServices userServices, ICaptchaValidator captchaValidator, ICartServices cartServices)
        {
            _userServices = userServices;
            _captchaValidator = captchaValidator;
            _cartServices = cartServices;
        }

        #endregion

        #region Register

        //[HttpGet("register")]
        //public IActionResult Register()
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Redirect("/");
        //    }

        //    return View();
        //}

        //[HttpPost("register"), ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterUserDTO registerUserDTO)
        //{
        //    //if (await _captchaValidator.IsCaptchaPassedAsync(registerUserDTO.Captcha))
        //    //{
        //    //    TempData[ErrorMessage] = "کد کپچا تایید نشد";
        //    //    return View(registerUserDTO);
        //    //}

        //    if (!ModelState.IsValid)
        //    {
        //        return View(registerUserDTO);
        //    }

        //    var res = await _userServices.RegisterUser(registerUserDTO);

        //    switch (res)
        //    {
        //        case RegisterUserDTO.RegisterUserResult.MobileExits:
        //            TempData[ErrorMessage] = "تلفن همراه وارد شده تکراری میباشد ";
        //            return View(registerUserDTO);
        //            break;
        //        case RegisterUserDTO.RegisterUserResult.Success:
        //            TempData[SuccessMessage] = "ثبت نام با موفقیت انجام شد";
        //            TempData[InfoMessage] = "کد تایید برای شما ارسال شد ";
        //            return View(viewName: "ActiveMobile", model: new ActiveMobileDto() { Mobile = registerUserDTO.Mobile });
        //            break;
        //    }

        //    return Redirect("/");


        //}

        //#endregion

        //#region Login

        //[HttpGet]
        //public IActionResult Login()
        //{
        //    return View();
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        //{
        //    if (User.Identity.IsAuthenticated)
        //    {
        //        return Redirect("/");
        //    }

        //    //if (await _captchaValidator.IsCaptchaPassedAsync(loginUserDto.Captcha))
        //    //{
        //    //    TempData[ErrorMessage] = "کد کپچا تایید نشد";
        //    //    return View(loginUserDto);
        //    //}

        //    if (ModelState.IsValid)
        //    {
        //        var res = await _userServices.LoginUser(loginUserDto);

        //        switch (res)
        //        {
        //            case LoginUserResult.NotFound:
        //                TempData[ErrorMessage] = "حساب کاربری یافت نشد";
        //                return View();
        //                break;
        //            case LoginUserResult.NotActivated:
        //                TempData[WarningMessage] = "حساب کاربری فعال نشده";
        //                return View();
        //                break;
        //            case LoginUserResult.Success:

        //                var user = await _userServices.GetUserByMobile(loginUserDto.Mobile);

        //                CookiesManeger cm = new CookiesManeger();

        //                var setUserIdToCart =
        //                    await _cartServices.SetCartToUserId(user.Id, cm.GetBrowserId(HttpContext));

        //                var claims = new List<Claim>()
        //                {
        //                    new Claim(ClaimTypes.Name, user.Mobile),
        //                    new Claim("UserName",user.FirstName+" "+user.LastName),
        //                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //                    new Claim(ClaimTypes.Role,user.Roles.RoleName)
        //                };

        //                var identity = new ClaimsIdentity(claims,
        //                    authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
        //                var prisple = new ClaimsPrincipal(identity);
        //                var propertis = new AuthenticationProperties()
        //                {
        //                    IsPersistent = loginUserDto.RememberMe,
        //                };

        //                await HttpContext.SignInAsync(prisple, propertis);


        //                TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";

        //                if (user.RolesId == 1)
        //                {
        //                    return Redirect("/admin");

        //                }
        //                return Redirect("/cart");
        //        }


        //    }

        //    return Redirect("/");

        //}


        #endregion

        #region Login

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string number)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            if (string.IsNullOrWhiteSpace(number))
            {
                TempData[ErrorMessage] = "شماره خود را وارد کنید";
                RedirectToAction("Login");
            }

            //if (await _captchaValidator.IsCaptchaPassedAsync(loginUserDto.Captcha))
            //{
            //    TempData[ErrorMessage] = "کد کپچا تایید نشد";
            //    return View(loginUserDto);
            //}

            var loginAndRegisterWithMobileDto = await _userServices.SendSmsForLoginAndRegisterWithMobile(number);

            return RedirectToAction("LoginWithMobileCode", loginAndRegisterWithMobileDto);

            //if (ModelState.IsValid)
            //{
            //    var res = await _userServices.LoginUser(loginUserDto);

            //    switch (res)
            //    {
            //        case LoginUserResult.NotFound:
            //            TempData[ErrorMessage] = "حساب کاربری یافت نشد";
            //            return View();
            //            break;
            //        case LoginUserResult.NotActivated:
            //            TempData[WarningMessage] = "حساب کاربری فعال نشده";
            //            return View();
            //            break;
            //        case LoginUserResult.Success:

            //            var user = await _userServices.GetUserByMobile(loginUserDto.Mobile);

            //            CookiesManeger cm = new CookiesManeger();

            //            var setUserIdToCart =
            //                await _cartServices.SetCartToUserId(user.Id, cm.GetBrowserId(HttpContext));

            //            var claims = new List<Claim>()
            //            {
            //                new Claim(ClaimTypes.Name, user.Mobile),
            //                new Claim("UserName",user.FirstName+" "+user.LastName),
            //                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //                new Claim(ClaimTypes.Role,user.Roles.RoleName)
            //            };

            //            var identity = new ClaimsIdentity(claims,
            //                authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
            //            var prisple = new ClaimsPrincipal(identity);
            //            var propertis = new AuthenticationProperties()
            //            {
            //                IsPersistent = loginUserDto.RememberMe,
            //            };

            //            await HttpContext.SignInAsync(prisple, propertis);


            //            TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";

            //            if (user.RolesId == 1)
            //            {
            //                return Redirect("/admin");

            //            }
            //            return Redirect("/cart");
            //    }


            //}


        }


        #endregion

        [HttpGet]
        public async Task<IActionResult> LoginWithMobileCode(ResultLoginAndRegisterWithMobileDto resultLoginAndRegisterWithMobile)
        {

            return View(resultLoginAndRegisterWithMobile);
        }

        [HttpPost]
        public async Task<IActionResult> LoginToSite(string mobile, string code)
        {
            var res = await _userServices.LoginAndRegisterWithMobile(mobile, code);

            switch (res)
            {
                case LoginUserResult.NotFound:
                    TempData[ErrorMessage] = "حساب کاربری یافت نشد";
                    RedirectToAction("Login");
                    break;
                case LoginUserResult.NotActivated:
                    TempData[WarningMessage] = "حساب کاربری فعال نشده";
                    RedirectToAction("Login");
                    break;
                case LoginUserResult.TryAgain:
                    TempData[WarningMessage] = "کد وارد شده نادرست میباشد";
                    RedirectToAction("Login");
                    break;
                case LoginUserResult.Success:

                    var user = await _userServices.GetUserByMobile(mobile);

                    CookiesManeger cm = new CookiesManeger();

                    var setUserIdToCart =
                        await _cartServices.SetCartToUserId(user.Id, cm.GetBrowserId(HttpContext));

                    var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name, user.Mobile),
                            new Claim("UserName",user.FirstName+" "+user.LastName),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                            new Claim(ClaimTypes.Role,user.Roles.RoleName)
                        };

                    var identity = new ClaimsIdentity(claims,
                        authenticationType: CookieAuthenticationDefaults.AuthenticationScheme);
                    var prisple = new ClaimsPrincipal(identity);
                    //var propertis = new AuthenticationProperties()
                    //{
                    //    IsPersistent = loginUserDto.RememberMe,
                    //};

                    await HttpContext.SignInAsync(prisple);


                    TempData[SuccessMessage] = "عملیات ورود با موفقیت انجام شد";

                    if (user.RolesId == 1)
                    {
                        return Redirect("/admin");

                    }
                    return Redirect("/");
            }

            return Redirect("/");

        }

        #region Forget

        [HttpGet]
        public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordDto forgetPasswordDto)
        {
         

            if (!ModelState.IsValid)
            {
                return View(forgetPasswordDto);
            }

            var res = await _userServices.RecoveryUserPassword(forgetPasswordDto);

            switch (res)
            {
                case ForgetPassResult.NotFound:
                    break;
                case ForgetPassResult.Error:
                    break;
                case ForgetPassResult.Success:
                    break;

            }

            return RedirectToAction("Login");
        }




        #endregion

        #region ActiveMobile

        [HttpGet("ActiveMobile/{mobile}")]

        public IActionResult ActiveMobile(string mobile)
        {
            if (User.Identity.IsAuthenticated)
            {
                return Redirect("/");
            }

            var activemobile = new ActiveMobileDto() { Mobile = mobile };

            return View(activemobile);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ActiveMobile(ActiveMobileDto activeMobileDto)
        {


            if (!ModelState.IsValid)
            {
                return View(activeMobileDto);
            }

            var res = await _userServices.ActiveMobile(activeMobileDto);
            if (res)
            {
                TempData[SuccessMessage] = "حساب کاربری شما با موفقیت فعال شد.";
                return RedirectToAction("Login");
            }
            else
            {
                TempData[ErrorMessage] = "حساب کاربری یافت نشد";
            }
            return Redirect("/");
        }



        #endregion
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
