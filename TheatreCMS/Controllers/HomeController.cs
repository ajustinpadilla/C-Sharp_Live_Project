using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TheatreCMS.Controllers;
using TheatreCMS.Models;
using TheatreCMS.ViewModels;
using TheatreCMS.Areas.Subscribers.Models;

namespace TheatreCMS.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ApplicationDbContext db = new ApplicationDbContext();            
            var proPhotos = db.ProductionPhotos.ToList();            
            return View(proPhotos);
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
        public ActionResult Archive()
        {
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
        [HttpPost]
        public ActionResult Archive(string SearchByCategory, string ArchiveSearchField)
        {
            var db = new ApplicationDbContext();
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
                return View();
            
                
        }

        
    }
}