using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using SachOnline.Models;

namespace SachOnline.Areas.Admin.Controllers
{
    public class DonDatHangController : Controller
    {
        private SachOnlineEntities4 db = new SachOnlineEntities4();

        // GET: Admin/DONDATHANGs
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] != null)
            {
                int iPageNum = (page ?? 1);
                int iPageSize = 7;
                return View(db.DONDATHANGs.ToList().OrderBy(n => n.MaDonHang).ToPagedList(iPageNum, iPageSize));
            }
            else
            {
                return RedirectToAction("Login", "Home");
            }
        }
        // GET: Admin/DonDatHangs/Details/5
        public ActionResult Details(int? id)
        {
            var dONDATHANG = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dONDATHANG == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(dONDATHANG);
        }

        // GET: Admin/DonDatHang/Create
        public ActionResult Create()
        {
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTen");
            return View();
        }

        // POST: Admin/DONDATHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MaDonHang,DaThanhToan,TinhTrangGiaoHang,NgayDat,NgayGiao,MaKH")] DONDATHANG dONDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.DONDATHANGs.Add(dONDATHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTen", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // GET: Admin/DONDATHANGs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DONDATHANG dONDATHANG = db.DONDATHANGs.Find(id);
            if (dONDATHANG == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTen", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // POST: Admin/DONDATHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MaDonHang,DaThanhToan,TinhTrangGiaoHang,NgayDat,NgayGiao,MaKH")] DONDATHANG dONDATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dONDATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MaKH = new SelectList(db.KHACHHANGs, "MaKH", "HoTen", dONDATHANG.MaKH);
            return View(dONDATHANG);
        }

        // GET: Admin/DONDATHANGs/Delete/5
        public ActionResult Delete(int? id)
        {
            var dondathang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(dondathang);
        }

        // POST: Admin/DONDATHANGs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var dondathang = db.DONDATHANGs.SingleOrDefault(n => n.MaDonHang == id);
            if (dondathang == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var ctdh = db.CHITIETDATHANGs.Where(ct => ct.MaDonHang == id);
            if (ctdh.Count() > 0)
            {
                ViewBag.ThongBao = "Đơn hàng này đang có trong bảng Chi tiết đặt hàng <br> " + " Nếu muốn xóa thì phải xóa hết mã đơn hàng này trong bảng Chi tiết đặt hàng";
                return View(dondathang);
            }

            db.DONDATHANGs.Remove(dondathang);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
