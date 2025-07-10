using System.Web.Mvc;

namespace SachOnline.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Admin";

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "SachOnline.Areas.Admin.Controllers" } // THÊM VÀO ĐÂY
            );
        }
    }
}
