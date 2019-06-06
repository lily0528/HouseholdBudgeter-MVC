using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Transaction
{
    public class TransactionView
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public string BankAccountName { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool IsOwner { get; set; }
    }
}