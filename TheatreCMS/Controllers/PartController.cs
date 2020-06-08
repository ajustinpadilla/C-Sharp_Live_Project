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

    public class PartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        // GET: Part
        public ActionResult Index()
        {
            return View(db.Parts.ToList());
        }


        // GET: Part/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);

        }

        // GET: Part/Create
        [Authorize(Roles ="Admin")]
        public ActionResult Create()
        {
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");

            ViewData["CastMembers"] = new SelectList(db.CastMembers.ToList(), "CastMemberId", "Name");

            ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
            return View();
        }

        // POST: Part/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PartID,Production,Person,Character,Type,Details")] Part part)
        {
            int productionID = Convert.ToInt32(Request.Form["Productions"]);
            int castID = Convert.ToInt32(Request.Form["CastMembers"]);

            if (ModelState.IsValid)
            {
                var person = db.CastMembers.Find(castID);
                var production = db.Productions.Find(productionID);
                
                part.Production = production;
                part.Person = person;
                db.Parts.Add(part);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(part);
        }

        [Authorize(Roles = "Admin")]
        // GET: Part/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                 return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            Part person = db.Parts.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            Part production = db.Parts.Find(id);
            if (production == null)
            {
                return HttpNotFound();
            }

			ViewData["Productions"] = new SelectList(db.Productions, "ProductionId", "Title", part.Production.ProductionId);
			
			ViewData["CastMembers"] = new SelectList(db.CastMembers, "CastMemberId", "Name", part.Person.CastMemberID);

            return View(part);
        }

        // POST: Part/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PartID,Production,Person,Character,Type,Details")] Part part)
        {

            int castID = Convert.ToInt32(Request.Form["CastMembers"]);
            int productionID = Convert.ToInt32(Request.Form["Productions"]);
            if (ModelState.IsValid)
            {
                var currentPart = db.Parts.Find(part.PartID);
                    currentPart.Character = part.Character;
                    currentPart.Type = part.Type;
                    currentPart.Details = part.Details;

                ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");

                ViewData["CastMembers"] = new SelectList(db.CastMembers.ToList(), "CastMemberID", "Name");

				var production = db.Productions.Find(productionID);
                
                var person = db.CastMembers.Find(castID);
                
                currentPart.Production = production;
                db.Entry(currentPart.Production).State = EntityState.Modified;
                db.SaveChanges();
                currentPart.Person = person;
                db.Entry(currentPart.Person).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(currentPart).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(part);
        }

        // GET: Part/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Part part = db.Parts.Find(id);
            if (part == null)
            {
                return HttpNotFound();
            }
            return View(part);
        }

        // POST: Part/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Part part = db.Parts.Find(id);
            db.Parts.Remove(part);
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
