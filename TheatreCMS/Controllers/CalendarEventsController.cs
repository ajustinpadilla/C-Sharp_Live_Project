using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Windows.Documents;
using TheatreCMS.Models;
using System.Web.Mvc.Html;

namespace TheatreCMS.Controllers
{
    public class CalendarEventsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CalendarEvents
        public ActionResult Index()
        {
            return View(db.CalendarEvent.ToList());
        }

        public JsonResult GetCalendarEvents()
        {
            var events = db.CalendarEvent.ToArray();
           
            return Json(db.CalendarEvent.Select(x => new
            {
                id = x.EventId,
                title = x.Title,
                start = x.StartDate,
                end = x.EndDate,
                color = x.Color,
                className = x.ClassName,
                someKey = x.SomeKey,
                allDay = false
            }).ToArray(), JsonRequestBehavior.AllowGet);
        }

        // GET: CalendarEvents/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.CalendarEvent.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        // GET: CalendarEvents/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CalendarEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EventId,Title,StartDate,EndDate,ProductionId,RentalRequestId")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.CalendarEvent.Add(calendarEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(calendarEvent);
        }

        // GET: CalendarEvents/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.CalendarEvent.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        // POST: CalendarEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EventId,Title,StartDate,EndDate,ProductionId,RentalRequestId")] CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(calendarEvent).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(calendarEvent);
        }

        // GET: CalendarEvents/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvent = db.CalendarEvent.Find(id);
            if (calendarEvent == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvent);
        }

        // POST: CalendarEvents/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalendarEvent calendarEvent = db.CalendarEvent.Find(id);
            db.CalendarEvent.Remove(calendarEvent);
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
