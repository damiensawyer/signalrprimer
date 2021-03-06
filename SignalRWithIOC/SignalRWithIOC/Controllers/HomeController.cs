﻿using System;
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

        public HomeController(NameService nameService)
        {
            _nameService = nameService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IOCHub()
        {
            _nameService.Clients.All.FromController("Controller Accessed");
            return View();
        }
    }
}