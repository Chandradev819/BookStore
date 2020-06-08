using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Text;

namespace BulkyBook.Models.ViewModels
{
    public class SubVm
    {

        public SubCategory SubCategory { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
       
    }
}
