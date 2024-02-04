using FIT5032_main_project.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Data.Entity;
using System;

public class MyScheduleController : Controller
{
    private ApplicationDbContext db = new ApplicationDbContext();


    [Authorize(Roles = "Doctor")]
    public ActionResult MySchedule()
    {
        ViewBag.Message = "Your time scheduling page.";
        string applicationUserId = User.Identity.GetUserId();

        var doctor = db.Doctors.FirstOrDefault(d => d.ApplicationUserId == applicationUserId);

        if (doctor == null)
        {
            return HttpNotFound("Doctor not found.");
        }

        var schedules = db.Schedules.Where(s => s.DoctorId == doctor.DoctorId).ToList();
        return View(schedules);
    }

    [HttpGet]
    [Authorize(Roles = "Doctor")]
    public ActionResult Add()
    {
        return View();
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Doctor")]
    public ActionResult Add([Bind(Include = "ScheduleDate,StartTime,EndTime")] Schedule schedule)
    {
        try
        {
            string applicationUserId = User.Identity.GetUserId();
            var doctor = db.Doctors.FirstOrDefault(d => d.ApplicationUserId == applicationUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found.");
            }

            schedule.DoctorId = doctor.DoctorId;

            if (schedule.StartTime == default(TimeSpan) || schedule.EndTime == default(TimeSpan))
            {
                ModelState.AddModelError("", "Start Time and End Time are required.");
                return View(schedule);
            }

            if (schedule.ScheduleDate == null || schedule.StartTime == null || schedule.EndTime == null)
            {
                throw new Exception("Invalid schedule details.");
            }

            if (schedule.StartTime == default(TimeSpan) || schedule.EndTime == default(TimeSpan))
            {
                ModelState.AddModelError("", "Start Time and End Time are required.");
                return View(schedule);
            }

            var startHour = schedule.StartTime.Hours;
            var endHour = startHour + 1;
            if (endHour > 23)
            {
                endHour = 0;
            }

            schedule.EndTime = new TimeSpan(endHour, schedule.StartTime.Minutes, 0);

            var existingSchedules = db.Schedules
                .Where(s => s.DoctorId == doctor.DoctorId && s.ScheduleDate == schedule.ScheduleDate)
                .ToList();

            foreach (var existingSchedule in existingSchedules)
            {
                if (IsTimeRangeOverlap(existingSchedule.StartTime, existingSchedule.EndTime, schedule.StartTime, schedule.EndTime))
                {
                    ModelState.AddModelError("", "Time clash with an existing schedule.");
                    return View(schedule);
                }
            }

            db.Schedules.Add(schedule);
            db.SaveChanges();
            return RedirectToAction("MySchedule", "MySchedule");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            return new HttpStatusCodeResult(500, "An error occurred while trying to add the schedule.");
        }
    }

    private bool IsTimeRangeOverlap(TimeSpan startTime1, TimeSpan endTime1, TimeSpan startTime2, TimeSpan endTime2)
    {
        return (startTime1 < endTime2 && startTime2 < endTime1);
    }



    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Doctor")]
    public ActionResult UpdateSchedule(List<Schedule> schedules)
    {
        if (ModelState.IsValid)
        {
            foreach (var schedule in schedules)
            {
                db.Entry(schedule).State = EntityState.Modified;
            }
            db.SaveChanges();
            return RedirectToAction("MySchedule", "MySchedule");
        }
        return View(schedules);
    }

    private List<Schedule> CreateDefaultSchedule(int doctorId)
    {
        DateTime defaultDate = DateTime.Today.AddDays(1); // Setting the default date to tomorrow
        var defaultSchedule = new Schedule
        {
            DoctorId = doctorId,
            ScheduleDate = defaultDate,
            StartTime = new TimeSpan(9, 0, 0), // 9 AM
            EndTime = new TimeSpan(17, 0, 0)  // 5 PM
        };

        // Adding default schedule to the database
        db.Schedules.Add(defaultSchedule);
        db.SaveChanges();

        return new List<Schedule> { defaultSchedule };
    }



    [HttpGet]
    [Authorize(Roles = "Doctor")]
    public ActionResult Edit(int id)
    {
        string applicationUserId = User.Identity.GetUserId();
        var doctor = db.Doctors.FirstOrDefault(d => d.ApplicationUserId == applicationUserId);
        if (doctor == null)
        {
            return new HttpStatusCodeResult(403, "Doctor not found.");
        }

        var schedule = db.Schedules.Find(id);
        if (schedule == null || schedule.DoctorId != doctor.DoctorId)
        {
            return HttpNotFound();
        }
        return View(schedule);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    [Authorize(Roles = "Doctor")]
    public ActionResult Edit([Bind(Include = "ScheduleId,ScheduleDate,StartTime,EndTime")] Schedule schedule)
    {
        try
        {
            string applicationUserId = User.Identity.GetUserId();
            var doctor = db.Doctors.FirstOrDefault(d => d.ApplicationUserId == applicationUserId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found.");
            }

            schedule.DoctorId = doctor.DoctorId;

            if (schedule.ScheduleDate == null || schedule.StartTime == null || schedule.EndTime == null)
            {
                throw new Exception("Invalid schedule details.");
            }

            var existingSchedule = db.Schedules.Find(schedule.ScheduleId);
            if (existingSchedule == null)
            {
                throw new Exception("Schedule not found.");
            }

            var existingSchedules = db.Schedules
                .Where(s => s.DoctorId == doctor.DoctorId && s.ScheduleDate == schedule.ScheduleDate && s.ScheduleId != schedule.ScheduleId)
                .ToList();

            foreach (var existing in existingSchedules)
            {
                if (IsTimeRangeOverlap(existing.StartTime, existing.EndTime, schedule.StartTime, schedule.EndTime))
                {
                    ModelState.AddModelError("", "Time clash with an existing schedule.");
                    return View(schedule);
                }
            }

            existingSchedule.ScheduleDate = schedule.ScheduleDate;
            existingSchedule.StartTime = schedule.StartTime;
            existingSchedule.EndTime = schedule.EndTime;

            ModelState.Clear();
            TryValidateModel(existingSchedule);

            if (ModelState.IsValid)
            {
                db.Entry(existingSchedule).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("MySchedule", "MySchedule");
            }
            return View(schedule);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            return new HttpStatusCodeResult(500, "An error occurred while trying to save the schedule.");
        }
    }



    public ActionResult Delete(int id)
    {
        try
        {
            var schedule = db.Schedules.Find(id);
            if (schedule == null)
            {
                return HttpNotFound();
            }

            var doctorId = schedule.DoctorId;
            db.Schedules.Remove(schedule);
            db.SaveChanges();

            if (!db.Schedules.Any(s => s.DoctorId == doctorId))
            {
                var doctor = db.Doctors.Find(doctorId);
                if (!doctor.IsDefaultScheduleAdded)
                {
                    DateTime defaultDate = DateTime.Today.AddDays(1); // Default to tomorrow
                    var defaultSchedule = new Schedule
                    {
                   
                        DoctorId = doctorId,
                        ScheduleDate = defaultDate,
                        StartTime = new TimeSpan(9, 0, 0),
                        EndTime = new TimeSpan(17, 0, 0)
                    };
                    db.Schedules.Add(defaultSchedule);
                    db.SaveChanges();

           
                    doctor.IsDefaultScheduleAdded = true;
                    db.Entry(doctor).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("MySchedule", "MySchedule");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception: " + ex.Message);
            return new HttpStatusCodeResult(500, "An error occurred while trying to delete the schedule.");
        }
    }
}

