using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class EventBookingSeatsViewModel
    {
        [Required]
        [Display(Name = "Event Name")]
        public string Event_Name { get; set; }

        [Display(Name = "Venue")]
        public string Event_VenueName { get; set; }

        public string PhoneNumber { get; set; }

        public List<String> SeatIdentifiers { get; set; }

        public int SelectPerformanceId { get; set; }

        public IEnumerable<SelectListItem> Performances { get; set; }

    }
}