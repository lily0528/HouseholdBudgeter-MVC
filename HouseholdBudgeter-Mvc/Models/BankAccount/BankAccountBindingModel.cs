using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseholdBudgeter_Mvc.Models.BankAccount
{
    public class BankAccountBindingModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int HouseholdId { get; set; }
        public SelectList Household { get; set; }
    }
}