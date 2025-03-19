using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services;
using SharghPc.DataLayer.DTOs.Account;

namespace SharghPc.Web.Controllers
{
    public class LoginTestController : Controller
    {
        private Itestservices _testservices;

        public LoginTestController(Itestservices testservices)
        {
            _testservices = testservices;
        }
        public async Task<IActionResult> loginTest()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> loginTest(string number)
        {
            var loginAndRegisterWithMobileDto = await _testservices.SendSmsForLoginAndRegisterWithMobile(number);

            //return View("WaitVerifyCode", loginAndRegisterWithMobileDto);
            return RedirectToAction("WaitVerifyCode", loginAndRegisterWithMobileDto);
        }

        [HttpGet]
        public async Task<IActionResult> WaitVerifyCode(ResultLoginAndRegisterWithMobileDto loginAndRegisterWithMobileDto)
        {
            var res = loginAndRegisterWithMobileDto;

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> Login2(string mobile,string code)
        {
            var res = await _testservices.LoginAndRegisterWithMobile(mobile,code);

            switch (res)
            {
                case LoginUserResult.Success:
                    Console.WriteLine("sd");
                    break;
                case LoginUserResult.TryAgain:
                    Console.WriteLine("d");
                    break;
            }

            return View();
        }

    }
}
