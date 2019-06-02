using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HouseholdBudgeter_Mvc.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (Request.Cookies["MyCookie"]?.Values["AccessToken"] != null)
            { 
            return RedirectToAction("GetHouseholds", "Household");
            }
            else
            {
                return RedirectToAction("Login", "UserManagement");
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}