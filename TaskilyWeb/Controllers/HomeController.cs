using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskilyWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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

        public ActionResult Hosted()
        {
            return View();
        }
        public ActionResult Demos()
        {
            return View();
        }
        
        public ActionResult Source()
        {
            return View();
        }
        
        public ActionResult Support()
        {
            return View();
        }
    }
}