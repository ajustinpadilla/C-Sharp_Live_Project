using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TheatreCMS.Areas.Subscribers.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Subscribers/Dashboard
        public ActionResult Index()
        {
            return View();
        }
    }
}