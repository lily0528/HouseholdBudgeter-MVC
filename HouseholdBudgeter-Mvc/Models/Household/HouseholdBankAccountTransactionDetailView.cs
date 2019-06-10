using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Household
{
    public class HouseholdBankAccountTransactionDetailView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string CategoryName { get; set; }
    }
}