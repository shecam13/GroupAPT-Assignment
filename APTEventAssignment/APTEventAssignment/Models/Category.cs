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
    
    public partial class Category
    {
        public Category()
        {
            this.Event = new HashSet<Event>();
        }
    
        public int Category_ID { get; set; }
        public string Category_Name { get; set; }
    
        public virtual ICollection<Event> Event { get; set; }
        public virtual Category Category1 { get; set; }
        public virtual Category Category2 { get; set; }
    }
}
