using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APTEventAssignment.ViewModels
{
    public class EventBookingViewModel
    {
        [Display(Name = "Booking Number")]
        public string EventBooking_UserID { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
       
        [Display(Name = "Date of Booking")]
        public Nullable<System.DateTime> EventBooking_Date { get; set; }

        [Display(Name = "Event")]
        public string EventName { get; set; }

        [Display(Name = "Date of Event")]
        public Nullable<System.DateTime> PerformanceDate { get; set; }

        
        //public Nullable<int> EventBooking_EventPerformanceID { get; set; }
        //public virtual EventPerformance EventPerformance { get; set; }
        //public virtual ICollection<EventBookingSeat> EventBookingSeat { get; set; }
    }
}