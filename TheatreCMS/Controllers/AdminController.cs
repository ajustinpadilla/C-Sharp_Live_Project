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
    }
}

