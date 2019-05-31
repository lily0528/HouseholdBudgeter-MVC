using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models
{
    public class ApiErrorMessage
    {
        public string message { get; set; }
        public Dictionary<string, string[]> ModelState { get; set; }
    }
}