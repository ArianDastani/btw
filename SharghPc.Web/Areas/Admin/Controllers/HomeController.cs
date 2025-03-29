using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Site;
using SharghPc.Application.Services.Sms;
using SharghPc.Web.Areas.Admin.Controllers;
using System.Net.NetworkInformation;

namespace RavistekTicket.Peresentation.Areas.Admin.Controllers
{
    public class HomeController : AdminBaseController
    {
        private ISiteInfoServices _siteInfoServices;
        private ISmsServices _smsServices;

        public HomeController(ISiteInfoServices siteInfoServices, ISmsServices smsServices)
        {
            _siteInfoServices = siteInfoServices;
            _smsServices = smsServices;
        }
        public async Task<IActionResult> Index()
        {
            var res = await _siteInfoServices.GetAdminViewModels();

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> SendWellcomSms(string number)
        {
            if (number == null)
            {
                TempData[ErrorMessage] = "شماره را وارد کنید";
                return RedirectToAction("Index");
            }

            if (number.Length < 11 || number.Length > 11)
            {
                TempData[ErrorMessage] = "شماره وارد شده نامعتبر میباشد";
                return RedirectToAction("Index");
            }

            try
            {
                Ping ping = new Ping();
                PingReply pingReply = ping.Send("google.com");

                if (pingReply.Status != IPStatus.Success)
                {
                    TempData[ErrorMessage] = "خطا در ارسال پیام . اتصال خود به اینترنت را بررسی کنید";
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                TempData[ErrorMessage] = "خطا در ارسال پیام . اتصال خود به اینترنت را بررسی کنید";
                return RedirectToAction("Index");
            }




            var re2s = await _smsServices.SendMessageForMobile(number, "hi");

            if (re2s)
            {
                TempData[SuccessMessage] = "با موفقیت ارسال شد";
            }
            else
            {
                TempData[WarningMessage] = "خطا در ارسال مجددا تلاش کنید";

            }
            return RedirectToAction("Index");

        }
    }
}
