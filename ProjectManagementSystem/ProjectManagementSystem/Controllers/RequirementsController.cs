﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ProjectManagementSystem.Controllers
{
    public class RequirementsController : Controller
    {
        // GET: Requirements
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Create() {
            return View();
        }
    }
}