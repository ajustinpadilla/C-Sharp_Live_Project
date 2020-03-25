using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Controllers;
using TheatreCMS.Helpers;
using TheatreCMS.Models;

namespace TheatreCMS.Controllers
{
    public class ProductionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();


        // GET: Productions
        public ActionResult Index()
        {
            return View(db.Productions.ToList());
        }

        public ActionResult Current()
        {
            var current = from a in db.Productions
                          where a.IsCurrent == true
                          select a;
            return View(current.ToList());
        }

        // GET: Productions/Details/5
        public ActionResult Details(int? id)
        {

            AdminSettings adminSettings = AdminSettingsReader.CurrentSettings();

            if (id == null)
            {
                id = adminSettings.onstage;
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // GET: Productions/Create
        public ActionResult Create()
        {
            return View();
        }


        // POST: Productions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductionId,Title,Playwright,Description,Runtime,OpeningDay,ClosingDay,DefaultPhoto_ProPhotoId,ShowtimeEve,ShowtimeMat,TicketLink,Season,IsCurrent,IsWorldPremiere")] Production production, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                //if (upload != null)
                //{
                //    var promoPhoto = ImageUploadController.ImageBytes(upload, out string _64);
                //    production.DefaultPhoto = promoPhoto;
                //}
                db.Productions.Add(production);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(production);
        }


        // GET: Productions/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }

            ViewData["ProductionPhotos"] = new SelectList(db.ProductionPhotos.Where(x => x.Production.ProductionId == id).ToList(), "ProPhotoId", "Title");
            return View(production);
        }


        // POST: Productions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductionId,Title,Playwright,Description,Runtime,OpeningDay,ClosingDay,DefaultPhoto,ShowtimeEve,ShowtimeMat,TicketLink,Season,IsCurrent,IsWorldPremiere")] Production production, HttpPostedFileBase upload)
        {
            int proPhotoId = Convert.ToInt32(Request.Form["ProductionPhotos"]);

            if (ModelState.IsValid)  //!!!!!part controller
            {
                var currentProduction = db.Productions.Find(production.ProductionId);
                currentProduction.Title = production.Title;
                currentProduction.Playwright = production.Playwright;
                currentProduction.Description = production.Description;
                currentProduction.Runtime = production.Runtime;
                currentProduction.OpeningDay = production.OpeningDay;
                currentProduction.ClosingDay = production.ClosingDay;
                currentProduction.ShowtimeEve = production.ShowtimeEve;
                currentProduction.ShowtimeMat = production.ShowtimeMat;
                currentProduction.TicketLink = production.TicketLink;
                currentProduction.Season = production.Season;
                currentProduction.IsCurrent = production.IsCurrent;
                currentProduction.IsWorldPremiere = production.IsWorldPremiere;

                var productionPhoto = db.ProductionPhotos.Find(proPhotoId);


                currentProduction.DefaultPhoto = productionPhoto;
                try
                {
                    db.Entry(currentProduction.DefaultPhoto).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (System.ArgumentNullException e)
                {
                    //Allowing this argument to pass
                }

                db.Entry(currentProduction).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
                return View(production);
        }


        // GET: Productions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Production production = db.Productions.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }
            return View(production);
        }

        // POST: Productions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Production production = db.Productions.Find(id);
            db.Productions.Remove(production);
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
