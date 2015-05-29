using APTEventAssignment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class EventPerformancesViewModel
    {
        [Key]
        public int EventPerformance_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> EventPerformance_EventID { get; set; }

        [Display(Name = "Event")]
        public string EventPerformance_EventName { get; set; } 

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EventPerformance_Date { get; set; }

        [Display(Name = "Time")]
        public Nullable<System.TimeSpan> EventPerformance_Time { get; set; }

    }

    public class AddEventPerformanceViewModel
    {
        [Key]
        public int EventPerformance_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> EventPerformance_EventID { get; set; }

        [Display(Name = "Event")]
        public string EventPerformance_EventName { get; set; }  

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> EventPerformance_Date { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "Is required field. Format hh:mm (24 hour time)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm\:ss}")]
        public Nullable<System.TimeSpan> EventPerformance_Time { get; set; }
    }
}