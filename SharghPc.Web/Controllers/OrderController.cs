using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Order;
using SharghPc.DataLayer.DTOs.Order;
using SharghPc.Web.PresentationExtensions;

namespace SharghPc.Web.Controllers
{
    public class OrderController : SiteBaseController
    {
        #region ctorf

        private IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        #endregion

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddProductToOrder(AddProductToOrderDto orderDto)
        {
            if (!ModelState.IsValid)
            {
                return JsonResponseStatus.StatusResult(JsonResponseStatusType.Error, "در ثبت خطایی رخ داد", null);
            }

            //var res=  await _orderServices.AddProductToOpenOrder(User.GetUserId(), orderDto);

            //if (res == false)
            //{
            //    return JsonResponseStatus.StatusResult(JsonResponseStatusType.Error, "در ثبت خطایی رخ داد", null);
            //}

            return JsonResponseStatus.StatusResult(JsonResponseStatusType.Success,
                  "محصول مورد نظر با موفقیت به سبد خرید اضافه شد", null);
        }

    }
}
