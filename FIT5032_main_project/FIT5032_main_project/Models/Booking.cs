using System;
using System.ComponentModel.DataAnnotations;

namespace FIT5032_main_project.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }
        public int ScheduleId { get; set; }
        public virtual Schedule Schedule { get; set; }

        public int PatientId { get; set; }
        public virtual Patient Patient { get; set; }
    }
}