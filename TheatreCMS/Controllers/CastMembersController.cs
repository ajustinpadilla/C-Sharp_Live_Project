using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Diagnostics;   // For testing purposes
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Models;
//using Microsoft.AspNet.Identity;

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

            // ModelState error to ensure that A user cannot be assigned to multiple cast members.
            // If the CastMemberUserID IS assigned for this user, that means that this user is assigned
            // to another Cast Member: add the ModelState error.
            if (!string.IsNullOrEmpty(userId) && db.Users.Find(userId).CastMemberUserID != 0)
                ModelState.AddModelError("CastMemberPersonID", $"{db.Users.Find(userId).UserName} already has a cast member profile");

            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    byte[] photo = ImageUploadController.ImageBytes(file, out string _64);
                    castMember.Photo = photo;
                }

                //ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");

                if (!string.IsNullOrEmpty(userId))
                {
                    castMember.CastMemberPersonID = db.Users.Find(userId).Id;
                }

                db.CastMembers.Add(castMember);
                db.SaveChanges();

                // If a user was selected, update the CastMemberUserID column in the User table with CastMemberPersonID.
                if (!string.IsNullOrEmpty(userId))
                {
                    // Use the recently added CastMemberPersonID to find the selected User
                    var selectedUser = db.Users.Find(castMember.CastMemberPersonID);

                    // Update the User's Cast Member Id column with castMemberId
                    selectedUser.CastMemberUserID = castMember.CastMemberID;

                    // Save the changes
                    db.Entry(selectedUser).State = EntityState.Modified;
                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }
            else  // This viewdata is required for the create view
            {
                ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName");
            }

            return View(castMember);
        }

        // GET: CastMembers/Edit/5
        public ActionResult Edit(int? id)
        {
            // STORY REQUIREMENT: The Edit function should check if the User has been modified(i.e. if the User has been added, 
            // removed, or changed) and set or reset the User(or Users) value for the appropriate CastMemberId.
           
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            CastMember castMember = db.CastMembers.Find(id);

            if (castMember == null)
            {
                return HttpNotFound();
            }

            // Check if the user associated with this CastMember is deleted.  If so, reset value to null.
            // If the user is already null, don't look for matching ids and don't update the database.
            if (castMember.CastMemberPersonID != null && db.Users.Where(x => x.Id == castMember.CastMemberPersonID).Count() <= 0)
            {
                Debug.WriteLine("\n\n\nDELETED USER DETECTED, Reset Username to N / A\n\n\n");
                castMember.CastMemberPersonID = null;

                db.Entry(castMember).State = EntityState.Modified;
                db.SaveChanges();
            }
            // ***still need to get existing value to display as a default in drop-down list***
            ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName", castMember.CastMemberPersonID);
            
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

            // ModelState error to ensure that A user cannot be assigned to multiple cast members.
            // If the userId is null, castMemberId is 0, or previous castMemberId is the same as the new CastMemberId,
            // Then don't add the model error.
            int oldCastMemberId = db.CastMembers.Find(castMember.CastMemberID).CastMemberID;    // These variables are here to make the 
            int newCastMemberId = db.Users.Find(userId).CastMemberUserID;                       // if statement below easier to read.
            if (!(string.IsNullOrEmpty(userId) || newCastMemberId == 0 || oldCastMemberId == newCastMemberId))
                ModelState.AddModelError("CastMemberPersonID", $"{db.Users.Find(userId).UserName} already has a cast member profile");

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

                // Variables to detect whether or not the username was changed.  If so, update the User db.  If not, ignore.
                string previousUserName = "";
                string newUserName = "";
                bool previousUserIsNull = false;
                bool newUserIsNull = false;

                // If the current cast member has no User associated with it, set previousUserIsNull to true.
                // Else, get the Username of that User.
                if (string.IsNullOrEmpty(currentCastMember.CastMemberPersonID))
                    previousUserIsNull = true;
                else
                    previousUserName = db.Users.Find(currentCastMember.CastMemberPersonID).UserName;

                // If the User selected is null, set newUserIsNull to true.
                // Else, get the Username of that User.
                if (string.IsNullOrEmpty(userId))
                    newUserIsNull = true;
                else
                    newUserName = db.Users.Find(userId).UserName;

                // If the previous user and the new user have valid Id's, then the User has changed.
                // null null ==> Don't Update | null !null ==> Update | !null null ==> Update | !null !null ==> Update
                // If the Username was changed, set the previous User's CastMemberUserID to 0.
                if ((previousUserName != newUserName) && !(previousUserIsNull && newUserIsNull))
                {
                    Debug.WriteLine("\n\nThe Usernames changed!!\n\n");
                    // Set the previous User's CastMemberUserId to 0 if that User exists.
                    if (!previousUserIsNull)
                        db.Users.Find(currentCastMember.CastMemberPersonID).CastMemberUserID = 0;

                    // Only do this if there was a User selected.  Links the Cast Member and
                    // User together by updated their associated databases.
                    if (!string.IsNullOrEmpty(userId))
                    {
                        // Link the Cast Member to the User
                        currentCastMember.CastMemberPersonID = db.Users.Find(userId).Id;

                        // Get the selected User.
                        var selectedUser = db.Users.Find(userId);

                        // Update the User's Cast Member Id column with castMemberId
                        selectedUser.CastMemberUserID = castMember.CastMemberID;

                        // Save the changes
                        db.Entry(selectedUser).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                    // When there is no User selected, remove the reference to the User for this cast member.
                    else
                        currentCastMember.CastMemberPersonID = null;
                }

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
            else
            {
                ViewData["dbUsers"] = new SelectList(db.Users.ToList(), "Id", "UserName", castMember.CastMemberPersonID);
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

            // Before the cast member is removed.  Set the associated User CastMemberUserId to 0 if a User was assigned.
            if (castMember.CastMemberPersonID != null)
                db.Users.Find(castMember.CastMemberPersonID).CastMemberUserID = 0;

            db.CastMembers.Remove(castMember);

            // PROBABLY NOT NEEDED

            // Remove the ModelState Error when the cast member is deleted.  Now the user associated with this
            // Deleted Cast Member can be assigned without creating an error.
            //string username = db.Users.Where(x => x.CastMemberUserID == id).First().UserName;
            //if (ModelState.ContainsKey(username))
            //    ModelState[username].Errors.Clear();

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
