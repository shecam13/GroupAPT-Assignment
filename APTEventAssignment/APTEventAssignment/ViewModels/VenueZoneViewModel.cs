
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class VenueZoneViewModel
    {
        [Key]
        public int VenueZone_ID { get; set; }

        [Required]
        [Display(Name="Zone Name")]
        public string VenueZone_Name { get; set; }

        [HiddenInput(DisplayValue = false)]
        public int VenueZone_VenueID { get; set; }

        [Required]
        [Display(Name = "Venue")]
        public String VenueZone_VenueName { get; set; }

        [Required]
        [Display(Name = "Zone Code")]
        public string VenueZone_Code { get; set; }

        [Required]
        [Display(Name = "Zone Rows")]
        public int VenueZone_Rows { get; set; } 
  
        [Required]
        [Display(Name = "Zone Columns")]
        public int VenueZone_Columns { get; set; }

    }

    public class AddVenueZoneViewModel
    {
        [Key]
        public int VenueZone_ID { get; set; }

        [Required]
        [Display(Name="Venue Zone Name")]
        public string VenueZone_Name { get; set; }


        [HiddenInput(DisplayValue = false)]
        public int VenueZone_VenueID { get; set; }

        [Required]
        [Display(Name = "Zone Code")]
        public string VenueZone_Code { get; set; }

        [Display(Name = "Zone Rows")]
        public int VenueZone_Rows { get; set; } 
  
        [Display(Name = "Zone Columns")]
        public int VenueZone_Columns { get; set; }

    }
}
