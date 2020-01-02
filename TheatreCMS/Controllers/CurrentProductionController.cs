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
    public class CurrentProductionController : Controller
    {
        private TheatreCMSContext db = new TheatreCMSContext();

        // GET: CurrentProduction
        public ActionResult Index()
        {
            return View(db.CurrentProductions.ToList());
        }

        // GET: CurrentProduction/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentProduction currentProduction = db.CurrentProductions.Find(id);
            if (currentProduction == null)
            {
                return HttpNotFound();
            }
            return View(currentProduction);
        }

        // GET: CurrentProduction/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CurrentProduction/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ProductionId,Title,Playwright,OpeningDay,ClosingDay,Image,ShowtimeEve,ShowtimeMat,TicketLink")] CurrentProduction currentProduction)
        {
            if (ModelState.IsValid)
            {
                db.CurrentProductions.Add(currentProduction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(currentProduction);
        }

        // GET: CurrentProduction/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentProduction currentProduction = db.CurrentProductions.Find(id);
            if (currentProduction == null)
            {
                return HttpNotFound();
            }
            return View(currentProduction);
        }

        // POST: CurrentProduction/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ProductionId,Title,Playwright,OpeningDay,ClosingDay,Image,ShowtimeEve,ShowtimeMat,TicketLink")] CurrentProduction currentProduction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(currentProduction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(currentProduction);
        }

        // GET: CurrentProduction/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CurrentProduction currentProduction = db.CurrentProductions.Find(id);
            if (currentProduction == null)
            {
                return HttpNotFound();
            }
            return View(currentProduction);
        }

        // POST: CurrentProduction/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CurrentProduction currentProduction = db.CurrentProductions.Find(id);
            db.CurrentProductions.Remove(currentProduction);
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
