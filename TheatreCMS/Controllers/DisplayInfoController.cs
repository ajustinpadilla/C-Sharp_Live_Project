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
    public class DisplayInfoController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: DisplayInfo
        public ActionResult Index()
        {
            return View(db.DisplayInfo.ToList());
        }

        // GET: DisplayInfo/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayInfo displayInfo = db.DisplayInfo.Find(id);
            if (displayInfo == null)
            {
                return HttpNotFound();
            }
            return View(displayInfo);
        }

        // GET: DisplayInfo/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: DisplayInfo/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "InfoId,Title,Description,Image,File")] DisplayInfo displayInfo)
        {
            if (ModelState.IsValid)
            {
                db.DisplayInfo.Add(displayInfo);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(displayInfo);
        }

        // GET: DisplayInfo/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayInfo displayInfo = db.DisplayInfo.Find(id);
            if (displayInfo == null)
            {
                return HttpNotFound();
            }
            return View(displayInfo);
        }

        // POST: DisplayInfo/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "InfoId,Title,Description,Image,File")] DisplayInfo displayInfo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(displayInfo).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(displayInfo);
        }

        // GET: DisplayInfo/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DisplayInfo displayInfo = db.DisplayInfo.Find(id);
            if (displayInfo == null)
            {
                return HttpNotFound();
            }
            return View(displayInfo);
        }

        // POST: DisplayInfo/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DisplayInfo displayInfo = db.DisplayInfo.Find(id);
            db.DisplayInfo.Remove(displayInfo);
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
