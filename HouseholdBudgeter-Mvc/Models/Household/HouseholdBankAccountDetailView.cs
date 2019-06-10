using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Household
{
    public class HouseholdBankAccountDetailView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public List<CategoryGroupView> Categorys { get; set; }

        public HouseholdBankAccountDetailView()
        {
            Categorys = new List<CategoryGroupView>();
        }

    }
}