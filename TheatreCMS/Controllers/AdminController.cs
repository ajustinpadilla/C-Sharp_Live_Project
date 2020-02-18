using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TheatreCMS.Models;
using Newtonsoft.Json;
using System.IO;

namespace TheatreCMS.Controllers
{
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

            return View();
        }

        [HttpPost]
        public ActionResult SettingsUpdate(int Current_Season, int Winter, int Fall, int Spring, int Span, DateTime Date, int Onstage)
        {
            AdminSettings settings = new AdminSettings
            {
                current_season = Current_Season,
                season_productions = new AdminSettings.seasonProductions
                {
                    winter = Winter,
                    fall = Fall,
                    spring = Spring
                },
                recent_definition = new AdminSettings.recentDefinition
                {
                    span = Span,
                    date = Date
                },
                onstage = Onstage
            };
            
            string newSettings = JsonConvert.SerializeObject(settings);
            
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
    }
}