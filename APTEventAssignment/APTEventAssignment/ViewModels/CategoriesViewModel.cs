using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace APTEventAssignment.ViewModels
{
    public class CategoriesViewModel
    {
        [Required]
        [Display(Name = "Category")]
        public string Category_Name { get; set; }
        public int Category_ID { get; set; }

    }
}