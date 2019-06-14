using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Household
{
    public class CategoryGroupView
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public decimal CategoryAmount { get; set; }
        //public List<HouseholdBankAccountTransactionDetailView> Transactions { get; set; }

        //public CategoryGroupView()
        //{
        //    Transactions = new List<HouseholdBankAccountTransactionDetailView>();
        //}
    }
}