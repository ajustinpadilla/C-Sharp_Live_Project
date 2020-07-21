using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Media.Animation;
using TheatreCMS.Models;
using TheatreCMS.ViewModels;

namespace TheatreCMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PhotoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Photo
        public ActionResult Index()
        {
            return View(db.Photo.ToList());
        }

        // Action method for displaying infinite scroll 
        public ActionResult GetPhotos(int pageIndex, int pageSize)
        {
            System.Threading.Thread.Sleep(500);  //sets a delay on loading. Used for debugging.
            var query = (from photo in db.Photo
                         orderby photo.PhotoId ascending
                         select new { photo.PhotoId, photo.OriginalHeight, photo.OriginalWidth, photo.Title }).Skip(pageIndex * pageSize).Take(pageSize);  // selecting anonymous type is done to prevent passing the byte array in the PhotFile attribute 

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(query), JsonRequestBehavior.AllowGet);
        }

        // GET: Photo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        //file -> byte[]
        [AllowAnonymous]
        public static byte[] ImageBytes(HttpPostedFileBase file)
        {
            //Convert the file into a System.Drawing.Image type
            Image image = Image.FromStream(file.InputStream, true, true);
            //Convert that image into a Byte Array to facilitate storing the image in a database
            var converter = new ImageConverter();
            byte[] imageBytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
            //return Byte Array
            return imageBytes;
        }

        //byte[] -> smaller byte[]
        [AllowAnonymous]
        public static byte[] ImageThumbnail(byte[] imageBytes, int thumbWidth, int thumbHeight)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Image img = Image.FromStream(new MemoryStream(imageBytes));
                using (Image thumbnail = img.GetThumbnailImage(img.Width, img.Height, null, new IntPtr()))
                {
                    thumbnail.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return ms.ToArray();
                }
            }
        }

        // GET: Photo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Photo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PhotoId,PhotoFile,OriginalHeight,OriginalWidth,Title")] Photo photo, HttpPostedFileBase file)
        {
            byte[] photoArray = ImageBytes(file);
            if (db.Photo.Where(x => x.PhotoFile == photoArray).ToList().Any())
            {
                var id = db.Photo.Where(x => x.PhotoFile == photoArray).ToList().FirstOrDefault().PhotoId;
                ModelState.AddModelError("PhotoFile", "This photo already exists in the database. Would you like to <a href='/Photo/Details/" + id + "'>view</a> or <a href='/Photo/Edit/" + id + "'>edit</a> the photo?");
            }
            if (ModelState.IsValid)
            {
                photo.PhotoFile = photoArray;
                Bitmap img = new Bitmap(file.InputStream);
                photo.OriginalHeight = img.Height;
                photo.OriginalWidth = img.Width;

                db.Photo.Add(photo);
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(photo);
        }

        [AllowAnonymous]
        public static int CreatePhoto(HttpPostedFileBase file, string title)

        {
            var photo = new Photo();
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                photo.Title = title;
                Image image = Image.FromStream(file.InputStream, true, true);
                photo.OriginalHeight = image.Height;
                photo.OriginalWidth = image.Width;
                var converter = new ImageConverter();
                photo.PhotoFile = (byte[])converter.ConvertTo(image, typeof(byte[]));
                db.Photo.Add(photo);
                db.SaveChanges();
                return photo.PhotoId;
            }
        }

        //Takes an image file and title string and returns the PhotoId as string
        [HttpPost]
        [ValidateAntiForgeryToken]
        public string GetPhotoId(HttpPostedFileBase file, string title)
        {
            if (ModelState.IsValid)
            {
                var photo = new Photo();
                photo.Title = title;
                Image image = Image.FromStream(file.InputStream, true, true);
                photo.OriginalHeight = image.Height;
                photo.OriginalWidth = image.Width;
                var converter = new ImageConverter();
                photo.PhotoFile = (byte[])converter.ConvertTo(image, typeof(byte[]));
                db.Photo.Add(photo);
                db.SaveChanges();
                return (photo.PhotoId).ToString();
    
            }

            return null;
        }


        [AllowAnonymous]
        public ActionResult DisplayPhoto(int? id) //nullable int
        {
            string filePath = Server.MapPath(Url.Content("~/Content/Images/no-image.png"));
            Image noImageAvail = Image.FromFile(filePath);
            var converter = new ImageConverter();
            var byteData = (byte[])converter.ConvertTo(noImageAvail, typeof(byte[]));
            if (id.HasValue)
            {
                Photo photo = db.Photo.Find(id);
                if (photo == null)
                {
                    return File(byteData, "image/png");
                }
                else
                {
                    return File(photo.PhotoFile, "image/png");
                }
            }
            else
            {
                return File(byteData, "image/png");
            }
        }

        // GET: Photo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PhotoId,PhotoFile,OriginalHeight,OriginalWidth,Title")] Photo photo, HttpPostedFileBase file)
        {
            var currentphoto = db.Photo.Find(photo.PhotoId);
            byte[] photoArray = ImageBytes(file);
            if (db.Photo.Where(x => x.PhotoFile == photoArray).ToList().Any())
            {
                var id = db.Photo.Where(x => x.PhotoFile == photoArray).ToList().FirstOrDefault().PhotoId;
                ModelState.AddModelError("PhotoFile", "This photo already exists in the database. Would you like to <a href='/Photo/Details/" + id + "'>view</a> the photo?");
            }
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    
                    currentphoto.PhotoFile = photoArray;
                    Bitmap img = new Bitmap(file.InputStream);
                    currentphoto.OriginalHeight = img.Height;
                    currentphoto.OriginalWidth = img.Width;
                    currentphoto.Title = photo.Title;
                    db.Entry(currentphoto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    currentphoto.Title = photo.Title;
                    db.Entry(currentphoto).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
            
            }

            return View(photo);
        }

        // GET: Photo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Photo photo = db.Photo.Find(id);
            if (photo == null)
            {
                return HttpNotFound();
            }
            return View(photo);
        }

        // POST: Photo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Photo photo = db.Photo.Find(id);
            db.Photo.Remove(photo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public static PhotoDependenciesVm FindDependencies(int? Id)
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                var photoDependencies = new PhotoDependenciesVm();
                photoDependencies.ValidId = true;
                if (Id == null)                                                                                              //If the Id argument is null, ValidId is returned false and the method stops
                {
                    photoDependencies.ValidId = false;
                    return null;                                                                                             //Will cause issues if the Id argument is invalid
                }
                var photoEntity = db.Photo.Find(Id);                                                                         //Declaring photo object from passed argument Id
                var sponsorEntity = db.Sponsors.FirstOrDefault(photo => photo.LogoId == photoEntity.PhotoId);                //Declaring sponsor object from photo object
                if (sponsorEntity != null && sponsorEntity.LogoId == photoEntity.PhotoId)                                    //Checks if there is a sponsor, and if that sponsor's photo id matches
                {
                    photoDependencies.Sponsors.Add(sponsorEntity);                                                           //Adds sponsor object to sponsors list inside ViewModel
                }
                var productionEntity = db.ProductionPhotos.FirstOrDefault(photo => photo.PhotoId == photoEntity.PhotoId);    //Declaring production object from photo object
                if (productionEntity != null && productionEntity.PhotoId == photoEntity.PhotoId)                             //Checks if there is a production, and if the prod photo id matches
                {
                    photoDependencies.ProductionPhotos.Add(productionEntity);                                                //Adds prod object to production list inside ViewModel
                }
                photoDependencies.HasDependencies = false;
                //Final check for dependencies. If either sponsorEntity or productionEntity are null an error is thrown, so an evaluation is necessary before comparing photo id's
                if (sponsorEntity != null && photoEntity.PhotoId == sponsorEntity.LogoId || productionEntity != null && photoEntity.PhotoId == productionEntity.PhotoId)
                {
                    photoDependencies.HasDependencies = true;
                }
                int season;
                if (productionEntity != null) season = productionEntity.Production.Season;                                   //Gets the producton's season before closing the connection to the database
                return photoDependencies;
            }
            
        }
    }
}
