using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Helpers;

namespace TheatreCMS.Models
{
    public class ProductionPhotosController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //render image from database
        public ActionResult RenderImage(int id)
        {
            //int photoID = Convert.ToInt32(Request.Form["ProductionPhotos"]);

            var photo = db.ProductionPhotos.Find(id);
            byte[] photoBack = photo.Photo;
            return File(photoBack, "image/png");
        }

        // GET: ProductionPhotos
        public ActionResult Index(ProductionPhotos productionPhotos)
        {
            //int production = Convert.ToInt32(Request.Form["ProductionPhotos"]);
            //var photo = db.ProductionPhotos.Find(photoID.);

            int photoID = productionPhotos.ProPhotoId;
            RenderImage(photoID);
            return View(db.ProductionPhotos.ToList());
        }

       

        // GET: ProductionPhotos/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionPhotos productionPhotos = db.ProductionPhotos.Find(id);
            if (productionPhotos == null)
            {
                return HttpNotFound();
            }
            return View(productionPhotos);
        }

        // GET: ProductionPhotos/Create
        public ActionResult Create()
        {
            ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
            return View();
        }

        // POST: ProductionPhotos/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProPhotoId,Photo,Title,Description")] ProductionPhotos productionPhotos)
        {
            int productionID = Convert.ToInt32(Request.Form["Productions"]);

            if (ModelState.IsValid)
            {
                var production = db.Productions.Find(productionID);

                productionPhotos.Production = production;
                db.ProductionPhotos.Add(productionPhotos);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productionPhotos);
        }

        // GET: ProductionPhotos/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionPhotos productionPhotos = db.ProductionPhotos.Find(id);
            if (productionPhotos == null)
            {
                return HttpNotFound();
            }

            ViewData["Productions"] = new SelectList(db.Productions, "ProductionId", "Title", productionPhotos.Production.ProductionId);
            return View(productionPhotos);
        }

        // POST: ProductionPhotos/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProPhotoId,Photo,Title,Description")] ProductionPhotos productionPhotos)
        {
            if (ModelState.IsValid)
            {
                ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");

                db.Entry(productionPhotos).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(productionPhotos);
        }

        // GET: ProductionPhotos/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductionPhotos productionPhotos = db.ProductionPhotos.Find(id);
            if (productionPhotos == null)
            {
                return HttpNotFound();
            }
            return View(productionPhotos);
        }

        // POST: ProductionPhotos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductionPhotos productionPhotos = db.ProductionPhotos.Find(id);
            db.ProductionPhotos.Remove(productionPhotos);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
                //Using Helpers.ImageUploader.ImageBytes to get the byte[] representation of the file
                //and extracting the string representation as a returned out-parrameter
                string imageBase64;
                byte[] imageBytes = ImageUploader.ImageBytes(file, out imageBase64);

                //Add the base64 representation of the image to the ViewBag to be accessed by the View
                ViewBag.ImageData = String.Format("data:image/png;base64,{0}", imageBase64);

                ViewBag.Message = "Image uploaded successfully!";
                return View();
            }

            catch
            {
                //Using this empty string for the View to trigger when an upload fails
                ViewBag.ImageData = "";
                ViewBag.Message = "There was an error uploading your image :(";
            }
            return View();
        }
    }
}
