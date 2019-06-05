using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Categories
{
    public class EditCategoryModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public int HouseholdId { get; set; }
    }
}