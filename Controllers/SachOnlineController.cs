using PagedList;
using PagedList.Mvc;
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
        public ActionResult Index(int? page)
        {
            int pageSize = 6;  // số sách mỗi trang
            int pageNum = page ?? 1; // số trang hiện tại (mặc định là 1)

            var listSachMoi = db.SACHes.OrderByDescending(s => s.NgayCapNhat);
            return View(listSachMoi.ToPagedList(pageNum, pageSize));
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
        public ActionResult SachTheoChuDe(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = page ?? 1;

            var sach = db.SACHes.Where(s => s.MaCD == id)
                                .OrderByDescending(s => s.NgayCapNhat)
                                .ToPagedList(pageNum, pageSize);

            ViewBag.MaChuDe = id; // để giữ lại khi tạo link phân trang
            return View(sach);
        }
        public ActionResult SachTheoNhaXuatBan(int id, int? page)
        {
            int pageSize = 6;
            int pageNum = (page ?? 1);
            var sach = db.SACHes.Where(s => s.MaNXB == id).OrderBy(s => s.MaSach).ToPagedList(pageNum, pageSize);
            ViewBag.MaNXB = id;
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
        public ActionResult LoginLogout()
        {
            return PartialView();
        }
    }
}