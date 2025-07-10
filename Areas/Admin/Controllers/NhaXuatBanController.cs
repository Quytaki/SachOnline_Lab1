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
    public class NhaXuatBanController : Controller
    {
        // GET: Admin/NhaXuatBan
        SachOnlineEntities4 db = new SachOnlineEntities4();
        public ActionResult Index(int? page)
        {
            if (Session["Admin"] != null)
            {
                int iPageNum = (page ?? 1);
                int iPageSize = 7;
                return View(db.NHAXUATBANs.ToList().OrderBy(n => n.MaNXB).ToPagedList(iPageNum, iPageSize));
            }
            else
            {
                return RedirectToAction("Login" , "Home");
            }
        }
        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(NHAXUATBAN nxb, FormCollection f)
        {
            if (f["sTenNXB"] == null)
            {
                ViewBag.ThongBao = "Hãy nhập tên nhà xuất bản.";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    nxb.TenNXB = f["sTenNXB"];
                    nxb.DiaChi = f["sDiaChi"];
                    nxb.DienThoai = f["sSoDienThoai"];
                    db.NHAXUATBANs.Add(nxb);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }

                return View();

            }
        }
        public ActionResult Details(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpGet]
        public ActionResult Delete(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(int id, FormCollection f)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            var sach = db.SACHes.Where(s => s.MaNXB == id);
            if (sach.Count() > 0)
            {
                ViewBag.ThongBao = "Nhà xuất bản này đang có trong bảng Sách <br> " + " Nếu muốn xóa thì phải xóa hết sách có mã nhà xuất bản này trong bảng Sách";
                return View(nxb);
            }
            db.NHAXUATBANs.Remove(nxb);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == id);
            if (nxb == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(nxb);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(FormCollection f)
        {
            int maNXB = int.Parse(f["iMaNXB"]);
            var nxb = db.NHAXUATBANs.SingleOrDefault(n => n.MaNXB == maNXB);
            if (ModelState.IsValid)
            {
                nxb.TenNXB = f["iTenNXB"];
                nxb.DiaChi = f["iDiaChi"];
                nxb.DienThoai = f["iSoDienThoai"];
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(nxb);
        }
    }
}