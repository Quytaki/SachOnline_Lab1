using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace SachOnline.Areas.Admin.Controllers
{
    public class KhachHangController : Controller
    {
        SachOnlineEntities4 db = new SachOnlineEntities4();
        // GET: Admin/KhachHang
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] != null)
            {
                int iPageNum = (page ?? 1);
                int iPageSize = 7;
                return View(db.KHACHHANGs.ToList().OrderBy(n => n.MaKH).ToPagedList(iPageNum, iPageSize));
            }
            else
            {
                return RedirectToAction("Login","Home");
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KHACHHANG kh, FormCollection f)
        {
            if (f["sTenKH"] == null)
            {
                ViewBag.ThongBao = "Hãy nhập tên khách hàng.";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    kh.HoTen = f["sTenKH"];
                    kh.DiaChi = f["sDiaChi"];
                    kh.DienThoai = f["sSoDienThoai"];
                    kh.TaiKhoan = f["sTenDN"];
                    kh.MatKhau = f["sMatKhau"];
                    kh.NgaySinh = Convert.ToDateTime(f["sNgaySinh"]);
                    kh.Email = f["sEmail"];
                    db.KHACHHANGs.Add(kh);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View();

            }
        }
        public ActionResult Details(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var dhh = db.DONDATHANGs.Where(s => s.MaKH == id);
            if (dhh.Count() > 0)
            {
                ViewBag.ThongBao = "Nhà xuất bản này đang có trong bảng Đơn đặt hàng <br> " + " Nếu muốn xóa thì phải xóa hết đơn có mã khách hàng này trong bảng Đơn đặt hàng";
                return View(dhh);
            }
            db.KHACHHANGs.Remove(kh);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == id);
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            int maKH = int.Parse(f["iMaKH"]);
            var kh = db.KHACHHANGs.SingleOrDefault(n => n.MaKH == maKH);
            if (ModelState.IsValid)
            {
                kh.HoTen = f["iHoTenKH"];
                kh.DiaChi = f["iDiaChiKH"];
                kh.DienThoai = f["iDienThoaiKH"];
                kh.TaiKhoan = f["iTenDN"];
                kh.MatKhau = f["iMatKhau"];
                kh.NgaySinh = Convert.ToDateTime(f["iNgaySinh"]);
                kh.Email = f["iEmail"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(kh);
        }
    }
}