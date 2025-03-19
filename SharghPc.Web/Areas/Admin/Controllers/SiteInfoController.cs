using Microsoft.AspNetCore.Mvc;
using SharghPc.Application.Services.Site;
using SharghPc.DataLayer.DTOs.Contact;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class SiteInfoController : AdminBaseController
    {
        #region Ctor

        private ISiteInfoServices _siteInfoServices;

        public SiteInfoController(ISiteInfoServices siteInfoServices)
        {
            _siteInfoServices = siteInfoServices;
        }

        #endregion

        #region List

        public async Task<IActionResult> Index()
        {
            return View(await _siteInfoServices.GetAllContact());
        }

        #endregion

        #region Load

        public async Task<IActionResult> LoadContact(long? Id)
        {
            var res = await _siteInfoServices.GetContactById(Id);

            return View(res);
        }

        #endregion

        #region AddOrEdit

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEditConatct(AddOrEditContentDto addOrEditContentDto)
        {
            var res = await _siteInfoServices.AddOrEditContact(addOrEditContentDto);

            return RedirectToAction("Index");
        }

        #endregion

        #region Remove

        public async Task<IActionResult> DeleteConatct(long Id)
        {
            var res = await _siteInfoServices.Delete(Id);

            return Json(res);
        }

        #endregion

    }
}
