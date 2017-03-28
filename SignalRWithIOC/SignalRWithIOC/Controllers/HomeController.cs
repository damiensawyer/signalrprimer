using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HelloSignalR.Services;

namespace HelloSignalR.Controllers
{
    public class HomeController : Controller
    {
        private readonly INameService _nameService;

        public HomeController(INameService nameService)
        {
            _nameService = nameService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IOCHub()
        {
            return View();
        }
    }
}