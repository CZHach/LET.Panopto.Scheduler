using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LET.Panopto.Scheduler.Models;
using Microsoft.AspNetCore.Authorization;

namespace LET.Panopto.Scheduler.Controllers
{
    public class HomeController : Controller
    {
        [Authorize(Roles = "PITT\\LET Admins")]
        public IActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "PITT\\LET Admins")]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }
        [Authorize(Roles = "PITT\\LET Admins")]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }
        [Authorize(Roles = "PITT\\LET Admins")]
        public IActionResult Privacy()
        {
            return View();
        }
        [Authorize(Roles = "PITT\\LET Admins")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
