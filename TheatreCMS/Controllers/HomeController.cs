﻿using System;
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
using System.Text.RegularExpressions;

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
            ArchiveSearch(db, SearchByCategory, ArchiveSearchField);
            return View(productions.ToList());
        }

        private void ArchiveSearch(ApplicationDbContext db, string SearchByCategory, string SearchKey)
        {
            ViewBag.Category = SearchByCategory;
            ViewData["SearchKey"] = SearchKey;
            string highlightedKey = "<span class='bg-primary'>" + SearchKey + "</span>";   //The value of this variable can be altered to change the highlight color of the match.
            switch (SearchByCategory)
            {
                case "ArchiveAll":
                    ViewBag.Message = string.Format("Results for \"{0}\" in Archive", SearchKey);
                    var resultsCast = db.CastMembers.Where(x => x.Name.ToLower().Contains(SearchKey.ToLower())              //Creates a list of cast members where there are search matches in any of those three columns
                                                             || x.YearJoined.ToString().Contains(SearchKey.ToLower())
                                                             || x.Bio.ToLower().Contains(SearchKey.ToLower())).ToList();
                    resultsCast = resultsCast.Distinct().ToList();//Prevents duplicate listings
                    var yearJoinedString = new List<string>();
                    for (int i = 0; i < resultsCast.Count; i++)   //YearJoined must be converted to text to highlight it properly. A separate list is created, then added to the viewbag.
                    { 
                        resultsCast[i].Name = Regex.Replace(resultsCast[i].Name, SearchKey, highlightedKey, RegexOptions.IgnoreCase);
                        resultsCast[i].Bio = Regex.Replace(resultsCast[i].Bio, SearchKey, highlightedKey, RegexOptions.IgnoreCase);
                        yearJoinedString.Add(resultsCast[i].YearJoined.ToString());
                        yearJoinedString[i] = Regex.Replace(yearJoinedString[i], SearchKey, highlightedKey, RegexOptions.IgnoreCase);
                    }
                    ViewBag.YearJoined = yearJoinedString;
                    var resultsProduction = db.Productions.Where(x => x.Title.ToLower().Contains(SearchKey.ToLower())
                                                                   || x.Playwright.ToLower().Contains(SearchKey.ToLower())
                                                                   || x.Description.ToLower().Contains(SearchKey.ToLower())).ToList();
                    resultsProduction = resultsProduction.Distinct().ToList();
                    foreach (Production production in resultsProduction)
                    {
                        production.Title = Regex.Replace(production.Title, SearchKey, highlightedKey);
                        production.Playwright = Regex.Replace(production.Playwright, SearchKey, highlightedKey);
                        production.Description = Regex.Replace(production.Description, SearchKey, highlightedKey);

                    }
                    var resultsPart = db.Parts.Where(x => x.Character.ToLower().Contains(SearchKey.ToLower())
                                                       || x.Production.Title.ToLower().Contains(SearchKey.ToLower())
                                                       || x.Person.Name.ToLower().Contains(SearchKey.ToLower())).ToList();
                    resultsPart = resultsPart.Distinct().ToList();
                    foreach (Part part in resultsPart)
                    {
                        part.Character = Regex.Replace(part.Character, SearchKey, highlightedKey);
                        part.Production.Title = Regex.Replace(part.Production.Title, SearchKey, highlightedKey);
                        part.Person.Name = Regex.Replace(part.Person.Name, SearchKey, highlightedKey);
                    }
                    if (resultsCast.Count > 0) ViewBag.ResultsCast = resultsCast;                       //sets ViewData value if there were any results
                    if (resultsProduction.Count > 0) ViewBag.ResultsProduction = resultsProduction;
                    if (resultsPart.Count > 0) ViewBag.ResultsPart = resultsPart;
                    break;
                case "ArchiveCastMember":
                    ViewBag.Message = string.Format("Results for \"{0}\" in Cast Members", SearchKey);
                    resultsCast = db.CastMembers.Where(x => x.Name.ToLower().Contains(SearchKey.ToLower())
                                                         || x.YearJoined.ToString().Contains(SearchKey.ToLower())
                                                         || x.Bio.ToLower().Contains(SearchKey.ToLower())).ToList();
                    resultsCast = resultsCast.Distinct().ToList();
                    yearJoinedString = new List<string>();
                    for (int i = 0; i < resultsCast.Count; i++)
                    {
                        resultsCast[i].Name = Regex.Replace(resultsCast[i].Name, SearchKey, highlightedKey, RegexOptions.IgnoreCase);
                        resultsCast[i].Bio = Regex.Replace(resultsCast[i].Bio, SearchKey, highlightedKey, RegexOptions.IgnoreCase);
                        yearJoinedString.Add(resultsCast[i].YearJoined.ToString());
                        yearJoinedString[i] = Regex.Replace(yearJoinedString[i], SearchKey, highlightedKey, RegexOptions.IgnoreCase);
                    }
                    ViewBag.YearJoined = yearJoinedString;
                    if (resultsCast.Count > 0) ViewBag.ResultsCast = resultsCast;
                    break;
                case "ArchiveProduction":
                    ViewBag.Message = string.Format("Results for \"{0}\" in Productions", SearchKey);
                    resultsProduction = db.Productions.Where(x => x.Title.ToLower().Contains(SearchKey.ToLower())
                                                               || x.Playwright.ToLower().Contains(SearchKey.ToLower())
                                                               || x.Description.ToLower().Contains(SearchKey.ToLower())).ToList();
                    resultsProduction = resultsProduction.Distinct().ToList();
                    if (resultsProduction.Count > 0) ViewData["ResultsProduction"] = resultsProduction;
                    foreach (Production production in resultsProduction)
                    {
                        production.Title = Regex.Replace(production.Title, SearchKey, highlightedKey);
                        production.Playwright = Regex.Replace(production.Playwright, SearchKey, highlightedKey);
                        production.Description = Regex.Replace(production.Description, SearchKey, highlightedKey);
                    }
                    break;
                case "ArchivePart":
                    ViewBag.Message = string.Format("Results for \"{0}\" in Parts", SearchKey);
                    resultsPart = db.Parts.Where(x => x.Character.ToLower().Contains(SearchKey.ToLower())
                                                   || x.Production.Title.ToLower().Contains(SearchKey.ToLower())
                                                   || x.Person.Name.ToLower().Contains(SearchKey.ToLower())).ToList();
                    resultsPart = resultsPart.Distinct().ToList();
                    if (resultsPart.Count > 0) ViewData["ResultsPart"] = resultsPart;
                    foreach (Part part in resultsPart)
                    {
                        part.Character = Regex.Replace(part.Character, SearchKey, highlightedKey);
                        part.Production.Title = Regex.Replace(part.Production.Title, SearchKey, highlightedKey);
                        part.Person.Name = Regex.Replace(part.Person.Name, SearchKey, highlightedKey);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}