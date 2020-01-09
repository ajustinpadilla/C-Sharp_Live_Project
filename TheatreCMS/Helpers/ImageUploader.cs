using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace TheatreCMS.Helpers
{
    public static class ImageUploader
    {
        public static string ImageToBase64(string path, out byte[] imageBytes)
        {
            using (System.Drawing.Image image = System.Drawing.Image.FromFile(path))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    imageBytes = ms.ToArray();
                    string base64String = Convert.ToBase64String(imageBytes);
                    return base64String;
                }
            }
        }
    }
}