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
using System.Web.UI.WebControls.Expressions;
using System.Data.Entity;

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
            AdminSettings current = AdminSettingsReader.CurrentSettings();
            UpdateAdminSettings();
            AddViewData(current);
            return View(current);
        }

        //Section to pass ViewData to controller.
        public void AddViewData(AdminSettings currentSettings)
        {
            currentSettings = AdminSettingsReader.CurrentSettings();
            List<SelectListItem> productionList = GetSelectListItems();
            #region ViewData
            ViewData["ProductionList"] = productionList;
            ViewData["CurrentProductionList"] = GetCurrentProductions();
            ViewData["nextSeasonNotification"] = NextSeasonNotification();
            #endregion
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


            //AUTO CALCULATION OF SEASON CODE
            public int AutoCalculateCurrentSeason()
        {
            DateTime season = new DateTime(1996, 6, 1);
            DateTime now = DateTime.Now;
            int currentSeason = now.Year - season.Year;

            if (now.Month < season.Month || (now.Month == season.Month && now.Day < season.Day))
                currentSeason--;

            return currentSeason;
        }
        //END AUTO CALCULATION OF SEASON CODE


        [HttpPost]
        public ActionResult SettingsUpdate(AdminSettings currentAdminSettings)
        {
            currentAdminSettings.models_missing_photos = AdminSettingsReader.CurrentSettings().models_missing_photos;
            string newSettings = JsonConvert.SerializeObject(currentAdminSettings, Formatting.Indented);

            newSettings = newSettings.Replace("T00:00:00", "");
            string filepath = Server.MapPath(Url.Content("~/AdminSettings.json"));
            string oldSettings = null;

            using (StreamReader reader = new StreamReader(filepath))
            {
                oldSettings = reader.ReadToEnd();
            }
            dynamic oldJSON = JObject.Parse(oldSettings);
            dynamic newJSON = JObject.Parse(newSettings);

            int thisSeason = AutoCalculateCurrentSeason();
            if (newJSON.current_season != thisSeason)
            {
                ModelState.AddModelError("current_season", "You have entered the incorrect Season number.");
            }
            if (oldJSON.season_productions != newJSON.season_productions)
            {
                UpdateProductions(newJSON);
            }
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.Write(newSettings);
            }
            UpdateAdminSettings();
            currentAdminSettings = AdminSettingsReader.CurrentSettings();
            AddViewData(currentAdminSettings);

            UpdateSubscribers();

            return View("Dashboard", currentAdminSettings);
        }
        //Sends message to view based on when the season is going to change.
        public string NextSeasonNotification()
        {
            DateTime season = new DateTime(1996, 6, 1);
            DateTime now = DateTime.Now.Date;
            int nextSeasonNumber = AutoCalculateCurrentSeason() + 1;
            DateTime nextSeason = new DateTime(season.Year + nextSeasonNumber, season.Month, season.Day);
            TimeSpan daysBeforeNextSeason = nextSeason.Subtract(now);

            if (now > nextSeason.AddMonths(-3))
            {
                //If season is going to change in 2 weeks or less, gives text on how many days until season change and what season its changing too.
                if (daysBeforeNextSeason.Days < 15)
                {
                    string nextSeasonNotification = String.Format("Season {0} begins in {1} days", nextSeasonNumber, daysBeforeNextSeason.Days);
                    return nextSeasonNotification;
                }
                //If season changes in 2 weeks - 3 months says what seasons its going to change too and on what date.
                else
                {
                    string nextSeasonNotification = String.Format("Season {0} begins on {1}", nextSeasonNumber, nextSeason.ToShortDateString());
                    return nextSeasonNotification;
                }
            }
            //If the next season is further than 3 months don't display anything.
            else
            {
                string nextSeasonNotification = "";
                return nextSeasonNotification;
            }
        }

        //Updates Changes in database depending on inputs from the Admin Settings form for recent_definition.
        private void UpdateSubscribers()
        {
            // Retrieve Admin Settings
            dynamic adminSettings = AdminSettingsReader.CurrentSettings();

            // Init variable
            DateTime recentDef = DateTime.Now;

            // Check and see what the recent definition is
            // Then set recentDef to the correct DateTime
            // If there is no selection, recentDef will be set to now
            // No recent subs will be displayed unless they donated at the exact time
            if (!adminSettings.recent_definition.bUsingSpan)
            {
                recentDef = adminSettings.recent_definition.date;
            }
            else if (adminSettings.recent_definition.bUsingSpan)
            {
                recentDef.AddMonths(-Convert.ToInt32(adminSettings.recent_definition.span));
            }

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

        //Updates Changes in database depending on inputs from the Admin Settings form for season productions.
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

        public ActionResult UserList(string requestedSort = "UserName", string currentSortOrder = "")
        {
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            var users = db.Users.AsNoTracking().ToList();
            foreach (var user in users)
            {
                var role = userManager.GetRoles(user.Id).FirstOrDefault();
                user.Role = role.ToString();
            }

            /// start of sort-order logic
            string newSortOrder = currentSortOrder;
            
            if (newSortOrder.Contains(requestedSort))
            {
                if (newSortOrder.Contains("_desc"))
                {
                    newSortOrder = newSortOrder.Replace("_desc", "");
                }
                else
                {
                    newSortOrder += "_desc";
                }
            }
            else
            {
                newSortOrder = requestedSort;
            }

            switch (newSortOrder)
            {
                case "UserName":
                    // do some sorting
                    users = users.OrderBy(user => user.UserName).ToList();
                    break;
                case "UserName_desc":
                    users = users.OrderByDescending(user => user.UserName).ToList();
                    break;
                case "FirstName":
                    // do some sorting
                    users = users.OrderBy(user => user.FirstName).ToList();
                    break;
                case "FirstName_desc":
                    users = users.OrderByDescending(user => user.FirstName).ToList();
                    break;
                case "LastName":
                    // do some sorting
                    users = users.OrderBy(user => user.LastName).ToList();
                    break;
                case "LastName_desc":
                    users = users.OrderByDescending(user => user.LastName).ToList();
                    break;
                case "Role":
                    // do some sorting
                    users = users.OrderBy(user => user.Role).ToList();
                    break;
                case "Role_desc":
                    users = users.OrderByDescending(user => user.Role).ToList();
                    break;
                default:
                    // if it's not a recognized case (sort order)
                    ViewBag.SortOrder = currentSortOrder;
                    return View(users);
            }
            

            ViewBag.SortOrder = newSortOrder;
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

        //returns a list of Production IDs of current season
        public static List<int> FindCurrentProductions()
        {
            var admin = new AdminController();
            int currentSeason = AdminSettingsReader.CurrentSettings().current_season;
            List<int> result = admin.db.Productions.Where(p => p.Season == currentSeason).OrderBy(p => p.OpeningDay).
                Select(prod => prod.ProductionId).ToList();
            return result;
        }

        //returns a List<Production> from a List<int> of current season production IDs
        public static List<Production> GetCurrentProductions()
        {
            List<int> i = new List<int>();
            var admin = new AdminController();
            List<int> current = new List<int>();
            List<Production> currentProd = admin.db.Productions.Where(p => current.Contains(p.ProductionId)).OrderBy(p => p.OpeningDay).ToList();
            return currentProd;
        }

        //returns ModelsMissingPhotos which contains a List<int> for models:
        //Production, ProductionPhotos, Sponsor, and Cast that dont have a picture.
        public static ModelsMissingPhotos FindModelsNoPics()
        {
            var admin = new AdminController();
            ModelsMissingPhotos modelsWithNoPics = new ModelsMissingPhotos();
            modelsWithNoPics.cast_members = admin.db.CastMembers.Where(p => p.PhotoId == null).OrderBy(p => p.CastMemberID).
                Select(p => p.CastMemberID).ToList();

            modelsWithNoPics.productions = admin.db.Productions.Where(p => p.DefaultPhoto == null).OrderBy(p => p.ProductionId).
                Select(p => p.ProductionId).ToList();

            modelsWithNoPics.production_photos = admin.db.ProductionPhotos.Where(p => p.PhotoId == null).OrderBy(p => p.ProPhotoId).
                Select(p => p.ProPhotoId).ToList();

            modelsWithNoPics.sponsors = admin.db.Sponsors.Where(p => p.LogoId == null).OrderBy(p => p.LogoId).
                Select(p => p.SponsorId).ToList();

            return modelsWithNoPics;
        }

        private void UpdateAdminSettings()
        {
            //flowchart: reads json file, deserializes, updates the values, serializes new string, and writes result to json file

            List<int> currentProd = FindCurrentProductions();
            ModelsMissingPhotos miss = FindModelsNoPics();

            string json_path = Server.MapPath(Url.Content("~/AdminSettings.json"));
            string json_string = null;
            using (StreamReader reader = new StreamReader(json_path))
            {
                json_string = reader.ReadToEnd();
            }

            JObject json_content = (JObject)JsonConvert.DeserializeObject(json_string);

            json_content.Property("current_productions").Value = JToken.FromObject(currentProd);
            json_content.Property("models_missing_photos").Value = JToken.FromObject(miss);

            string updated_json = JsonConvert.SerializeObject(json_content, Formatting.Indented);

            System.IO.File.WriteAllText(json_path, updated_json);
        }
    }
}

