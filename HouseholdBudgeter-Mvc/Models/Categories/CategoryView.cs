using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Categories
{
    public class CategoryView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int HouseholdId { get; set; }
        public bool IsOwner { get; set; }
    }
}