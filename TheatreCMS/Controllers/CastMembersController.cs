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
    public class CastMembersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: CastMembers
        public ActionResult Index()
        {
             var users = db.Users.ToArray();
            var user = from a in users select new { a.Id, a.UserName };
            List<string> result1 = new List<string>();
            List<string> result2 = new List<string>();
            foreach (var item in user)
            {
                result1.Add(item.Id);
                result2.Add(item.UserName);
            }
            ViewData["Ids"] = result1;
            ViewData["Names"] = result2;
            return View(db.CastMembers.ToList());
        }
        public JsonResult GetAllUsersDropdown()
        {
            var users = db.Users.ToArray();
            
            var result = Json(db.Users.Select(x => new
            {
                id = x.Id,
                UserName = x.UserName,
                
            }).ToArray(), JsonRequestBehavior.AllowGet);
            
            return result;
        }
        

        // GET: CastMembers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CastMember castMember = db.CastMembers.Find(id);
            if (castMember == null)
            {
                return HttpNotFound();
            }
            var users = db.Users.ToArray();
            var user = from a in users select new { a.Id, a.UserName };
            foreach (var item in user)
            {
                if (item.Id.ToString() == castMember.CastMemberPersonID && castMember.CastMemberID == id)
                    ViewData["userName"] = item.UserName;
            }

            return View(castMember);
        }

        // GET: CastMembers/Create
        public ActionResult Create()
        {
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");
            
            return View();
        }

        // POST: CastMembers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CastMemberID,Name,YearJoined,MainRole,Bio,Photo,CurrentMember")] CastMember castMember)
        {
            string userId = Request.Form["dbUsers"].ToString();
            if (ModelState.IsValid)
            {
                db.CastMembers.Add(castMember);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(castMember);
        }

        // GET: CastMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CastMember castMember = db.CastMembers.Find(id);
            if (castMember == null)
            {
                return HttpNotFound();
            }
            var users = db.Users.ToArray();
            var user = from a in users select new { a.Id, a.UserName };
            foreach (var item in user)
            {
                if (item.Id.ToString() == castMember.CastMemberPersonID && castMember.CastMemberID == id)
                    ViewData["userName"] = item.UserName;
            }
            return View(castMember);
        }

        // POST: CastMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CastMemberID,Name,YearJoined,MainRole,Bio,Photo,CurrentMember")] CastMember castMember)
        {
            string userId = Request.Form["dbUsers"].ToString();
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");

            if (ModelState.IsValid)
            {
                db.Entry(castMember).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(castMember);
        }

        // GET: CastMembers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CastMember castMember = db.CastMembers.Find(id);
            if (castMember == null)
            {
                return HttpNotFound();
            }
            var users = db.Users.ToArray();
            var user = from a in users select new { a.Id, a.UserName };
            foreach (var item in user)
            {
                if (item.Id.ToString() == castMember.CastMemberPersonID && castMember.CastMemberID == id)
                    ViewData["userName"] = item.UserName;
            }
            return View(castMember);
        }

        // POST: CastMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            string userId = Request.Form["dbUsers"].ToString();
            

            CastMember castMember = db.CastMembers.Find(id);
            db.CastMembers.Remove(castMember);
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
