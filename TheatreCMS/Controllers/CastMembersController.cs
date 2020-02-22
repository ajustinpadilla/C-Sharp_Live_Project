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
            //Creates a dictionary of Id's and Usernames passing it to the View 
             var Users = from a in db.Users select new { a.Id, a.UserName };
            Dictionary<string, string> keyValuePairs = new Dictionary<string, string>();
            foreach (var user in Users)
                keyValuePairs.Add(user.Id, user.UserName);

            ViewBag.Users = keyValuePairs;
            return View(db.CastMembers.ToList());
        }

        

        // GET: CastMembers/Details/5
        public ActionResult Details(int? id)
        {
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CastMember castMember = db.CastMembers.Find(id);
            if (castMember == null)
            {
                return HttpNotFound();
            }
            //Passes The Username of the currently selected cast member to the model
            if (castMember.CastMemberPersonID != null)
                ViewBag.CurrentUser = db.Users.Find(castMember.CastMemberPersonID).UserName;
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
        public ActionResult Create([Bind(Include = "CastMemberID,Name,YearJoined,MainRole,Bio,Photo,CurrentMember,CastMemberPersonId,AssociateArtist,EnsembleMember,CastYearLeft,DebutYear")] CastMember castMember, HttpPostedFileBase file)
        {
            
            ModelState.Remove("CastMemberPersonID");

            //Extract the Guid as type String from user's selected User (from SelectList)
            string userId = Request.Form["dbUsers"].ToString();

            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    byte[] photo = ImageUploadController.ImageBytes(file, out string _64);
                    castMember.Photo = photo;
                }

                ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");

                castMember.CastMemberPersonID = db.Users.Find(userId).Id;

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
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");
            
            return View(castMember);
        }

        // POST: CastMembers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CastMemberID,Name,YearJoined,MainRole,Bio,Photo,CurrentMember,AssociateArtist,EnsembleMember,CastYearLeft,DebutYear")] CastMember castMember, HttpPostedFileBase file)
        {

            ModelState.Remove("CastMemberPersonID");
            string userId = Request.Form["dbUsers"].ToString();
            if (ModelState.IsValid)
            {
                var currentCastMember = db.CastMembers.Find(castMember.CastMemberID);
                byte[] oldPhoto = currentCastMember.Photo;

                currentCastMember.Name = castMember.Name;
                currentCastMember.YearJoined = castMember.YearJoined;
                currentCastMember.MainRole = castMember.MainRole;
                currentCastMember.Bio = castMember.Bio;
                currentCastMember.CurrentMember = castMember.CurrentMember;
                currentCastMember.AssociateArtist = castMember.AssociateArtist;
                currentCastMember.EnsembleMember = castMember.EnsembleMember;
                currentCastMember.CastYearLeft = castMember.CastYearLeft;
                currentCastMember.DebutYear = castMember.DebutYear;

                if (file != null && file.ContentLength > 0)
                {
                    byte[] newPhoto = ImageUploadController.ImageBytes(file, out string _64);
                    currentCastMember.Photo = newPhoto;
                }
                else
                {
                    currentCastMember.Photo = oldPhoto;
                }
                //castMember.CastMemberPersonID = db.Users.Find(userId).Id;
                //db.Entry(castMember).State = EntityState.Modified;
                db.Entry(currentCastMember).State = EntityState.Modified;
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
            if (castMember.CastMemberPersonID != null)
                ViewBag.CurrentUser = db.Users.Find(castMember.CastMemberPersonID).UserName;
            return View(castMember);
        }

        // POST: CastMembers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? id)
        {
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
