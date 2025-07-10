using System;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Areas.Admin.Controllers
{
    public class UploadController : Controller
    {
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase upload, string CKEditorFuncNum)
        {
            if (upload != null && upload.ContentLength > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var filePath = Path.Combine(Server.MapPath("~/Images/Upload"), fileName);
                upload.SaveAs(filePath);

                var imageUrl = Url.Content("~/Images/Upload/" + fileName);
                var message = "Tải ảnh thành công";

                var output = $@"<script>window.parent.CKEDITOR.tools.callFunction({CKEditorFuncNum}, '{imageUrl}', '{message}');</script>";
                return Content(output, "text/html");
            }

            return HttpNotFound();
        }
    }
}
