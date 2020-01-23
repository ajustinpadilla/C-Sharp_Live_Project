﻿using System;
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

        public JsonResult GetProductionDropdown()
        {
            var productions = db.Productions.ToArray();

            return Json(db.Productions.Select(x => new
            {
                id = x.ProductionId,
                title = x.Title
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetCastMemberDropdown()
        {
            var persons = db.CastMembers.ToArray();

            return Json(db.CastMembers.Select(x => new
            {
                id = x.CastMemberID,
                name = x.Name
            }).ToArray(), JsonRequestBehavior.AllowGet);
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
        public ActionResult Create()
        {
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");
            return View();
        }

        // POST: Part/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PartID,Character,Type,Details")] Part part)
        {
            if (ModelState.IsValid)
            {
                db.Parts.Add(part);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(part);
        }

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
            ViewData["CastMembers"] = new SelectList(db.CastMembers.ToList(), "CastMemberId", "Name");

            ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
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

                ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");

                ViewData["CastMembers"] = new SelectList(db.CastMembers.ToList(), "CastMemberID", "Name");



                var production = db.Productions.Find(productionID);
                
                var person = db.CastMembers.Find(castID);

                part.Production = production;
                db.Entry(part.Production).State = EntityState.Modified;
                db.SaveChanges();
                part.Person = person;
                db.Entry(part.Person).State = EntityState.Modified;
                db.SaveChanges();
                db.Entry(part).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(part);
        }

        // GET: Part/Delete/5
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
