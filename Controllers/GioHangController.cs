using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using System.Net.Mail;
using Newtonsoft.Json.Linq;


namespace SachOnline.Controllers
{
    public class GioHangController : Controller
    {
        SachOnlineEntities4 db = new SachOnlineEntities4();

        // GET: GioHang
        public ActionResult GioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "SachOnline");
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);

        }
        public List<GioHang> LayGioHang()
        {
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<GioHang>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        public ActionResult ThemGioHang(int ms, string url)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSach == ms);
            if (sp == null)
            {
                sp = new GioHang(ms);
                lstGioHang.Add(sp);
            }
            else
            {
                sp.iSoLuong++;
            }
            return Redirect(url);
        }

        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iSoLuong);
            }
            return iTongSoLuong;
        }

        private double TongTien()
        {
            double dTongTien = 0;
            List<GioHang> lstGioHang = Session["GioHang"] as List<GioHang>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.dThanhTien);
            }
            return dTongTien;
        }
        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaSPKhoiGioHang(int iMaSach)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                lstGioHang.RemoveAll(n => n.iMaSach == iMaSach);
                if (lstGioHang.Count == 0)
                {
                    return RedirectToAction("Index", "SachOnline");
                }
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int iMaSach, FormCollection f)
        {
            List<GioHang> lstGioHang = LayGioHang();
            GioHang sp = lstGioHang.Find(n => n.iMaSach == iMaSach);
            if (sp != null)
            {
                sp.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang");
        }
        public ActionResult XoaGioHang()
        {
            List<GioHang> lstGioHang = LayGioHang();
            lstGioHang.Clear();
            return RedirectToAction("Index", "SachOnline");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "User");
            }
            if (!string.IsNullOrEmpty(Session["GioHang"] as string))
            {
                return RedirectToAction("Index", "SachOnline");
            }
            List<GioHang> lstGioHang = LayGioHang();
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return View(lstGioHang);
        }
        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            // Thêm đơn hàng
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> lstGioHang = LayGioHang();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            var NgayGiao = String.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            ddh.NgayGiao = DateTime.Parse(NgayGiao);
            ddh.TinhTrangGiaoHang = 1;
            ddh.DaThanhToan = false;
            db.DONDATHANGs.Add(ddh);
            db.SaveChanges();


            // Thêm chi tiết đơn hàng
            foreach (var item in lstGioHang)
            {
                CHITIETDATHANG ctdh = new CHITIETDATHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSach = item.iMaSach;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dDonGia;
                db.CHITIETDATHANGs.Add(ctdh);
            }
            db.SaveChanges();
            // Gửi email xác nhận đơn hàng
            string emailBody = "Xin chào " + kh.HoTen + ",\n\nCảm ơn bạn đã đặt hàng tại cửa hàng của chúng tôi. Dưới đây là thông tin về đơn hàng của bạn:\n\n";

            decimal TongTien = 0;
            foreach (var item in lstGioHang)
            {
                emailBody += "Tên sách: " + item.sTenSach + "\nSố lượng: " + item.iSoLuong.ToString() + "\nĐơn giá: " + item.dDonGia.ToString() + "\n\n";
                TongTien += item.iSoLuong * (decimal)item.dDonGia;
            }
            emailBody += "Tổng tiền: " + TongTien.ToString() + "\n\nChúng tôi sẽ liên hệ với bạn sớm nhất để xác nhận đơn hàng. Cảm ơn bạn đã mua hàng tại cửa hàng của chúng tôi.\n\nTrân trọng,\n[Your Store Name]";

            GuiEmail(kh.Email, "Thông tin đơn hàng từ [Your Store Name]", emailBody);

            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        private void GuiEmail(string toEmail, string subject, string body)
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(toEmail);
            mail.From = new MailAddress("2124802010521@student.tdmu.edu.vn", "SachOnline Shop");
            mail.Subject = subject;
            mail.Body = body;
            mail.IsBodyHtml = false;

            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("2124802010521@student.tdmu.edu.vn", "enmhtwwgrxusafab"), // KHÔNG có \r\n
                EnableSsl = true
            };

            try
            {
                smtp.Send(mail);
            }
            catch (SmtpException ex)
            {
                // Bạn có thể log lỗi ra file hoặc hiển thị thông báo
                throw new Exception("Không thể gửi email: " + ex.Message);
            }
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
        public ActionResult Payment()
        {
            string endpoint = "https://test-payment.momo.vn/v2/gateway/api/create";
            string partnerCode = "MOMO0HGO20210817";  // Sửa ở đây
            string accessKey = "F8BBA842ECF85";
            string secretKey = "K951B6PE1waDMi640xX08PD3vg6EkVlz";
            string orderInfo = "Thanh toán đơn hàng";
            string returnUrl = "https://c2ae-2a09-bac1-7a80-18-00-3c2-1.ngrok-free.app/GioHang/ConfirmPaymentClient";
            string notifyurl = "https://c2ae-2a09-bac1-7a80-18-00-3c2-1.ngrok-free.app/GioHang/ConfirmPaymentClient";

            string amount = "1000";
            string orderid = DateTime.Now.Ticks.ToString();
            string requestId = DateTime.Now.Ticks.ToString();
            string extraData = "";

            string rawHash = "accessKey=" + accessKey +
                             "&amount=" + amount +
                             "&extraData=" + extraData +
                             "&ipnUrl=" + notifyurl +
                             "&orderId=" + orderid +
                             "&orderInfo=" + orderInfo +
                             "&partnerCode=" + partnerCode +
                             "&redirectUrl=" + returnUrl +
                             "&requestId=" + requestId +
                             "&requestType=captureWallet";

            MoMoSecurity crypto = new MoMoSecurity();
            string signature = crypto.signSHA256(rawHash, secretKey);

            JObject message = new JObject
{
    { "partnerCode", partnerCode },
    { "accessKey", accessKey },
    { "requestId", requestId },
    { "amount", amount },
    { "orderId", orderid },
    { "orderInfo", orderInfo },
    { "redirectUrl", returnUrl },
    { "ipnUrl", notifyurl },
    { "extraData", extraData },
    { "requestType", "captureWallet" },
    { "signature", signature },
    { "lang", "vi" }
};


            string responseFromMomo = PaymentRequest.sendPaymentRequest(endpoint, message.ToString());

            if (string.IsNullOrEmpty(responseFromMomo))
            {
                return Content("❌ Không nhận được phản hồi từ MoMo. Hãy kiểm tra lại đường dẫn endpoint hoặc kết nối mạng.");
            }

            JObject jmessage;
            try
            {
                jmessage = JObject.Parse(responseFromMomo);
            }
            catch (Exception ex)
            {
                return Content("❌ JSON phản hồi từ MoMo không hợp lệ:<br/>" + responseFromMomo, "text/html");
            }

            if (jmessage["payUrl"] == null)
            {
                return Content("❌ Không tìm thấy <b>payUrl</b> trong phản hồi từ MoMo:<br/><pre>" + responseFromMomo + "</pre>", "text/html");
            }

            return Redirect(jmessage["payUrl"].ToString());
        }



        //Khi thanh toán xong ở cổng thanh toán Momo, Momo sẽ trả về một số thông tin, trong đó có errorCode để check thông tin thanh toán
        //errorCode = 0 : thanh toán thành công (Request.QueryString["errorCode"])
        //Tham khảo bảng mã lỗi tại: https://developers.momo.vn/#/docs/aio/?id=b%e1%ba%a3ng-m%c3%a3-l%e1%bb%97i
        public ActionResult ConfirmPaymentClient(Result result)
        {
            //lấy kết quả Momo trả về và hiển thị thông báo cho người dùng (có thể lấy dữ liệu ở đây cập nhật xuống db)
            string rMessage = result.message;
            string rOrderId = result.orderId;
            string rErrorCode = result.errorCode; // = 0: thanh toán thành công
            return View();
        }

        [HttpPost]
        public void SavePayment()
        {
            //cập nhật dữ liệu vào db
            String a = "";
        }

    }
}