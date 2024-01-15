using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FIT5032_main_project.Models;
using Microsoft.AspNet.Identity;
using System.Data.Entity;

namespace FIT5032_main_project.Controllers
{
    public class DoctorController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Doctors
        [Authorize(Roles = "Patient")]
        public ActionResult ViewDoctors()
        {
            var doctors = db.Doctors.Include(d => d.Ratings).ToList();
            return View(doctors);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Patient")]
        public ActionResult RateDoctor(int doctorId, int ratingScore)
        {
            var userId = User.Identity.GetUserId();

            // Check if the user already rated this doctor
            var existingRating = db.Ratings.FirstOrDefault(r => r.DoctorId == doctorId && r.PatientId == userId);

            if (existingRating != null)
            {
                existingRating.Score = ratingScore; // Update existing rating
            }
            else
            {
                var rating = new Rating
                {
                    DoctorId = doctorId,
                    PatientId = userId,
                    Score = ratingScore
                };
                db.Ratings.Add(rating);  // Add new rating
            }

            db.SaveChanges();

            return RedirectToAction("ViewDoctors");
        }

        // GET: Doctor Detail
        [Authorize(Roles = "Patient")]
        public ActionResult ViewDoctorDetail(int id)
        {
            var doctor = db.Doctors.Include(d => d.Ratings).FirstOrDefault(d => d.DoctorId == id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

       

    }


}
