using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Order;

namespace SharghPc.Web.Areas.User.Controllers
{
    public class HomeController : UserBaseController
    {
        #region ctor

        private IOrderServices _orderServices;

        public HomeController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        #endregion

        #region Dashbord

        [HttpGet("DashBoard")]
        public async Task<IActionResult> DashBoard()
        {
            var order = await _orderServices.GetOrdersForUser(User.GetUserId());

            return View(order);
        }

        public async Task<IActionResult> GetOrder(long Id)
        {
            if (Id == 0 || Id == null) return NotFound();

            var order = await _orderServices.GetOrderDetailForSite(Id, User.GetUserId());

            if(order == null) return NotFound();


            return View(order);
        }

        #endregion
    }
}
