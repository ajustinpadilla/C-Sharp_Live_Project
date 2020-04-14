﻿using System;
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
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Net;

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
        public ActionResult SettingsUpdate(AdminSettings currentSettings)
        {
            string newSettings = JsonConvert.SerializeObject(currentSettings, Formatting.Indented);
            newSettings = newSettings.Replace("T00:00:00", "");
            string filepath = Server.MapPath(Url.Content("~/AdminSettings.json"));
            string oldSettings = null;
             
            using (StreamReader reader = new StreamReader(filepath))
            {
               oldSettings = reader.ReadToEnd();
            }

            dynamic oldJSON = JObject.Parse(oldSettings);
            dynamic newJSON = JObject.Parse(newSettings);

            if (oldJSON.recent_definition.date != newJSON.recent_definition.date)
            {
                UpdateSubscribers(newJSON);
            }
            if (oldJSON.season_productions != newJSON.season_productions)
            {
                UpdateProductions(newJSON);
            }

            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.Write(newSettings);
            }

            return RedirectToAction("Dashboard");

        }

        private void UpdateSubscribers(dynamic newJSON)
        {
            DateTime recentDef = newJSON.recent_definition.date;
            foreach (var subscriber in db.Subscribers)
            {
                if (recentDef >= subscriber.LastDonated)
                {
                    subscriber.RecentDonor = false;
                }
                else
                {
                    subscriber.RecentDonor = true;
                }
            }
            db.SaveChanges();
        }

        private void UpdateProductions(dynamic newJSON)
        {
            int fall = newJSON.season_productions.fall;
            int winter = newJSON.season_productions.winter;
            int spring = newJSON.season_productions.spring;
            foreach (var production in db.Productions)
            {
                if (production.ProductionId == fall || production.ProductionId == winter || production.ProductionId == spring)
                {
                    production.IsCurrent = true;
                }
                else
                {
                    production.IsCurrent = false;
                }
            }
            db.SaveChanges();
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

        public ActionResult UserList()
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            //var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var users = db.Users.ToList();

            foreach (var user in users)
            {
                var role = userManager.GetRoles(user.Id).FirstOrDefault();
                //var roleName = roleManager.FindById(role);
                user.Role = role.ToString();
                
            }
            return View(users);
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

