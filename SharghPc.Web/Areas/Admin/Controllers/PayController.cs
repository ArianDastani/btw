using Microsoft.AspNetCore.Mvc;

namespace SharghPc.Web.Areas.Admin.Controllers
{
    public class PayController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
