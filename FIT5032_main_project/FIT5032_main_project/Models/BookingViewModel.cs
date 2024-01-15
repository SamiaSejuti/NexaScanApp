using System;
using System.Collections.Generic;

namespace FIT5032_main_project.Models
{
    public class BookingViewModel
    {
        public BookingViewModel()
        {
            Appointments = new List<Appointment>();
        }
        public int ScheduleId { get; set; }
        public DateTime BookingTime { get; set; }
        public List<Appointment> Appointments { get; set; } = new List<Appointment>();
    }

    public class Appointment
    {
        public string DoctorName { get; set; }
        public string DayOfWeek { get; set; }
        public DateTime Time { get; set; }
    }
}
