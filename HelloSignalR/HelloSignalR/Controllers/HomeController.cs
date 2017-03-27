using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HelloSignalR.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult First_HelloSignalR()
        {
            return View();
        }

        public ActionResult Second_CommonService()
        {
            return View();
        }
    }
}