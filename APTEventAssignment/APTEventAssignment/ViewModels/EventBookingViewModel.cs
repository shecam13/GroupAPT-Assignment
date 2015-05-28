using APTEventAssignment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class EventBookingViewModel
    {
        [Key]
        [Display(Name = "Booking Number")]
        public int EventBooking_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string EventBooking_UserID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int EventBooking_EventPerformanceID { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }
       
        [Display(Name = "Date of Booking")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EventBooking_Date { get; set; }

        [Display(Name = "Event")]
        public string EventName { get; set; }

        [Display(Name = "Date of Event")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> PerformanceDate { get; set; }

    }

    public class AddEventBookingViewModel
    {
        [Key]
        public int EventBooking_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string EventBooking_UserID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int? EventBooking_EventPerformanceID { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Date of Booking")]
        public Nullable<System.DateTime> EventBooking_Date { get; set; }

        [Display(Name = "Event")]
        public string EventName { get; set; }

        [Display(Name = "Date of Event")]
        public Nullable<System.DateTime> PerformanceDate { get; set; }
    }
}