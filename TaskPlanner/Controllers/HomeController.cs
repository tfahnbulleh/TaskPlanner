﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TaskPlanner.Models;

namespace TaskPlanner.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("https://tfahnbulleh.github.io/Task-Planner/");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
