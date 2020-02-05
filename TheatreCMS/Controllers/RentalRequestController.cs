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
    public class RentalRequestController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: RentalRequest
        public ActionResult Index()
        {
            return View(db.RentalRequests.ToList());
        }

        public void RentalAddCalendar(RentalRequest rentalRequest)
        {
            CalendarEvent calendar = new CalendarEvent();

            calendar.RentalRequestId = rentalRequest.RentalRequestId;
            calendar.Title = rentalRequest.Company;
            calendar.StartDate = rentalRequest.StartTime;
            calendar.EndDate = rentalRequest.EndTime;
            db.CalendarEvents.Add(calendar);
            db.SaveChanges();
            
        }

        public void RentalEditCalendar(RentalRequest rentalRequest, CalendarEvent calendarEvent)
        {
            //CalendarEvent calendar = new CalendarEvent();
            //int calendarId = Convert.ToInt32(calendarEvent);
            //CalendarEvent calendar = db.CalendarEvents.Where(x => x.RentalRequestId == rentalRequest.RentalRequestId);
            //int rental = rentalRequest.RentalRequestId;
            //CalendarEvent calendar = db.CalendarEvents.Where(rentalRequest.RentalRequestId);

            int calendar = Convert.ToInt32(calendarEvent.RentalRequestId);

            if (rentalRequest.RentalRequestId == calendar)
            {
                //calendar.RentalRequestId = rentalRequest.RentalRequestId;
                calendarEvent.Title = rentalRequest.Company;
                calendarEvent.StartDate = rentalRequest.StartTime;
                calendarEvent.EndDate = rentalRequest.EndTime;
                db.Entry(calendarEvent).State = EntityState.Modified;
                db.SaveChanges();
            }
            else
            {
                RentalAddCalendar(rentalRequest);
            }
        }

        public void RentalDeleteCalendar(RentalRequest rentalRequest)
        {
            CalendarEvent calendar = db.CalendarEvent.Find(rentalRequest.RentalRequestId);

            db.CalendarEvent.Remove(calendar);
            db.SaveChanges();
            
        }
        

        // GET: RentalRequest/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalRequest rentalRequest = db.RentalRequests.Find(id);
            if (rentalRequest == null)
            {
                return HttpNotFound();
            }
            return View(rentalRequest);
        }

        // GET: RentalRequest/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RentalRequest/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "RentalRequestId,ContactPerson,Company,StartTime,EndTime,ProjectInfo,Requests,RentalCode,Accepted,ContractSigned")] RentalRequest rentalRequest)
        {
            if (ModelState.IsValid)
            {
                db.RentalRequests.Add(rentalRequest);
                var randomNum = new Random();
                int codeNum = randomNum.Next(10000, 99999);
                rentalRequest.RentalCode = codeNum;
                db.SaveChanges();
                if (rentalRequest.Accepted == true)
                {
                    RentalAddCalendar(rentalRequest);
                }
                return RedirectToAction("Index");
            }

            return View(rentalRequest);
        }

        // GET: RentalRequest/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalRequest rentalRequest = db.RentalRequests.Find(id);
            if (rentalRequest == null)
            {
                return HttpNotFound();
            }
            return View(rentalRequest);
        }

        // POST: RentalRequest/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "RentalRequestId,ContactPerson,Company,StartTime,EndTime,ProjectInfo,Requests,RentalCode,Accepted,ContractSigned")] RentalRequest rentalRequest, CalendarEvent calendarEvent)
        {
            if (ModelState.IsValid)
            {
                db.Entry(rentalRequest).State = EntityState.Modified;
                db.SaveChanges();
                if (rentalRequest.Accepted == true)
                {
                    RentalEditCalendar(rentalRequest, calendarEvent);
                }
                return RedirectToAction("Index");
            }
            return View(rentalRequest);
        }

        // GET: RentalRequest/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RentalRequest rentalRequest = db.RentalRequests.Find(id);
            if (rentalRequest == null)
            {
                return HttpNotFound();
            }
            return View(rentalRequest);
        }

        // POST: RentalRequest/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RentalRequest rentalRequest = db.RentalRequests.Find(id);
            if (rentalRequest.Accepted == true)
            {
                RentalDeleteCalendar(rentalRequest);
            }
            db.RentalRequests.Remove(rentalRequest);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RequestConfirm()
        {
            return View();
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
