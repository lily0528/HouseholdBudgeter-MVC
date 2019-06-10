using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Transaction
{
    public class HouseholdBankAccountTransactionDetailView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string CategoryName { get; set; }
        public int CategoryId { get; set; }
    }
}