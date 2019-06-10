using HouseholdBudgeter_Mvc.Models;
using HouseholdBudgeter_Mvc.Models.BankAccount;
using HouseholdBudgeter_Mvc.Models.Household;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HouseholdBudgeter_Mvc.Controllers
{
    public class BankAccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "UserManagement");
            }

            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/household/GetHouseholdsSelectList";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var households = JsonConvert.DeserializeObject<List<HouseholdView>>(data);
                var model = new BankAccountBindingModel();
                model.Household = new SelectList(households, "Id", "Name");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(BankAccountBindingModel model)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var token = cookie.Values["AccessToken"];
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("HouseholdId", model.HouseholdId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", model.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", model.Description));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PostAsync("http://localhost:64873/api/BankAccount/create", encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetBankAccounts");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var errors = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                foreach (var key in errors.ModelState)
                {
                    foreach (var error in key.Value)
                    {
                        ModelState.AddModelError(key.Key, error);
                    }
                }
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "An unexpected error has occured. Please try again later");
                return View(model);
            }
        }


        [HttpGet]
        public ActionResult Edit(int id)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var token = cookie.Values["AccessToken"];
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = httpClient.GetAsync($"http://localhost:64873/api/BankAccount/Edit/{id}").Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<BankAccountView>(data);
                if (!result.IsOwner)
                {
                    return RedirectToAction("GetCategory");
                }
                var editViewModel = new BankAccountBindingModel();
                editViewModel.HouseholdId = result.HouseholdId;
                editViewModel.Name = result.Name;
                editViewModel.Description = result.Description;
                return View(editViewModel);
            }
            else
            {
                return RedirectToAction("GetCategory");
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, EditBankAccountModel model)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var token = cookie.Values["AccessToken"];
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("HouseholdId", model.HouseholdId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Name", model.Name));
            parameters.Add(new KeyValuePair<string, string>("Description", model.Description));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PutAsync($"http://localhost:64873/api/BankAccount/Edit/{id}", encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetBankAccounts");
            }

            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var errors = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                foreach (var key in errors.ModelState)
                {
                    foreach (var error in key.Value)
                    {
                        ModelState.AddModelError(key.Key, error);
                    }
                }
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "An unexpected error has occured. Please try again later");
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult GetBankAccounts()
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/BankAccount/ViewBankAccount";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<BankAccountView>>(data);
                return View("GetBankAccounts", model);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }

        public ActionResult Delete(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("GetCategory");
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }
            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/BankAccount/Delete/{id}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = httpClient.DeleteAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetBankAccounts");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View("informationError", result);
            }
            else
            {
                return RedirectToAction("GetBankAccounts");
            }
        }

        
        public ActionResult CalculateBalance(int id)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/BankAccount/CalculateBalance/{id}";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Id", id.ToString()));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return View("GetBankAccounts");
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }
    }
}