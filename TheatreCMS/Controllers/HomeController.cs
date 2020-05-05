using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TheatreCMS.Controllers;
using TheatreCMS.Models;
using TheatreCMS.ViewModels;
using System.Data.Entity;
using TheatreCMS.Areas.Subscribers.Models;

namespace TheatreCMS.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            var productions = from p in db.Productions
                          select p;


            //Filter list by current and future productions
            var query = productions.Where(p => p.OpeningDay > DateTime.Now || p.IsCurrent == true || (p.OpeningDay <= DateTime.Now && p.ClosingDay >= DateTime.Now))
                .OrderBy(p => p.OpeningDay);

            List<Production> unorderedProductions = query.ToList();
            var orderedProductions = SortProductions(unorderedProductions);

            return View(orderedProductions);
        }

        private List<Production> SortProductions(List<Production> list)
        {
            var sorted = new List<Production>();
            var onStage = new List<Production>();
            var current = new List<Production>();
            var comingSoon = new List<Production>();

            foreach (var item in list)
            {
                if (item.OpeningDay <= DateTime.Now && item.ClosingDay >= DateTime.Now)
                {
                    onStage.Add(item);
                }
            }
            sorted.AddRange(onStage);
            foreach (var item in list)
            {
                if (item.IsCurrent && !sorted.Contains(item))
                {
                    current.Add(item);
                }
            }
            sorted.AddRange(current);
            foreach (var item in list)
            {
                if (item.OpeningDay > DateTime.Now && !sorted.Contains(item))
                {
                    comingSoon.Add(item);
                }
            }
            sorted.AddRange(comingSoon);

            return sorted;
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        //File upload GET and POST controls
        public ActionResult UploadImage()
        {
            
            return View();
        }
        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            try
            {
                //Using Helpers.ImageUploader.ImageBytes to get the byte[] representation of the file
                //and extracting the string representation as a returned out-parameter
                string imageBase64;
                byte[] imageBytes = ImageUploadController.ImageBytes(file, out imageBase64);

                //Add the base64 representation of the image to the ViewBag to be accessed by the View
                ViewBag.ImageData = String.Format("data:image/png;base64,{0}", imageBase64);

                ViewBag.Message = "Image uploaded successfully!";
                return View();
            }

            catch
            {
                //Using this empty string for the View to trigger when an upload fails
                ViewBag.ImageData = "";
                ViewBag.Message = "There was an error uploading your image :(";
            }
            return View();
        }

        public ActionResult NewsletterSubscribers()
        {
            using (var db = new ApplicationDbContext())
            {
                var listSubscribers = db.Subscribers.Where(s => s.Newsletter == true).ToList();
                var newsletterListVms = new List<NewsletterListVm>();
                foreach (var listSubscriber in listSubscribers)
                {
                    var signUpVm = new NewsletterListVm
                    {
                        FirstName = listSubscriber.SubscriberPerson.FirstName,
                        LastName = listSubscriber.SubscriberPerson.LastName,
                        StreetAddress = listSubscriber.SubscriberPerson.StreetAddress,
                        City = listSubscriber.SubscriberPerson.City,
                        State = listSubscriber.SubscriberPerson.State,
                        ZipCode = listSubscriber.SubscriberPerson.ZipCode,
                        Email = listSubscriber.SubscriberPerson.Email
                    };
                    newsletterListVms.Add(signUpVm);
                }
                return View(newsletterListVms);
            }

        }

        public ActionResult Archive()
        {
            var db = new ApplicationDbContext();
            var productions = db.Productions
                .Include(i => i.DefaultPhoto);
            return View(productions.ToList());
        }

        [HttpPost]
        public ActionResult Archive(string SearchByCategory, string ArchiveSearchField)
        {
            var db = new ApplicationDbContext();
            var productions = db.Productions
                .Include(i => i.DefaultPhoto);
            ViewBag.Category = SearchByCategory;

            switch (SearchByCategory)
            {
                case "ArchiveCastMember":
                    ViewBag.Message = string.Format("Displaying Results for \"{0}\" in Cast Members", ArchiveSearchField);
                    var resultsCast = db.CastMembers.Where(x => x.Name.ToLower().Contains(ArchiveSearchField.ToLower())).ToList();
                    ViewData["ResultsList"] = resultsCast;
                    break;
                    case "ArchiveProduction":
                        ViewBag.Message = string.Format("Displaying Results for \"{0}\" in Productions", ArchiveSearchField);
                    var resultsProduction = db.Productions.Where(x => x.Title.ToLower().Contains(ArchiveSearchField.ToLower())).ToList();
                    ViewData["ResultsList"] = resultsProduction;
                    break;
                    case "ArchivePart":
                        ViewBag.Message = string.Format("Displaying Results for \"{0}\" in Parts", ArchiveSearchField);
                    var resultsPart = db.Parts.Where(x => x.Character.ToLower().Contains(ArchiveSearchField.ToLower())).ToList();
                    ViewData["ResultsList"] = resultsPart;
                    break;
                    default:
                        break;
                }
            return View(productions.ToList());
            
                
        }

        
    }
}