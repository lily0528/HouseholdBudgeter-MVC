using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Models.Household
{
    public class HouseholdView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfUsers { get; set; }
        public bool IsOwner { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        //public int Id { get; set; }
        //public string Name { get; set; }
        //public string Description { get; set; }
        //public DateTime Created { get; set; }
        //public string CreatorId { get; set; }
        //public string CreatorName { get; set; }
    }
}