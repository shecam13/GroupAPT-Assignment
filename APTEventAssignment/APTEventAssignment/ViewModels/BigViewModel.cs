using APTEventAssignment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace APTEventAssignment.ViewModels
{
    public class BigViewModel
    {
        [Key]
        public int Event_ID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string Event_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        //[Display(Name = "Event Venue")]
        public int Event_VenueID { get; set; }


        [Display(Name = "Event Rating")]
        public string Event_Rating { get; set; }

        [HiddenInput(DisplayValue = false)]
        //[Display(Name = "Event Category")]
        public int Event_CategoryID { get; set; }


        [Display(Name = "Venue")]
        public string Event_VenueName { get; set; }

        [Display(Name = "Category")]
        public string Event_CategoryName { get; set; }

        [Display(Name = "Image")]
        public byte[] Event_Image { get; set; }


        //Event PERFORMANCES

        [Key]
        public int EventPerformance_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> EventPerformance_EventID { get; set; }


        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EventPerformance_Date { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "Is required field. Format hh:mm (24 hour time)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm\:ss}")]
        public Nullable<System.TimeSpan> EventPerformance_Time { get; set; }
    
    }

    public class AddBigViewModel
    {
        [Key]
        public int Event_ID { get; set; }

        [Required]
        [Display(Name = "Event Name")]
        public string Event_Name { get; set; }

        [Required]
        [Display(Name = "Event Venue")]
        public int Event_VenueID { get; set; }

        [Display(Name = "Event Rating")]
        public string Event_Rating { get; set; }

        [Required]
        [Display(Name = "Event Category")]
        public int Event_CategoryID { get; set; }

        public byte[] Event_Image { get; set; }

        //Event PERFORMANCES
        [Key]
        public int EventPerformance_ID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> EventPerformance_EventID { get; set; }

        
        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        //[DisplayFormat(DataFormatString = "{0:dd MMMM yyyy}", ApplyFormatInEditMode = true)]
        public Nullable<System.DateTime> EventPerformance_Date { get; set; }

        [Display(Name = "Time")]
        [Required(ErrorMessage = "Is required field. Format hh:mm (24 hour time)")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = @"{0:hh\:mm\:ss}")]
        public Nullable<System.TimeSpan> EventPerformance_Time { get; set; }
    }
}