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

        [Key]
        public int Venue_ID { get; set; }

        [Required]
        [Display(Name = "Venue Name")]
        public string Venue_Name { get; set; }

        [Display(Name = "Address")]
        public string Venue_Address { get; set; }

        [Display(Name = "Capacity")]
        public Nullable<int> Venue_Capacity { get; set; }

        [HiddenInput(DisplayValue = false)]
        public Nullable<int> Venue_VenueTypeID { get; set; }

        [Display(Name = "Venue Type")]
        public string VenueType_Name { get; set; }
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

    }
}