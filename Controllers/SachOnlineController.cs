using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        // GET: SachOnline
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ChuDePartial()
        {
            return PartialView();
        }
        public ActionResult SachBanNhieuPartial()
        {
            return PartialView();
        }
    }
}