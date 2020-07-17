using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TheatreCMS.Models;

namespace TheatreCMS.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Messages
        public ActionResult Index()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            ApplicationUser currentUser = userManager.FindById(User.Identity.GetUserId());
            var filteredUsers = db.Users.OrderBy(i => i.LastName).ThenBy(i => i.FirstName);
            if (!User.IsInRole("Admin"))
            {
                filteredUsers = (IOrderedQueryable<ApplicationUser>)filteredUsers.Where(i => i.Role == "Admin");
            }
            var UserList = filteredUsers.Select(i => new SelectListItem
            {
                Value = i.Id,
                Text = i.LastName + ", " + i.FirstName
            });
            ViewData["listOfUsers"] = new SelectList(UserList, "Value", "Text");
            ViewData["currentUserId"] = currentUser.Id;
            ViewData["currentUserName"] = currentUser.LastName + ", " + currentUser.FirstName;
            return View(db.Messages.Where(i => i.SenderId == currentUser.Id || i.RecipientId == currentUser.Id).OrderByDescending(i => i.SentTime).ToList());
        }

        // GET: Messages/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // GET: Messages/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Messages/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MessageId,RecipientId,IsViewed,ParentId,Subject,Body")] Message message)
        {
            if (ModelState.IsValid)
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
                ApplicationUser currentUser = userManager.FindById(User.Identity.GetUserId());
                message.SenderId = currentUser.Id;
                message.SentTime = DateTime.Now;
                db.Messages.Add(message);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(message);
        }

        // GET: Messages/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Message message = db.Messages.Find(id);
        //    if (message == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(message);
        //}

        // POST: Messages/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //In this particular controller, Edit is only called to update IsViewed via AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int MessageId)
        {
            Message message = db.Messages.Find(MessageId);
            if (message == null)
            {
                return HttpNotFound();
            }
            message.IsViewed = DateTime.Now;
            db.Entry(message).State = EntityState.Modified;
            db.SaveChanges();
            return Json(new { success = true });

        }

        // GET: Messages/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Message message = db.Messages.Find(id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        // POST: Messages/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Message message = db.Messages.Find(id);
            db.Messages.Remove(message);
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
