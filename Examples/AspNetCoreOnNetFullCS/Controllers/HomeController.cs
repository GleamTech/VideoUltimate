﻿using Microsoft.AspNetCore.Mvc;

namespace GleamTech.VideoUltimateExamples.AspNetCoreOnNetFullCS.Controllers
{
    public partial class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
