using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.BankAccount
{
    public class BankAccountView
    {
        public int Id { get; set; }
        [Required]
        public int HouseholdId { get; set; }
        public string HouseholdName { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Balance { get; set; }
        public bool IsOwner { get; set; }
    }
}