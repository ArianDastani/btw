using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Order;
using SharghPc.DataLayer.Entites.ProductOrder;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class OrderController : AdminBaseController
    {
        #region Ctor

        private IOrderServices _orderServices;

        public OrderController(IOrderServices orderServices)
        {
            _orderServices = orderServices;
        }

        #endregion

        #region List

        public async Task<IActionResult> Index(OrderState orderState)
        {
            var orders = await _orderServices.GetOrdersForAdmin(orderState);

            return View(orders);
        }

        #endregion

        #region AddToDelivered

        public async Task<IActionResult> AddToDelivered(long Id)
        {
            var res = await _orderServices.AddToDelivered(Id);

            return Json(res);
        }

        #endregion

        #region Detail

        public async Task<IActionResult> GetOrder(long Id)
        {
            var order = await _orderServices.GetOrderForAdmin(Id);

            if (order == null) return RedirectToAction("Index");

            return View(order);
        }


        public async Task<IActionResult> OrderPrint(long Id)
        {
            var order = await _orderServices.GetOrderForAdmin(Id);

            if (order == null) return RedirectToAction("Index");

            return View(order);
        }
        #endregion

    }
}
