using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FIT5032_main_project.Models;
using Microsoft.AspNet.Identity;

namespace FIT5032_main_project.Controllers
{
    public class PatientController : Controller
    {

        // GET: Patient
        [Authorize(Roles = "Patient")]
        public ActionResult Index()
        {
            return View();
        }

    }
}

