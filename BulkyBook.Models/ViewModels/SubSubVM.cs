using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Models.ViewModels
{
    public class SubSubVM
    {
        public Subss Subss { get; set; }
        public IEnumerable<SelectListItem> Categorylist { get; set; }
        public IEnumerable<SelectListItem> SubCategorylist { get; set; }
    }
}
