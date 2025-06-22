using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        SachOnlineEntities4 db = new SachOnlineEntities4();
        private List<SACH> LaySachMoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        // GET: SachOnline
        public ActionResult Index()
        {
            var listSachMoi = LaySachMoi(6);
            return View(listSachMoi);
        }
        public ActionResult ChuDePartial()
        {
            var listChuDe = from cd in db.CHUDEs select cd;
            return PartialView(listChuDe);
        }
        public ActionResult NhaXuatBanPartial()
        {
            var listNhaXuatBan = from nxb in db.NHAXUATBANs select nxb;
            return PartialView(listNhaXuatBan);
        }
        public ActionResult SachBanNhieuPartial()
        {
            var sachBanNhieu = db.SACHes.OrderByDescending(s => s.SoLuongBan).Take(6).ToList();
            return PartialView("SachBanNhieuPartial", sachBanNhieu);

        }
        public ActionResult SachTheoChuDe(int id)
        {
            var sach = from s in db.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SachTheoNhaXuatBan(int id)
        {
            var sach = from s in db.SACHes where s.MaNXB == id select s;
            return View(sach);
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in db.SACHes where s.MaSach == id select s;
            return View(sach.Single());
        }
        public ActionResult NavParital()
        {
            return PartialView("NavPartial");
        }
        public ActionResult SliderPatial()
        {
            return PartialView("SliderPatial");
        }
    }
}