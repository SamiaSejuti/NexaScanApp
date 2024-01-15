using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FIT5032_main_project.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace FIT5032_main_project.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var manager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
                var roles = manager.GetRoles(userId);
                var role = roles.FirstOrDefault();
                string welcomeMessage = "Welcome to NexaScan!"; 
                if (role == "Patient")
                {
                    welcomeMessage = "Welcome Patient to NexaScan!";
                }
                else if (role == "Doctor")
                {
                    welcomeMessage = "Welcome Doctor to NexaScan!";
                }

                ViewBag.WelcomeMessage = welcomeMessage;
                
            }

            return View();
        }

        public ActionResult Services()
        {
            ViewBag.Message = "Your application services page.";

            return View();
        }


        [Authorize]
        public ActionResult ScheduleScan()
        {
            ViewBag.Message = "Your ScheduleScan page.";

            return View();
        }


        public ActionResult About(String date)
        {
            if (String.IsNullOrEmpty(date))
            {
                return View(new Event()); // Return the view with an empty Event model
            }

            Event e = new Event();
            DateTime convertedDate = DateTime.Parse(date);
            e.Start = convertedDate;

            return View(e);
        }


        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Comments()
        {
            return View();
        }


    }
}