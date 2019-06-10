using HouseholdBudgeter_Mvc.Models;
using HouseholdBudgeter_Mvc.Models.BankAccount;
using HouseholdBudgeter_Mvc.Models.Categories;
using HouseholdBudgeter_Mvc.Models.Household;
using HouseholdBudgeter_Mvc.Models.Transaction;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace HouseholdBudgeter_Mvc.Controllers
{
    public class TransactionController : Controller
    {
        // GET: Transaction
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult CreateByHouseholdId()
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/household/GetHouseholds";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var households = JsonConvert.DeserializeObject<List<HouseholdView>>(data);
                var model = new householdSelectView();
                 model.Household= new SelectList(households, "Id", "Name");
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
            
        }

        [HttpPost]
        public ActionResult CreateByHouseholdId(int HouseholdId)
        {
            return RedirectToAction("Create", "Transaction", new { id = HouseholdId });
        }

        //[HttpPost]
        //public ActionResult CreateByHouseholdId()
        //{
        //    var cookie = Request.Cookies["MyCookie"];
        //    if (cookie == null)
        //    {
        //        return RedirectToAction("Login", "Account");
        //    }
        //    if (!ModelState.IsValid)
        //    {
        //        return View(model);
        //    }
        //    var token = cookie.Values["AccessToken"];
        //    var parameters = new List<KeyValuePair<string, string>>();
        //    parameters.Add(new KeyValuePair<string, string>("HouseholdId", model.HouseholdId.ToString()));
        //    parameters.Add(new KeyValuePair<string, string>("Name", model.Name));
        //    parameters.Add(new KeyValuePair<string, string>("Description", model.Description));
        //    var encodedParameters = new FormUrlEncodedContent(parameters);
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            //    var response = httpClient.PostAsync("http://localhost:64873/api/BankAccount/create", encodedParameters).Result;
            //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
            //    {
            //        return RedirectToAction("GetBankAccounts");
            //    }

            //    else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            //    {
            //        var data = response.Content.ReadAsStringAsync().Result;
            //        var errors = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
            //        foreach (var key in errors.ModelState)
            //        {
            //            foreach (var error in key.Value)
            //            {
            //                ModelState.AddModelError(key.Key, error);
            //            }
            //        }
            //        return View(model);
            //    }
            //    else
            //    {
            //        ModelState.AddModelError("", "An unexpected error has occured. Please try again later");
            //        return View(model);
            //    }

            //}

        [HttpGet]
        public ActionResult Create(int id)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("login", "UserManagement");
            }

            var token = cookie.Values["AccessToken"];
            var GetBankAccountUrl = $"http://localhost:64873/api/BankAccount/GetBankAccountsSelectList/{id}";
            var GetCategoryUrl = $"http://localhost:64873/api/Category/GetCategoriesSelectList/{id}";

            var GetBankAccountHttpClient = new HttpClient();
            var GetCategoryHttpClient = new HttpClient();

            GetBankAccountHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            GetCategoryHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var GetBankAccountResponse = GetBankAccountHttpClient.GetAsync(GetBankAccountUrl).Result;
            var GetCategoryResponse = GetCategoryHttpClient.GetAsync(GetCategoryUrl).Result;
            if (GetBankAccountResponse.StatusCode == System.Net.HttpStatusCode.OK && GetCategoryResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var model = new TransactionBindingModel();
                var bankAccountData = GetBankAccountResponse.Content.ReadAsStringAsync().Result;
                var bankAccounts = JsonConvert.DeserializeObject<List<BankAccountView>>(bankAccountData);

                var categoryData = GetCategoryResponse.Content.ReadAsStringAsync().Result;
                var categories = JsonConvert.DeserializeObject<List<CategoryView>>(categoryData);

                model.BankAccount = new SelectList(bankAccounts, "Id", "Name");
                model.Category = new SelectList(categories, "Id", "Name");
                model.Date = DateTime.Now;
                return View(model);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }

        [HttpPost]
        public ActionResult Create(TransactionBindingModel model)
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
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", model.BankAccountId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", model.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Title", model.Title));
            parameters.Add(new KeyValuePair<string, string>("Date", model.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", model.Amount.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Description", model.Description));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var url = "http://localhost:64873/api/Transaction/Create";

            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PostAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetTransactions");
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
        public ActionResult GetTransactions()
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return RedirectToAction("Login", "Account");
            }
            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/Transaction/ViewTransactions";


            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<TransactionView>>(data);
                return View("GetTransactions", model);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
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
            var url = $"http://localhost:64873/api/Transaction/Edit/{id}";
            var GetBankAccountUrl = "http://localhost:64873/api/BankAccount/GetBankAccountsSelectList";
            var GetCategoryUrl = "http://localhost:64873/api/Category/GetCategoriesSelectList";

            var httpClient = new HttpClient();
            var GetBankAccountHttpClient = new HttpClient();
            var GetCategoryHttpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            GetBankAccountHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            GetCategoryHttpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.GetAsync(url).Result;
            var GetBankAccountResponse = GetBankAccountHttpClient.GetAsync(GetBankAccountUrl).Result;
            var GetCategoryResponse = GetCategoryHttpClient.GetAsync(GetCategoryUrl).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK && GetBankAccountResponse.StatusCode == System.Net.HttpStatusCode.OK && GetCategoryResponse.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<TransactionView>(data);

                var bankAccountData = GetBankAccountResponse.Content.ReadAsStringAsync().Result;
                var bankAccounts = JsonConvert.DeserializeObject<List<BankAccountView>>(bankAccountData);

                var categoryData = GetCategoryResponse.Content.ReadAsStringAsync().Result;
                var categories = JsonConvert.DeserializeObject<List<CategoryView>>(categoryData);
                if (!result.IsOwner)
                {
                    return RedirectToAction("GetTransactions");
                }
                var model = new EditTransactionBindingModel();
                model.BankAccount = new SelectList(bankAccounts, "Id", "Name");
                model.Category = new SelectList(categories, "Id", "Name");
                model.BankAccountId = result.BankAccountId;
                model.CategoryId = result.CategoryId;
                model.Title = result.Title;
                model.Description = result.Description;
                model.Date = result.Date;
                model.Amount = result.Amount;
                return View(model);
            }
            else
            {
                return RedirectToAction("GetTransactions");
            }
        }

        [HttpPost]
        public ActionResult Edit(int id, EditTransactionBindingModel model)

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
            parameters.Add(new KeyValuePair<string, string>("BankAccountId", model.BankAccountId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("CategoryId", model.CategoryId.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Title", model.Title));
            parameters.Add(new KeyValuePair<string, string>("Date", model.Date.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Amount", model.Amount.ToString()));
            parameters.Add(new KeyValuePair<string, string>("Description", model.Description));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = httpClient.PutAsync($"http://localhost:64873/api/Transaction/Edit/{id}", encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetTransactions");
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

        //[HttpPost]
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
            var url = $"http://localhost:64873/api/Transaction/Delete/{id}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = httpClient.DeleteAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetTransactions");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View("informationError", result);
            }
            else
            {
                return RedirectToAction("GetTranactions");
            }
        }

        //[HttpPost]
        public ActionResult VoidTransaction(int? id)
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
            var url = $"http://localhost:64873/api/Transaction/VoidTransaction/{id}";
          
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("IfVoid", false.ToString()));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = httpClient.PutAsync(url, encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return RedirectToAction("GetTransactions");
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View("informationError", result);
            }
            else
            {
                return RedirectToAction("GetTranactions");
            }
        }

        [HttpGet]
        public ActionResult BankAccountDetails(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("GetHouseholds");
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }
            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/Household/BankAccountDetails/{id}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var response = httpClient.GetAsync(url).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var model = JsonConvert.DeserializeObject<List<HouseholdBankAccountDetailView>>(data);
                return View("BankAccountDetails", model);
            }
            else
            {
                ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
                return View();
            }
        }

        //[HttpGet]
        //public ActionResult BankAccountDetails(int? id)
        //{
        //    if (!id.HasValue)
        //    {
        //        return RedirectToAction("GetHouseholds");
        //    }
        //    var cookie = Request.Cookies["MyCookie"];
        //    if (cookie == null)
        //    {
        //        return View();
        //    }
        //    var token = cookie.Values["AccessToken"];
        //    var url = $"http://localhost:64873/api/Household/BankAccountDetails/{id}";
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        //    var response = httpClient.GetAsync(url).Result;
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var data = response.Content.ReadAsStringAsync().Result;
        //        var model = JsonConvert.DeserializeObject<List<ViewBankAccountView>>(data);
        //        return View("BankAccountDetails", model);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
        //        return View();
        //    }
        //}

        //[HttpGet]
        //public ActionResult TransactionDetails(int? id)
        //{
        //    if (!id.HasValue)
        //    {
        //        return RedirectToAction("GetHouseholds");
        //    }
        //    var cookie = Request.Cookies["MyCookie"];
        //    if (cookie == null)
        //    {
        //        return View();
        //    }
        //    var token = cookie.Values["AccessToken"];
        //    var url = $"http://localhost:64873/api/Household/TransactionDetails/{id}";
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        //    var response = httpClient.GetAsync(url).Result;
        //    if (response.StatusCode == System.Net.HttpStatusCode.OK)
        //    {
        //        var data = response.Content.ReadAsStringAsync().Result;
        //        var model = JsonConvert.DeserializeObject<List<HouseholdBankAccountTransactionDetailView>>(data);

        //        var queryCategory = from item in model group item by item.CategoryName into newGroup orderby newGroup.Key select newGroup;
        //        ViewBag.transaction = "";
        //         foreach (var nameGroup in queryCategory)
        //        {
        //            decimal totalAmount = 0;
        //            ViewBag.transaction += $"<div><strong>Categroy Name:</strong> {nameGroup.Key}</div>";
        //            foreach (var record in nameGroup)
        //            {
        //                ViewBag.transaction += $"<tr>" +
        //                    $"<td> { record.Id} </td >" +
        //                    $"<td> { record.Title} </td>" +
        //                    $"<td> { record.Amount} </td>" +
        //                    $"<td> { record.CategoryName}</td></tr >";
        //                totalAmount += record.Amount;
        //            }
        //            ViewBag.transaction += $"<strong>Total Amount:</strong> {totalAmount}";
        //        }
        //        return View("TransactionDetails");

        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Sorry. An unexpected error has occured. Please try again later");
        //        return View();
        //    }
    }
    }
