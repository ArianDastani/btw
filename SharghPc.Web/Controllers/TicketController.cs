using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Contact;
using SharghPc.DataLayer.DTOs.Contact;

namespace SharghPc.Web.Controllers
{
    [Authorize(Policy = "CustomerRole")]

    public class TicketController : SiteBaseController
    {
        private IContactServices _contactServices;
        public TicketController(IContactServices contactServices)
        {
            _contactServices = contactServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("add-ticket")]
        public async Task<IActionResult> AddTicket()
        {
            return View();
        }

        [HttpPost("add-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddTicket(AddTicketDTO addTicketDTO)
        {
            if (ModelState.IsValid)
            {
                var res = await _contactServices.AddUserTicket(addTicketDTO, User.GetUserId());
                switch (res)
                {
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        TempData[InfoMessage] = "پاسخ به زودی ارسال خواهد شد";
                        return Redirect("/");
                        break;
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "عملیت با خطا مواجه شد";
                        break;
                }
            }

            return View(addTicketDTO);
        }
    }
}
