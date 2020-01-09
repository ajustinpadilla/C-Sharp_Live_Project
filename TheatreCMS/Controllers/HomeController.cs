using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TheatreCMS.Helpers;

namespace TheatreCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //File upload GET and POST controls
        public ActionResult UploadImage()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            try
            {
                //Upload user photo to project directory
                string _fileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/UploadedImages"), _fileName);
                file.SaveAs(_path);

                //Capture image as base64 string and store image byte array
                byte[] imageBytes;
                string imageBase64 = ImageUploader.ImageToBase64(_path, out imageBytes);

                ViewBag.ImageData = String.Format("data:image/png;base64,{0}", imageBase64);
                ViewBag.Message = "Image uploaded successfully!";
                
                return View();
            }

            catch
            {
                ViewBag.ImageData = "";
                ViewBag.Message = "There was an error uploading your image :(";
            }

            return View();

        }
    }
}