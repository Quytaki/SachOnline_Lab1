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
        [HttpGet] // Đánh dấu action chỉ được xử lý dưới phương thức GET
        public ActionResult DangKy()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangKy(FormCollection collection)
        {
            // Lấy dữ liệu từ form
            var sHoTen = collection["HoTen"];
            var sDiaChi = collection["DiaChi"];
            var sTenDN = collection["TenDN"];
            var sMatkhau = collection["MatKhau"];
            var sMatkhauNhapLai = collection["MatKhauNL"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var sNgaySinh = collection["NgaySinh"];

            // Giữ lại dữ liệu đã nhập khi render view
            ViewBag.HoTen = sHoTen;
            ViewBag.DiaChi = sDiaChi;
            ViewBag.TenDN = sTenDN;
            ViewBag.Email = sEmail;
            ViewBag.DienThoai = sDienThoai;
            ViewBag.NgaySinh = sNgaySinh;

            // Validate các trường bắt buộc
            if (String.IsNullOrEmpty(sHoTen))
                ViewData["err1"] = "Họ tên không được rỗng";
            else if (String.IsNullOrEmpty(sTenDN))
                ViewData["err2"] = "Tên đăng nhập không được rỗng";
            else if (String.IsNullOrEmpty(sMatkhau))
                ViewData["err3"] = "Phải nhập mật khẩu";
            else if (String.IsNullOrEmpty(sMatkhauNhapLai))
                ViewData["err4"] = "Phải nhập lại mật khẩu";
            else if (sMatkhau != sMatkhauNhapLai)
                ViewData["err4"] = "MK nhập lại không khớp";
            else if (String.IsNullOrEmpty(sEmail))
                ViewData["err5"] = "Email không được rỗng";
            else if (String.IsNullOrEmpty(sDienThoai))
                ViewData["err6"] = "Số điện thoại không được rỗng";
            else if (db.KHACHHANGs.Any(n => n.TaiKhoan == sTenDN))
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
            else if (db.KHACHHANGs.Any(n => n.Email == sEmail))
                ViewBag.ThongBao = "Email này đã được sử dụng";
            else
            {
                // Parse Ngày Sinh nếu có
                DateTime? ngaySinh = null;
                if (!String.IsNullOrEmpty(sNgaySinh))
                {
                    if (DateTime.TryParse(sNgaySinh, out DateTime tmp))
                        ngaySinh = tmp;
                    else
                        ViewBag.ThongBao = "Ngày sinh không đúng định dạng";
                }

                if (ViewBag.ThongBao == null) // Nếu không có lỗi Ngày Sinh
                {
                    try
                    {
                        KHACHHANG kh = new KHACHHANG
                        {
                            HoTen = sHoTen,
                            TaiKhoan = sTenDN,
                            MatKhau = sMatkhau,
                            Email = sEmail,
                            DiaChi = sDiaChi,
                            DienThoai = sDienThoai,
                            NgaySinh = ngaySinh
                        };

                        db.KHACHHANGs.Add(kh);
                        int rows = db.SaveChanges();

                        if (rows > 0)
                        {
                            TempData["SuccessMessage"] = "Đăng ký thành công!";
                            return RedirectToAction("Index");
                        }
                        else
                            ViewBag.ThongBao = "Không có dòng nào được thêm";
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ThongBao = "Lỗi khi lưu: " + ex.Message;
                    }
                }
            }

            return View();
        }


        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Vui lòng nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(sMatkhau))
            {
                ViewData["Err2"] = "Vui lòng nhập mật khẩu";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatkhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công vào hệ thống";
                    Session["TaiKhoan"] = kh;

                 // return RedirectToAction("DatHang", "GioHang");


                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không hợp lệ";
                }

            }
            return View();

        }

    }
}