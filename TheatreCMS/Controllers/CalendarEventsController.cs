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
                seats = x.TicketsAvailable,
                color = x.Color,
                className = x.ClassName,
                someKey = x.SomeKey,
                allDay = false,
                rentalrequestid = x.RentalRequestId,//added for calendar event modals
                productionid = x.ProductionId,//added for calendar event modals
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
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {

            ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId","Title");
            ViewData["RentalRequests"] = new SelectList(db.RentalRequests.ToList(), "RentalRequestId", "Company");
            return View();
            
      
        }

        // POST: CalendarEvents/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for  on, let me show u the 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "EventId,Title,StartDate,EndDate,TicketsAvailable,ProductionId,RentalRequestId")] CalendarEvent calendarEvent)
        {

            
            //ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
            //ViewData["RentalRequests"] = new SelectList(db.RentalRequests.ToList(), "RentalRequestId", "Company");

            //var productionID = Request.Form["Productions"];
            //var rentalID = Request.Form["RentalRequests"];

            //if (productionID == "" && rentalID == "")
            //{
            //    var validationMessage = "Please select a Production or a Rental Request.";
            //    this.ModelState.AddModelError("ProductionId", validationMessage);
            //    this.ModelState.AddModelError("RentalRequestId", validationMessage);
            //}
            //else if (productionID != "" && rentalID == "")
            //{
            //calendarEvent.ProductionId = Convert.ToInt32(productionID);
            //}
            //else if (productionID == "" && rentalID != "")
            //{
            //calendarEvent.RentalRequestId = Convert.ToInt32(rentalID);
            //}
            //else
            //{
            //    var validationMessage = "You can only select either Production or Rental Request, please try again.";
            //    this.ModelState.AddModelError("ProductionId", validationMessage);
            //    this.ModelState.AddModelError("RentalRequestId", validationMessage);
            //}
            var isAjax = Request.IsAjaxRequest();
            if (ModelState.IsValid && !isAjax)
            {
                
                //ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
                //ViewData["RentalRequests"] = new SelectList(db.RentalRequests.ToList(), "RentalRequestId", "Company");

                //if (ViewData["Productions"] != null)


                db.CalendarEvent.Add(calendarEvent);
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine("This call was NOT made from ajax, therefore from Create MVC Page. isAjax: " + isAjax);
                return RedirectToAction("Index");              
            }
            if (ModelState.IsValid && isAjax)
            {
                System.Diagnostics.Debug.WriteLine("This call was made from Ajax, therefore from Add Modal. isAjax: " + isAjax);
                db.CalendarEvent.Add(calendarEvent);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return View(calendarEvent);
        }

        // GET: CalendarEvents/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CalendarEvent calendarEvents = db.CalendarEvent.Find(id);
            
            if (calendarEvents == null)
            {
                return HttpNotFound();
            }
            return View(calendarEvents);
        }

        // POST: CalendarEvents/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "EventId,Title,StartDate,EndDate,TicketsAvailable,ProductionId,RentalRequestId")] CalendarEvent calendarEvents)
        {
            var isAjax = Request.IsAjaxRequest();
            if (ModelState.IsValid && !isAjax)
            {                
                db.Entry(calendarEvents).State = EntityState.Modified;
                db.SaveChanges();
                System.Diagnostics.Debug.WriteLine("This call was NOT made from ajax, therefore from Create MVC Page. isAjax: " + isAjax);
                return RedirectToAction("Index");
            }
            if (ModelState.IsValid && isAjax)
            {
                System.Diagnostics.Debug.WriteLine("This call was made from Ajax, therefore from Add Modal. isAjax: " + isAjax);
                db.Entry(calendarEvents).State = EntityState.Modified;
                db.SaveChanges();               
            }
            return View();
        }

        // GET: CalendarEvents/Delete/5
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            CalendarEvent calendarEvent = db.CalendarEvent.Find(id);
            db.CalendarEvent.Remove(calendarEvent);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        // POST: CalendarEvents Confirm Delete Modal
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public JsonResult DeletingEvent(int id)
        {
            var status = false;
            CalendarEvent eventtodel = db.CalendarEvent.Where(a => a.EventId == id).FirstOrDefault();
            if (eventtodel != null)
            {
                db.CalendarEvent.Remove(eventtodel);
                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
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
