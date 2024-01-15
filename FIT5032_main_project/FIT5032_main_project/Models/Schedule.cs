using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;

namespace FIT5032_main_project.Models
{
    public enum Days
    {
        Monday,
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }


    public class Schedule
    {
        public int ScheduleId { get; set; }

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        public DateTime ScheduleDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }

        public int DoctorId { get; set; }

        [Required]
        public virtual Doctor Doctor { get; set; }
        public virtual ICollection<Booking> Bookings { get; set; }
    }
}