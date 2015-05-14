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

        //[Display(Name = "Event")]
        //public string EventPerformance_EventName { get; set; }  since this will be a partial view to the event creation..... don't need to show the event again 

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EventPerformance_Date { get; set; }

        //[Required(ErrorMessage = "Is required field. Format HH:MM (24 hour time)")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public Nullable<System.TimeSpan> EventPerformance_Time { get; set; }

        //public bool EventPerformance_Deleted { get; set; }

        //public virtual ICollection<EventBooking> EventBooking { get; set; }
        //public virtual Event Event { get; set; }
    }

    public class AddEventPerformanceViewModel
    {
        [Key]
        public int EventPerformance_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> EventPerformance_EventID { get; set; }

        //[Display(Name = "Event")]
        //public string EventPerformance_EventName { get; set; }  since this will be a partial view to the event creation..... don't need to show the event again 

        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EventPerformance_Date { get; set; }

        [Required(ErrorMessage = "Is required field. Format HH:MM (24 hour time)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:HH:mm}")]
        public Nullable<System.TimeSpan> EventPerformance_Time { get; set; }
    }
}