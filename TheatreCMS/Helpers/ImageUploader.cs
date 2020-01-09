using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;

namespace TheatreCMS.Helpers
{
    public static class ImageUploader
    {
        public static Image UploadImage(string filename, out byte[] imageBytes)
        {
            //Function that takes in the filname of the image and returns the System.Drawing.Image representation of the file. 

            //image is rendered and stored as Image type
            Image image = Image.FromFile(filename);

            //converts the image into a Byte Array and stores is at as an out-parameter
            var imgCon = new ImageConverter();
            imageBytes = (byte[])imgCon.ConvertTo(image, typeof(byte[]));

            //returns image as Image object
            return image;
        }
    }
}