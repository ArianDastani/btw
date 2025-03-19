using MarketPlace.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Shipment;

namespace SharghPc.Web.Areas.User.Controllers
{
    public class ShipmentController : UserBaseController
    {
        #region Ctor

        private IShipmentServices _shipmentServices;

        public ShipmentController(IShipmentServices shipmentServices)
        {
            _shipmentServices = shipmentServices;
        }

        #endregion

        #region List

        public async Task<IActionResult> Index()
        {
            var res = await _shipmentServices.GetShipment(User.GetUserId());

            return View(res);
        }

        #endregion

        #region Add

        public async Task<IActionResult> AddShipment()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddShipment(AddUserShipmentDto ShipmentDto)
        {
            if (!ModelState.IsValid)
            {
                TempData[WarningMessage] = "تمامی موارد خواسته شده را وارد کنید";
                return View(ShipmentDto);
            }

            var res = await _shipmentServices.AddUserShipment(ShipmentDto, User.GetUserId());

            if (res)
            {
                TempData[SuccessMessage] = "آدرس با موفقیت ثبت شد";
                return Redirect("/cart");
            }
            else
            {
                TempData[WarningMessage] = "ناموفق بود";
                return View(ShipmentDto);
            }

        }
        #endregion

        #region Edit

        public async Task<IActionResult> EditShipment(long Id)
        {
            var res = await _shipmentServices.GetShipmentForEdit(Id);

            if (res == null)
            {
                return RedirectToAction("Index");
            }

            return View(res);
        }

        [HttpPost]
        public async Task<IActionResult> EditShipment(GetShipmentForEditDto shipmentEditDto)
        {
            var res = await _shipmentServices.EditShipment(shipmentEditDto);

            if (res)
            {
                TempData[SuccessMessage] = "آدرس با موفقیت ویرایش شد";
                return Redirect("/cart");
            }
            else
            {
                TempData[WarningMessage] = "ناموفق بود";
                return View(shipmentEditDto);
            }

            return View();
        }

        #endregion
    }
}
