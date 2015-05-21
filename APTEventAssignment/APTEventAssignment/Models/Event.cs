//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace APTEventAssignment.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Event
    {
        public Event()
        {
            this.EventPerformance = new HashSet<EventPerformance>();
            this.EventVenueZone = new HashSet<EventVenueZone>();
        }
    
        public int Event_ID { get; set; }
        public string Event_Name { get; set; }
        public int Event_VenueID { get; set; }
        public string Event_Rating { get; set; }
        public bool Event_Deleted { get; set; }
        public int Event_CategoryID { get; set; }
        public byte[] Event_Image { get; set; }
    
        public virtual Category Category { get; set; }
        public virtual Venue Venue { get; set; }
        public virtual ICollection<EventPerformance> EventPerformance { get; set; }
        public virtual ICollection<EventVenueZone> EventVenueZone { get; set; }
        public virtual Event Event1 { get; set; }
        public virtual Event Event2 { get; set; }
    }
}
