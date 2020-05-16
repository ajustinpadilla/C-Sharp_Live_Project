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
using System.Web.UI.WebControls;
using System.Diagnostics;

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
            ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
            ViewData["RentalRequests"] = new SelectList(db.RentalRequests.ToList(), "RentalRequestId", "Company");

            var productionID = Request.Form["Productions"];
            var rentalID = Request.Form["RentalRequests"];

            if (productionID == "" && rentalID == "")
            {
                var validationMessage = "Please select a Production or a Rental Request.";
                this.ModelState.AddModelError("ProductionId", validationMessage);
                this.ModelState.AddModelError("RentalRequestId", validationMessage);
            }
            else if (productionID != "" && rentalID == "")
            {
                calendarEvent.ProductionId = Convert.ToInt32(productionID);
            }
            else if (productionID == "" && rentalID != "")
            {
                calendarEvent.RentalRequestId = Convert.ToInt32(rentalID);
            }
            else
            {
                var validationMessage = "You can only select either Production or Rental Request, please try again.";
                this.ModelState.AddModelError("ProductionId", validationMessage);
                this.ModelState.AddModelError("RentalRequestId", validationMessage);
            }

            if (ModelState.IsValid)
            {
               // ViewData["Productions"] = new SelectList(db.Productions.ToList(), "ProductionId", "Title");
               // ViewData["RentalRequests"] = new SelectList(db.RentalRequests.ToList(), "RentalRequestId", "Company");

                //if (ViewData["Productions"] != null)
              

                db.CalendarEvent.Add(calendarEvent);
                db.SaveChanges();
                return RedirectToAction("Index");
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
            if (ModelState.IsValid)
            {                
                db.Entry(calendarEvents).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    
        // GET: CalendarEvents/BulkAdd

        [Authorize(Roles = "Admin")]
        public ActionResult BulkAdd()
        {
            ViewData["Productions"] = new SelectList(db.Productions.OrderByDescending(x => x.Season).ToList(), "ProductionId","Title");
            ViewData["Times"] = GetTimeIntervals();
            return View();
        }
        
        [Authorize(Roles = "Admin")]
        public ActionResult GetDates(int productionId = 0)
        {
            int id = Convert.ToInt32(productionId);
            var query = (from production in db.Productions
                         where production.ProductionId == id
                         select new { production.OpeningDay, production.ClosingDay });
            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(query), JsonRequestBehavior.AllowGet);
            
        }

        public List<string> GetTimeIntervals()
        {
            List<string> timeIntervals = new List<string>();
            TimeSpan startTime = new TimeSpan(8, 0, 0);
            DateTime startDate = new DateTime(DateTime.MinValue.Ticks); // Date to be used to get shortTime format.
            for (int i = 0; i < 29; i++)
            {
                int minutesToBeAdded = 30 * i;      // Increasing minutes by 30 minutes interval
                TimeSpan timeToBeAdded = new TimeSpan(0, minutesToBeAdded, 0);
                TimeSpan t = startTime.Add(timeToBeAdded);
                DateTime result = startDate + t;
                timeIntervals.Add(result.ToShortTimeString());      // Use Date.ToShortTimeString() method to get the desired format                
            }
            return timeIntervals;
        }
    }
}
