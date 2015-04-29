using APTEventAssignment.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class VenueViewModel
    {
        //public VenueViewModel()
        //{
        //    this.Event = new HashSet<Event>();
        //    this.VenueZone = new HashSet<VenueZone>();
        //}

        [Key]
        public int Venue_ID { get; set; }

        [Required]
        [Display(Name = "Venue Name")]
        public string Venue_Name { get; set; }

        [Display(Name = "Address")]
        public string Venue_Address { get; set; }

        [Display(Name = "Capacity")]
        public Nullable<int> Venue_Capacity { get; set; }

        //[Display(Name = "Venue Type ID")]
        //public Nullable<int> Venue_VenueTypeID { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> Venue_VenueTypeID { get; set; }

        [Display(Name = "Venue Type")]
        public string VenueType_Name { get; set; }

        //[Display(Name = "Deleted")]
        //public bool Venue_Deleted { get; set; }

        //public virtual ICollection<Event> Event { get; set; }
        //public virtual VenueType VenueType { get; set; }
        //public virtual ICollection<VenueZone> VenueZone { get; set; }
    }

    public class AddVenueViewModel
    {
        [Key]
        public int Venue_ID { get; set; }

        [Required]
        [Display(Name = "Venue Name")]
        public string Venue_Name { get; set; }

        [Display(Name = "Address")]
        public string Venue_Address { get; set; }

        [Display(Name = "Capacity")]
        public Nullable<int> Venue_Capacity { get; set; }

        [Display(Name = "Venue Type ID")]
        public Nullable<int> Venue_VenueTypeID { get; set; }

        //[Display(Name = "Type")]
        //public string VenueType_Name { get; set; }

        //[Display(Name = "Deleted")]
        //public bool Venue_Deleted { get; set; }

    }
}