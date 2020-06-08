using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBook.Models.ViewModels
{
    public class CategoryVM
    {
        public SubCategory SubCategory { get; set; }
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
