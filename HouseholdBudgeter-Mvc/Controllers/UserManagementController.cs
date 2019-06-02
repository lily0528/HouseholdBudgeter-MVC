using HouseholdBudgeter_Mvc.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HouseholdBudgeter_Mvc.Controllers
{
    public class UserManagementController : Controller
    {
        // GET: UserManagement
        public ActionResult Index()
        {
            return View();
        }
    [AllowAnonymous]
        [HttpGet]
        public ActionResult Register()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Register(RegisterBindingModel model)
        {
            //Url and parameters to post
            var url = "http://localhost:64873/api/Account/Register";
            var UserName = model.Email;
            var Email = model.Email;
            var Password = model.Password;
            var ConfirmPassword = model.ConfirmPassword;

            //HttpClient object to handle the comunication
            var httpClient = new HttpClient();

            //Parameters list with KeyValue pair
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("UserName", Email));
            parameters.Add(new KeyValuePair<string, string>("Email", Email));
            parameters.Add(new KeyValuePair<string, string>("Password", Password));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", ConfirmPassword));

            //Encoding the parameters before sending to the API
            var encodedParameters = new FormUrlEncodedContent(parameters);

            //Calling the API and storing the response
            var response = httpClient.PostAsync(url, encodedParameters).Result;


            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<RegisterBindingModel>(data);
                return RedirectToAction("Login", "UserManagement");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }
            return View();
        }

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(/*string returnUrl*/)
        {
            //ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var url = "http://localhost:64873/token";

            var userName = model.Email;
            var password = model.Password;
            var grantType = "password";

            var httpClient = new HttpClient();

            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("username", userName));
            parameters.Add(new KeyValuePair<string, string>("password", password));
            parameters.Add(new KeyValuePair<string, string>("grant_type", grantType));

            var encodedValues = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedValues).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;

                var result = JsonConvert.DeserializeObject<LoginData>(data);

                // Session["Token"] = result.AccessToken;

                var cookie = new HttpCookie("MyCookie");
                cookie.Values.Add("AccessToken", result.AccessToken);
                cookie.Values.Add("Username", result.Username);

                Response.Cookies.Add(cookie);

                return RedirectToAction("GetHouseholds", "Household");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);

                return View("informationError", result);
            }
            return View("Error");
        }

        [HttpPost]
        public ActionResult LogOff(/*string returnUrl*/)
        {
            if (Request.Cookies["MyCookie"] != null)
            {
                var c = new HttpCookie("MyCookie");
                // set cookie as expired, then the browser will log the user off.
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                return RedirectToAction("Login", "UserManagement");
            }
            return HttpNotFound();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }


        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ChangePasswordBindingModel model)
        {
            var cookie = Request.Cookies["MyCookie"];

            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/Account/ChangePassword";
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization",$"Bearer {token}");

            var OldPassword = model.OldPassword;
            var newPassword = model.NewPassword;
            var ConfirmPassword = model.ConfirmPassword;
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("OldPassword", OldPassword));
            parameters.Add(new KeyValuePair<string, string>("newPassword", newPassword));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", ConfirmPassword));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
             
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ChangePasswordBindingModel>(data);
                return RedirectToAction("Login", "UserManagement");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(ForgotPasswordViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            var url = "http://localhost:64873/api/Account/ForgotPassword";

            var Email = model.Email;
            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", Email));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ForgotPasswordViewModel>(data);
                return RedirectToAction("ResetPassword", "UserManagement");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View();
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.InternalServerError)
            {
                return View("Error");
            }
            return View();
        }

        public ActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var url = "http://localhost:64873/api/Account/ResetPassword";
            var Email = model.Email;
            var Password = model.Password;
            var ConfirmPassword = model.ConfirmPassword;
            var Code = model.Code;
            var httpClient = new HttpClient();
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Email", Email));
            parameters.Add(new KeyValuePair<string, string>("Password", Password));
            parameters.Add(new KeyValuePair<string, string>("ConfirmPassword", ConfirmPassword));
            parameters.Add(new KeyValuePair<string, string>("Code", Code));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ResetPasswordViewModel>(data);
                return RedirectToAction("Login", "UserManagement");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View("NotFound");
            }
            return View();
        }

    }
}