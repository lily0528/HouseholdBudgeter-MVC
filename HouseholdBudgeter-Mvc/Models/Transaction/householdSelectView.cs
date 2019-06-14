using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseholdBudgeter_Mvc.Models.Transaction
{
    public class HouseholdSelectView
    {
        public int HouseholdId { get; set; }
        public SelectList Household { get; set; }
    }
}