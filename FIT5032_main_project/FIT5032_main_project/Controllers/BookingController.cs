using FIT5032_main_project.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;

public class BookingController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();

    [Authorize(Roles = "Patient")]
    public ActionResult ScheduleScan()
    {
        if (TempData["BookingSuccess"] != null)
        {
            ViewBag.BookingSuccess = TempData["BookingSuccess"].ToString();
        }
        // Fetch the list of doctors to populate the dropdown
        ViewBag.DoctorList = new SelectList(db.Doctors, "DoctorId", "Name");

        // Retrieve the appointments for the logged-in patient
        string applicationUserId = User.Identity.GetUserId();
        var patient = db.Patients.FirstOrDefault(p => p.ApplicationUserId == applicationUserId);
        if (patient != null)
        {
            var appointments = db.Bookings
                                .Include("Schedule")
                                .Include("Schedule.Doctor")
                                .Where(b => b.PatientId == patient.PatientId)
                                .ToList();
            ViewBag.Appointments = appointments;
        }

        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Patient")]
    public ActionResult ScheduleScan(Booking booking, int doctorId, int scheduleId, DateTime selectedDate)
    {
        try
        {
            string applicationUserId = User.Identity.GetUserId();
            var patient = db.Patients.FirstOrDefault(p => p.ApplicationUserId == applicationUserId);
            if (patient == null)
            {
                throw new Exception("Patient not found.");
            }

            booking.PatientId = patient.PatientId;
            booking.ScheduleId = scheduleId;
            booking.BookingDate = selectedDate;
            db.Bookings.Add(booking);
            db.SaveChanges();

            // Redirect to the same action to refresh the appointments list
            return RedirectToAction("ScheduleScan", "Booking");
        }
        catch (Exception ex)
        {
            // repopulate the doctor list in ViewBag
            ViewBag.DoctorList = new SelectList(db.Doctors, "DoctorId", "Name");
            ViewBag.Error = ex.Message;

            // Retrieve the appointments for the logged-in patient
            string applicationUserId = User.Identity.GetUserId();
            var patient = db.Patients.FirstOrDefault(p => p.ApplicationUserId == applicationUserId);
            if (patient != null)
            {
                var appointments = db.Bookings
                                    .Include("Schedule")
                                    .Include("Schedule.Doctor")
                                    .Where(b => b.PatientId == patient.PatientId)
                                    .ToList();
                ViewBag.Appointments = appointments;
            }

            return View(booking);
        }
    }



    [Authorize(Roles = "Patient")]
    public JsonResult GetDoctorSchedules(int doctorId)
    {
        var schedules = db.Schedules
            .Where(s => s.DoctorId == doctorId)
            .Select(s => new { s.ScheduleId, Time = s.StartTime + " - " + s.EndTime })
            .ToList();

        return Json(schedules, JsonRequestBehavior.AllowGet);
    }

    public ActionResult GetAvailableDates(int doctorId)
    {
        try
        {
            var availableDates = db.Schedules
                .Where(s => s.DoctorId == doctorId)
                .Select(s => s.ScheduleDate)
                .Distinct()
                .ToList()
                .Select(d => d.ToString("yyyy-MM-dd"))
                .ToList();

            return Json(availableDates, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            // Your error handling code here
            return Json(new { error = "An error occurred while retrieving available dates." }, JsonRequestBehavior.AllowGet);
        }
    }





    public ActionResult GetAvailableTimings(int doctorId, string selectedDay)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("GetAvailableTimings method started.");
            System.Diagnostics.Debug.WriteLine("DoctorId: " + doctorId);
            System.Diagnostics.Debug.WriteLine("Selected Day: " + selectedDay);

            if (!DateTime.TryParse(selectedDay, out DateTime selectedDateTime))
            {
                return Json(new { error = "Invalid date format." }, JsonRequestBehavior.AllowGet);
            }

            var bookedScheduleIds = db.Bookings
                .Where(b => b.Schedule.DoctorId == doctorId && DbFunctions.TruncateTime(b.Schedule.ScheduleDate) == selectedDateTime.Date)
                .Select(b => b.ScheduleId)
                .ToList();


            System.Diagnostics.Debug.WriteLine("Before querying the database.");
            var availableTimings = db.Schedules
    .Where(s => s.DoctorId == doctorId && DbFunctions.TruncateTime(s.ScheduleDate) == selectedDateTime.Date && !bookedScheduleIds.Contains(s.ScheduleId))
    .Select(s => new
    {
        ScheduleId = s.ScheduleId,
        StartTime = s.StartTime
    })
    .ToList()
    .Select(s => new
    {
        ScheduleId = s.ScheduleId,
        Time = DateTime.Today.Add(s.StartTime).ToString("hh:mm tt")
    })
    .ToList();


            System.Diagnostics.Debug.WriteLine("After querying the database.");
            System.Diagnostics.Debug.WriteLine("Available Timings: " + availableTimings.Count);
            return Json(availableTimings, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("An error occurred.");
            System.Diagnostics.Debug.WriteLine(ex.Message);
            System.Diagnostics.Debug.WriteLine(ex.StackTrace);
            return Json(new { error = "An error occurred while retrieving available timings." }, JsonRequestBehavior.AllowGet);
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Patient")]
    public ActionResult CreateBooking(int scheduleId)
    {
        // Find the schedule in the database
        var schedule = db.Schedules.Find(scheduleId);
        if (schedule == null)
        {
            // Handle error - schedule not found
            ModelState.AddModelError("", "Schedule not found.");
            return View();
        }

        // Get the application user ID of the currently logged-in user
        string applicationUserId = User.Identity.GetUserId();
        var patient = db.Patients.FirstOrDefault(p => p.ApplicationUserId == applicationUserId);
        if (patient == null)
        {
            // Handle error - patient not found
            ModelState.AddModelError("", "Patient not found.");
            return View();
        }

        // Create a new booking
        Booking newBooking = new Booking
        {
            ScheduleId = scheduleId,
            BookingDate = schedule.ScheduleDate, 
            StartTime = schedule.StartTime,
            EndTime = schedule.EndTime,     
            PatientId = patient.PatientId
        };

       
        db.Bookings.Add(newBooking);
        db.SaveChanges();
        TempData["BookingSuccess"] = "Booking has been successfully created!";
        // Redirect to a success page
        return RedirectToAction("ScheduleScan", "Booking");


    }

    public ActionResult Appointments()
    {
        // Get the application user ID of the currently logged in user
        string applicationUserId = User.Identity.GetUserId();

        // Find the patient related to the logged in user
        var patient = db.Patients.FirstOrDefault(p => p.ApplicationUserId == applicationUserId);
        if (patient == null)
        {
            // Handle error - patient not found
            ModelState.AddModelError("", "Patient not found.");
            return View();
        }

        // Retrieve bookings related to the found patient
        var bookings = db.Bookings
                        .Include("Schedule")
                        .Include("Schedule.Doctor")
                        .Where(b => b.PatientId == patient.PatientId)
                        .ToList();

        return View(bookings);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Patient")]
    public ActionResult Delete(int id) // assuming id is the ScheduleId
    {
        var booking = db.Bookings.FirstOrDefault(b => b.ScheduleId == id);
        if (booking != null)
        {
            // Store schedule details before deleting the booking
            var scheduleDetails = new Schedule
            {
                ScheduleDate = booking.BookingDate,
                StartTime = booking.StartTime,
                EndTime = booking.EndTime,
                DoctorId = booking.Schedule.DoctorId
            };

            // Delete the booking
            db.Bookings.Remove(booking);
            db.SaveChanges();

            // Add the schedule back to the available schedules
            db.Schedules.Add(scheduleDetails);
            try
            {
                db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var validationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        // Log validation errors to a file, or output them to debug window.
                        Trace.TraceInformation("Property: {0} Error: {1}",
                                                validationError.PropertyName,
                                                validationError.ErrorMessage);
                    }
                }
            }


            ViewBag.BookingSuccess = "Booking has been successfully deleted!";
        }
        else
        {
            ViewBag.BookingError = "Booking not found.";
        }

        // Redirect back to the ScheduleScan view.
        return RedirectToAction("ScheduleScan");
    }






}
