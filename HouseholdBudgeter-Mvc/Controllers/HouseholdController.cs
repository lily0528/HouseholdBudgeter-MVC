using HouseholdBudgeter_Mvc.Models;
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
    public class HouseholdController : Controller
    {
        // GET: Household
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(HouseholdBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/household/create";
            var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var Name = model.Name;
            var Description = model.Description;
            var parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("Name", Name));
            parameters.Add(new KeyValuePair<string, string>("Description", Description));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<HouseholdView>(data);
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View();
            }
            return View("Error");
        }

        [HttpGet]
        public ActionResult GetHouseholds()
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = "http://localhost:64873/api/household/GetHouseholds";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var model = JsonConvert.DeserializeObject<List<HouseholdView>>(data);
            return View("GetHouseholds", model);
        }

        [HttpGet]
        public ActionResult EditHousehold(int? id)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/household/edit/{id}";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var model = JsonConvert.DeserializeObject<HouseholdBindingModel>(data);

            return View("EditHousehold", model);
        }

        [HttpPost]
        public ActionResult EditHousehold(int id, HouseholdBindingModel model)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/household/edit/{id}";
            var Name = model.Name;
            var Description = model.Description;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("Name", Name));
            parameters.Add(new KeyValuePair<string, string>("Description", Description));

            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PutAsync(url, encodedParameters).Result;

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
 
                return View("Error");
        }

        [HttpGet]
        public ActionResult GetHouseholdMembers(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/Household/GetHouseholdMembers/{id}";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var data = httpClient.GetStringAsync(url).Result;
            var model = JsonConvert.DeserializeObject<List<UsersView>>(data);
            return View("GetHouseholdMembers", model);
        }

        [HttpGet]
        public ActionResult EmailInvitation(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
            return View("EmailInvitation");
        }

        [HttpPost]
        public ActionResult EmailInvitation(int? id, InvitationBindingModelcs model)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/Invitation/EmailInvitation";
            var Email = model.Email;
            var householdId = id;
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("Email", Email));
            parameters.Add(new KeyValuePair<string, string>("householdId", householdId.ToString()));
            var encodedParameters = new FormUrlEncodedContent(parameters);

            var response = httpClient.PostAsync(url, encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("GetHouseholds", "Household");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View("informationError",result);
            }
                return View("Error");
        }

        [HttpGet]
        public ActionResult JoinHousehold()
        {
            return View();
        }

        [HttpPost]
        public ActionResult JoinHousehold(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/Invitation/AcceptInvitation/{id}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("householdId", id.ToString()));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, encodedParameters).Result;
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                return RedirectToAction("GetHouseholds", "Household");
            }
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);
                return View("informationError", result);
            }
            return View("Error");
        }

        [HttpGet]
        public ActionResult Leave(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Values["AccessToken"];
            var url = $"http://localhost:64873/api/Household/MemberLeave/{id}";
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            var parameters = new List<KeyValuePair<string, string>>();

            parameters.Add(new KeyValuePair<string, string>("householdId", id.ToString()));
            var encodedParameters = new FormUrlEncodedContent(parameters);
            var response = httpClient.PostAsync(url, encodedParameters).Result;
           
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<HouseholdView>(data);
                return RedirectToAction(nameof(HouseholdController.GetHouseholds));
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                var data = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<ApiErrorMessage>(data);

                return View("informationError",result);
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return View("NotFound");
            }
            return View("Error");
           
        }
    }
}