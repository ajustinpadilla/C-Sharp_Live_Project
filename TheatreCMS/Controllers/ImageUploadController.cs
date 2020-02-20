using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Drawing;
using TheatreCMS.Models;

namespace TheatreCMS.Controllers
{
    public class ImageUploadController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //file -> buyte[] (out string64)
        public static byte[] ImageBytes(HttpPostedFileBase file, out string imageBase64)
        {
            //Convert the file into a System.Drawing.Image type
            Image image = Image.FromStream(file.InputStream, true, true);
            //Convert that image into a Byte Array to facilitate storing the image in a database
            var converter = new ImageConverter();
            byte[] imageBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
            //Extract the String.Base64 representation of the image for inline, browser-side rendering during display
            imageBase64 = Convert.ToBase64String(imageBytes);
            //return Byte Array
            return imageBytes;
        }

        //byte[] -> smaller byte[]
        public static byte[] ImageThumbnail(byte[] imageBytes, int thumbWidth, int thumbHeight)
        {
            using (MemoryStream ms = new MemoryStream())
            using (Image thumbnail = Image.FromStream(new MemoryStream(imageBytes)).GetThumbnailImage(thumbWidth, thumbHeight, null, new IntPtr()))
            {
                thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                return ms.ToArray();
            }
        }

        public FileContentResult ImageView(int id, string table)
        {
            string path = "";
            if (table == "Sponsor")
            {
                var sponsor = db.Sponsors.Find(id);
                path = Convert.ToBase64String(sponsor.Logo);
            }
            byte[] imgArray = System.IO.File.ReadAllBytes(path);
            return new FileContentResult(imgArray, "image/jpg");
        }
    }
}