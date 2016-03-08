using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskilyWeb.Controllers
{
    public class DebugAndTestController : Controller
    {
        // GET: DebugAndTest
        public ActionResult Index()
        {
            var emailHelper = new Helpers.TaskilyEmailHelper();
            emailHelper.SendUpgradeEmail("kevin@jumoo.co.uk");

            return View();
        }
    }
}