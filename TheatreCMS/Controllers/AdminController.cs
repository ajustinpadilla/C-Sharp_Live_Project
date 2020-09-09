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
            #region ViewData
            List<SelectListItem> productionList = GetSelectListItems();
            ViewData["ProductionList"] = productionList;
            #endregion
            UpdateAdminSettings();
            AdminSettings current = new AdminSettings();
            current = AdminSettingsReader.CurrentSettings();
            ViewData["CurrentProductionList"] = GetCurrentProductions();
            ViewData["nextSeasonNotification"] = NextSeasonNotification();
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
        public ActionResult SettingsUpdate(AdminSettings currentSettings)
        {
            // The current_productions property of the AdminSettings object in the parameter does not 
            // get set when POSTED.  This line of code sets that property.
            currentSettings.current_productions = FindCurrentProductions();

            // Populates the ViewData with the current productions same as the Dashboard() method.
            // Without this line, an error is thrown after the form is submitted.
            ViewData["CurrentProductionList"] = GetCurrentProductions();

            List<SelectListItem> productionList = GetSelectListItems();
            ViewData["ProductionList"] = productionList;
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
            int thisSeason = AutoCalculateCurrentSeason();
            //ADDING AUTO CALC ERROR CODE HERE
            if (newJSON.current_season != thisSeason)
                //(oldJSON.current_season == 23)
                //(oldJSON.current_season != newJSON.current_season)
            {
                ModelState.AddModelError("current_season", "You have entered the incorrect Season number."); 
            }
            
          
            //END AUTO CALC ERROR CODE

            if (oldJSON.season_productions != newJSON.season_productions)
            {
                UpdateProductions(newJSON);
            }
            using (StreamWriter writer = new StreamWriter(filepath))
            {
                writer.Write(newSettings);
                return View("Dashboard", currentSettings); 
            }
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

        //returns a Lis<Production> from a List<int> of current season production IDs
        public static List<Production> GetCurrentProductions()
        {
            var admin = new AdminController();
            List<int> currentProdId = AdminSettingsReader.CurrentSettings().current_productions;
            List<Production> currentProd = admin.db.Productions.Where(p => currentProdId.Contains(p.ProductionId)).OrderBy(p=> p.OpeningDay).ToList();
            return currentProd;
        }

        // updates the AdminSettings.json file with list of current season Production IDs
        private void UpdateAdminSettings()
        {
            //flowchart: reads json file, deserializes, updates the values, serializes new string, and writes result to json file
            List<int> currentProd = AdminController.FindCurrentProductions();
            string json_path = Server.MapPath(Url.Content("~/AdminSettings.json"));
            string json_string = null;

            using (StreamReader reader = new StreamReader(json_path))
            {
                json_string = reader.ReadToEnd();
            }

            JObject json_content = (JObject)JsonConvert.DeserializeObject(json_string);
            json_content.Property("current_productions").Value = JToken.FromObject(currentProd);
            string updated_json = JsonConvert.SerializeObject(json_content, Formatting.Indented);

            using (StreamWriter writer = new StreamWriter(json_path))
            {
                writer.Write(updated_json);
            }


        }
    }
}

