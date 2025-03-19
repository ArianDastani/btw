using Microsoft.AspNetCore.Mvc;

namespace SharghPc.Web.Areas.Admin.ViewComponents
{
    //public class AdminHeaderViewComponents : ViewComponent
    //{
    //    public async Task<IViewComponentResult> InvokeAsync()
    //    {
    //        return View("AdminHeader");
    //    }
    //}

    public class AdminFooterViewComponents : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminFooter");
        }
    }

    public class AdminSidebarViewComponents : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminSidebar");
        }
    }
}
