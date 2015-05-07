using APTEventAssignment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class EventsViewModel
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
        //public Nullable<byte[]> Image { get; set; }

        //public virtual Venue Venue { get; set; }
        //public virtual ICollection<EventPerformance> EventPerformance { get; set; }
        //public virtual ICollection<EventVenueZone> EventVenueZone { get; set; }
        //public virtual Category Category { get; set; }
    }

    public class AddEventViewModel
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
        //public Nullable<byte[]> Image { get; set; }
    }

    public class EventsDetailsViewModel
    {
        [Required]
        [Display(Name = "Event Name")]
        public string Event_Name { get; set; }

        //[HiddenInput(DisplayValue = false)]
        //public int Event_VenueID { get; set; }

        [Display(Name = "Venue")]
        public string Event_VenueName { get; set; }

        [Display(Name = "Event Rating")]
        public string Event_Rating { get; set; }

        //[HiddenInput(DisplayValue = false)]
        //public int Event_CategoryID { get; set; }

        [Display(Name = "Category")]
        public string Event_CategoryName { get; set; }

        public byte[] Event_Image { get; set; }

        public List<EventPerformance> Event_Performances { get; set; }
        //public List<System.DateTime> Event_Performances { get; set; }
        //public Nullable<System.DateTime> EventPerformance_Date { get; set; }
    }
}