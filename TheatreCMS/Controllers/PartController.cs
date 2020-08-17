using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Enum;
using TheatreCMS.Models;

namespace TheatreCMS.Controllers
{

    public class PartController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        // GET: Part
        public ActionResult Index()
        {
            var filterProds = db.Productions.Select(i => new SelectListItem
            {
                Value = i.ProductionId.ToString(),
                Text = i.Title
            });
            ViewData["ProductionsID"] = new SelectList(filterProds, "Value", "Text");

            //List<Part> prodsList = new List<Part>();
            //List<int> newProdsList = new List<int>();

            //foreach (Part part in db.Parts)
            //{
            //    if (prodsList.Count == 0)
            //    {
            //        prodsList.Add(part);
            //        newProdsList.Add(part.Production.ProductionId);
            //    }
            //    else if (!newProdsList.Contains(part.Production.ProductionId))
            //    {
            //        prodsList.Add(part);
            //        newProdsList.Add(part.Production.ProductionId);
            //    }
            //}

            var filterCastMem = db.CastMembers.Select(i => new SelectListItem
            {
                Value = i.CastMemberID.ToString(),
                Text = i.Name
            });
            ViewData["CastMembersID"] = new SelectList(filterCastMem, "Value", "Text");

            List<Part> seenPartPosition = new List<Part>();
            List<PositionEnum> seenPosition = new List<PositionEnum>();

            foreach (Part part in db.Parts)
            {
                if (seenPartPosition.Count == 0)
                {
                    seenPartPosition.Add(part);
                    seenPosition.Add(part.Type);
                }
                else if (!seenPosition.Contains(part.Type))
                {
                    seenPartPosition.Add(part);
                    seenPosition.Add(part.Type);
                }
            }

            var filterRoles = seenPartPosition.Select(i => new SelectListItem
            {
                Value = i.Type.ToString(),
                Text = i.Type.ToString()
            });
            ViewData["Roles"] = new SelectList(filterRoles, "Value", "Text");


            return View(db.Parts.ToList());
        }

        [HttpPost]
        public ActionResult Index(int ProductionsID = 0, int CastMembersID = 0 , string Roles = "")
        {
            var filterProds = db.Productions.Select(i => new SelectListItem
            {
                Value = i.ProductionId.ToString(),
                Text = i.Title
            });
            ViewData["ProductionsID"] = new SelectList(filterProds, "Value", "Text");

            var filterCastMem = db.CastMembers.Select(i => new SelectListItem
            {
                Value = i.CastMemberID.ToString(),
                Text = i.Name
            });
            ViewData["CastMembersID"] = new SelectList(filterCastMem, "Value", "Text");

            List<Part> seenPartPosition = new List<Part>();
            List<PositionEnum> seenPosition = new List<PositionEnum>();

            foreach (Part part in db.Parts)
            {
                if (seenPartPosition.Count == 0)
                {
                    seenPartPosition.Add(part);
                    seenPosition.Add(part.Type);
                }
                else if (!seenPosition.Contains(part.Type))
                {
                    seenPartPosition.Add(part);
                    seenPosition.Add(part.Type);
                }
            }

            var filterRoles = seenPartPosition.Select(i => new SelectListItem
            {
                Value = i.Type.ToString(),
                Text = i.Type.ToString()
            });
            ViewData["Roles"] = new SelectList(filterRoles, "Value", "Text");


            if (ProductionsID != 0 && CastMembersID == 0 && Roles == "")
            {
                var myList = db.Parts.Where(i => i.Production.ProductionId == ProductionsID).ToList();

                return View(myList);
            }
            if (ProductionsID == 0 && CastMembersID != 0 && Roles == "")
            {
                var myList = db.Parts.Where(i => i.Person.CastMemberID == CastMembersID).ToList();

                return View(myList);
            }
            if (ProductionsID == 0 && CastMembersID == 0 && Roles != "")
            {
                var myList = db.Parts.Where(i => i.Type.ToString() == Roles).ToList();

                return View(myList);
            }
            if (ProductionsID != 0 && CastMembersID != 0 && Roles == "")
            {
                var myList = db.Parts.Where(i => i.Production.ProductionId == ProductionsID && i.Person.CastMemberID == CastMembersID).ToList();

                return View(myList);
            }
            if (ProductionsID == 0 && CastMembersID != 0 && Roles != "")
            {
                var myList = db.Parts.Where(i => i.Person.CastMemberID == CastMembersID && i.Type.ToString() == Roles).ToList();

                return View(myList);
            }
            if (ProductionsID != 0 && CastMembersID == 0 && Roles != "")
            {
                var myList = db.Parts.Where(i => i.Production.ProductionId == ProductionsID && i.Type.ToString() == Roles).ToList();

                return View(myList);
            }
            if (ProductionsID != 0 && CastMembersID != 0 && Roles != "")
            {
                var myList = db.Parts.Where(i => i.Production.ProductionId == ProductionsID && i.Person.CastMemberID == CastMembersID && i.Type.ToString() == Roles).ToList();

                return View(myList);
            }

            return View(db.Parts.ToList());
        }

        public ActionResult ResetFilters()
        {
            return RedirectToAction("Index");
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
