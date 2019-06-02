using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HouseholdBudgeter_Mvc.Helpers
{
    public class IdentityHelper
    {
        string GetToken(HttpCookieCollection cookies)
        {
            var cookie = cookies["MyCookie"];
            if (cookie == null)
            {
                return null;
            }
            return cookies["MyCookie"].Values["AccessToken"];
        }
    }
}