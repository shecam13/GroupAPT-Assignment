using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace APTEventAssignment.ViewModels
{
    public class EventBookingSeatsViewModel
    {
        public int SelectPerformanceId { get; set; }
        //public SelectList Performances { get; set; }
        public IEnumerable<SelectListItem> Performances { get; set; }
        //public List<String> EventBookingSeat_SeatIdentifier { get; set; }
    }
}