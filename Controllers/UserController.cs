using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class UserController : Controller
    {
        private SachOnlineEntities4 db = new SachOnlineEntities4();

        [HttpGet]
        // GET: User
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap(string url) 
        {
            ViewBag.Url = url; 
            return View();
        }

        [HttpGet]
        public ActionResult DangXuat()
        {
            Session.Clear();

            return RedirectToAction("Index", "SachOnline");
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KHACHHANG kh)
        {
            var sHoTen = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNL"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var sNgaySinh = collection["NgaySinh"];

            // Kiểm tra rỗng
            if (String.IsNullOrEmpty(sHoTen))
            {
                ViewData["err1"] = "Họ tên không được rỗng";
            }
            else if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["err2"] = "Tên đăng nhập không được để rỗng";
            }
            else if (sTenDN.Contains(" "))
                ViewData["err2"] = "Tên đăng nhập không được chứa khoảng trắng";
            else if (!System.Text.RegularExpressions.Regex.IsMatch(sTenDN, @"^[a-zA-Z0-9_]{6,20}$"))
                ViewData["err2"] = "Tên đăng nhập phải từ 6–20 ký tự, chỉ gồm chữ, số và dấu gạch dưới";
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["err3"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(sMatKhauNhapLai))
            {
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            }
            else if (sMatKhau != sMatKhauNhapLai)
            {
                ViewData["err4"] = "Mật khẩu nhập lại không khớp";
            }
            // 🔐 Kiểm tra độ mạnh mật khẩu
            else if (sMatKhau.Length < 8 ||
                     !sMatKhau.Any(char.IsUpper) ||
                     !sMatKhau.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                ViewData["err3"] = "Mật khẩu phải có ít nhất 8 ký tự, 1 chữ hoa và 1 ký tự đặc biệt";
            }
            else if (String.IsNullOrEmpty(sEmail))
            {
                ViewData["err5"] = "Email không được rỗng";
            }
            else if (String.IsNullOrEmpty(sDienThoai))
            {
                ViewData["err6"] = "Số điện thoại không được rỗng";
            }
            else if (db.KHACHHANGs.Any(n => n.TaiKhoan == sTenDN))
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            }
            else if (db.KHACHHANGs.Any(n => n.Email == sEmail))
            {
                ViewBag.ThongBao = "Email đã được sử dụng";
            }
            else
            {
                // Gán thông tin
                kh.HoTen = sHoTen;
                kh.TaiKhoan = sTenDN;

                // ✅ Băm mật khẩu với BCrypt
                kh.MatKhau = BCrypt.Net.BCrypt.HashPassword(sMatKhau);

                kh.Email = sEmail;
                kh.DiaChi = sDiaChi;
                kh.DienThoai = sDienThoai;
                kh.NgaySinh = DateTime.Parse(sNgaySinh);

                db.KHACHHANGs.Add(kh);
                db.SaveChanges();

                return RedirectToAction("DangNhap");
            }

            return this.DangKy();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection, string url)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];

            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Bạn chưa nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                ADMIN admin = db.ADMINs.SingleOrDefault(n => n.TenDN == sTenDN && n.MatKhau == sMatKhau);

                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;

                    // ✅ Nếu có url được truyền → quay lại đó
                    if (!string.IsNullOrEmpty(url))
                        return Redirect(url);

                    // ❗ Nếu không có thì mặc định quay về Trang chủ
                    return RedirectToAction("Index", "SachOnline");
                }
                else if (admin != null)
                {
                    Session["Admin"] = admin;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }
    }
}