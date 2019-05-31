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

            var token = cookie.Value;
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
                //Todo: No data return
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

        [HttpGet]
        public ActionResult GetHouseholds()
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Value;
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

            var token = cookie.Value;
            var url = $"http://localhost:64873/api/household/edit/{id}";

            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var data = httpClient.GetStringAsync(url).Result;

            var model = JsonConvert.DeserializeObject<HouseholdBindingModel>(data);
           
            return View("EditHousehold",model);
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

            var token = cookie.Value;
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

            var data = response.Content.ReadAsStringAsync().Result;

            //var result = JsonConvert.DeserializeObject<HouseholdView>(data);

            return RedirectToAction("GetHouseholds", "Household");
        }

        [HttpGet]
        public ActionResult GetHouseholdMembers(int? id)
        {
            var cookie = Request.Cookies["MyCookie"];
            if (cookie == null)
            {
                return View();
            }

            var token = cookie.Value;
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
            return View();
        }

    }
}