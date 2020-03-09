using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Models;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;
using static TheatreCMS.Models.AdminSettings;
using Newtonsoft.Json.Linq;
using TheatreCMS.Helpers;

namespace TheatreCMS.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin
        public ActionResult Index()
        {
            
            return View();
        }
        public ActionResult Dashboard()
        {
            #region ViewData
            List<SelectListItem> productionList = GetSelectListItems();
            ViewData["ProductionList"] = productionList;
            #endregion
            AdminSettings current = new AdminSettings();
            current = AdminSettingsReader.CurrentSettings();

            return View(current);
        }

        // ***OLD "SettingsUpdate" method, can be deleted once NEW one with tweaks (see below) has been reviewed - 2/18/2020, jc***
        //
        //[HttpPost]
        //public ActionResult SettingsUpdate(int Current_Season, int Winter, int Fall, int Spring, int Span, DateTime Date, int Onstage)
        //{
        //    AdminSettings settings = new AdminSettings
        //    {
        //        current_season = Current_Season,
        //        season_productions = new AdminSettings.seasonProductions
        //        {
        //            winter = Winter,
        //            fall = Fall,
        //            spring = Spring
        //        },
        //        recent_definition = new AdminSettings.recentDefinition
        //        {
        //            span = Span,
        //            date = Date,
        //        },
        //        onstage = Onstage
        //    };
        //    string newSettings = JsonConvert.SerializeObject(settings);
        //    string filepath = Server.MapPath(Url.Content("~/AdminSettings.json"));
        //    using (StreamWriter writer = new StreamWriter(filepath))
        //    {
        //        writer.Write(newSettings);
        //    }
        //    return RedirectToAction("Dashboard");
        //}

        [HttpPost]
        public ActionResult SettingsUpdate(AdminSettings adminSet, seasonProductions seasonProd, recentDefinition recentDef)
        {
            AdminSettings settings = new AdminSettings();
            settings = adminSet;
            settings.season_productions = seasonProd;
            settings.recent_definition = recentDef;

            string newSettings = JsonConvert.SerializeObject(settings, Formatting.Indented);
            newSettings = newSettings.Replace("T00:00:00", "");
            string filepath = Server.MapPath(Url.Content("~/AdminSettings.json"));
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.Write(newSettings);
            }

            return RedirectToAction("Dashboard");

        }

        public ActionResult DonorList()
        {

            var donor = from z in db.Subscribers
                        where z.RecentDonor == true
                        select z;


            return View(donor.ToList());
        }

        public ActionResult AddShows()
        {
            return View();
        }

        public List<SelectListItem> GetSelectListItems()
        {
            //Create a list of productions sorted by season(int)
            var SortedProductions = db.Productions.ToList().OrderByDescending(prod => prod.Season).ToList();

            // Create an empty list to hold result
            var selectList = new List<SelectListItem>();

            // For each string in the 'elements' variable, create a new SelectListItem object
            // that has both its Value and Text properties set to a particular value.
            foreach (var production in SortedProductions)
            {
                selectList.Add(new SelectListItem
                {
                    Value = production.ProductionId.ToString(),
                    Text = production.Title
                });
            }

            return selectList;
        }
    }
}


