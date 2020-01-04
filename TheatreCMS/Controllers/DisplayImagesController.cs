using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Models;

namespace TheatreCMS.Controllers
{
    public class DisplayImagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DisplayImages
        public ActionResult Index()
        {
            return View(db.DisplayImages.ToList());
        }

        // GET: DisplayImages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayImage displayImage = db.DisplayImages.Find(id);
            if (displayImage == null)
            {
                return HttpNotFound();
            }
            return View(displayImage);
        }

        // GET: DisplayImages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DisplayImages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InfoId,Title,Description,Image,File")] DisplayImage displayImage)
        {
            if (ModelState.IsValid)
            {
                db.DisplayImages.Add(displayImage);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(displayImage);
        }

        // GET: DisplayImages/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayImage displayImage = db.DisplayImages.Find(id);
            if (displayImage == null)
            {
                return HttpNotFound();
            }
            return View(displayImage);
        }

        // POST: DisplayImages/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InfoId,Title,Description,Image,File")] DisplayImage displayImage)
        {
            if (ModelState.IsValid)
            {
                db.Entry(displayImage).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(displayImage);
        }

        // GET: DisplayImages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayImage displayImage = db.DisplayImages.Find(id);
            if (displayImage == null)
            {
                return HttpNotFound();
            }
            return View(displayImage);
        }

        // POST: DisplayImages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisplayImage displayImage = db.DisplayImages.Find(id);
            db.DisplayImages.Remove(displayImage);
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
    }
}
