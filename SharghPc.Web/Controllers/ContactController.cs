using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Contact;
using SharghPc.Application.Services.Site;
using SharghPc.DataLayer.DTOs;
using SharghPc.DataLayer.DTOs.Contact;

namespace SharghPc.Web.Controllers
{
    public class ContactController :SiteBaseController
    {
        private readonly ISiteInfoServices _siteInfoServices;
        private IContactServices _contactServices;

        public ContactController(ISiteInfoServices siteInfoServices, IContactServices contactServices)
        {
            _siteInfoServices = siteInfoServices;
            _contactServices = contactServices;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            //var res = await _siteInfoServices.GetSiteInfoAsync();
            //ContactViewModel contactViewModel = new ContactViewModel()
            //{
            //    SiteInfo = res
            //};
            //return View(contactViewModel);
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(CreatContactUsDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var ip= HttpContext.Connection.RemoteIpAddress.ToString();
            var res = await _contactServices.CreateContactUs(dto, IdentityExtensions.GetUserId(User), ip);

            if (res)
            {
                TempData[SuccessMessage] = "پیام شما با موفقیت ارسال شد";
                return RedirectToAction("Index");
            }

            TempData[ErrorMessage] = "خطا در ارسال پیام";

            return View();
        }
    }
}
