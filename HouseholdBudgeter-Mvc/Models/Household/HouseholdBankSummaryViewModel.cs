using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Household
{
    public class HouseholdBankSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<BankAccountSummary> BankAccounts { get; set; }
        public decimal TotalBalance { get; set; }
        public List<CategorySummary> Categories { get; set; }
        public decimal TotalSum { get; set; }
    }

    public class BankAccountSummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
    }

    public class CategorySummary
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Sum { get; set; }
    }
}