using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiExample.Controllers
{
    /// <summary>
    ///     Controller which override search behaviour and allow customers to hide their keys. This solutions is recommended for
    ///     sensitive data.
    /// </summary>
    public class CludoController : ApiController
    {
        //
        private string CustomerKey
        {
            get { return WebConfigurationManager.AppSettings["Cludo.CustomerKey"]; }
        }

        private const string CludoSearchApiUrl = "https://api.cludo.com/api/v3/"; // Add this url to your config file as well

        /// <summary>
        ///     Search script is using public settings to get settings which enable or disable Google or Siteimprove analytics
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        [Route("~/api/Cludo/{customerId:int}/{websiteId:int}/websites/publicsettings")]
        [HttpGet]
        public Task<HttpResponseMessage> PublicSettings(int customerId, int websiteId)
        {
            var requestUrl = string.Format("{0}/{1}/websites/publicsettings", customerId, websiteId);
            return MakeGetRequest(customerId, requestUrl);
        }

        /// <summary>
        ///     Method which used to get autosuggestions when user types in the search box
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="websiteId"></param>
        /// <param name="text"></param>
        /// <returns></returns>
        [Route("~/api/Cludo/{customerId:int}/{websiteId:int}/Autocomplete")]
        [HttpGet]
        public Task<HttpResponseMessage> Autocomplete(int customerId, int websiteId, string text)
        {
            var requestUrl = string.Format("{0}/{1}/Autocomplete?text={2}", customerId, websiteId, text);
            return MakeGetRequest(customerId, requestUrl);
        }

        /// <summary>
        ///     Actual search method
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="websiteId"></param>
        /// <returns></returns>
        [Route("~/api/Cludo/{customerId:int}/{websiteId:int}/Search")]
        [HttpPost]
        public async Task<HttpResponseMessage> Search(int customerId, int websiteId)
        {
            //In "ReadAndModifyRequest" you can append filters for ex. personalization
            var requestBody = await ReadAndModifyRequest();

            using (var client = GetClient(customerId))
            {
                var requestUrl = string.Format("{0}/{1}/search", customerId, websiteId);
                var result = await
                    client.PostAsync(requestUrl,
                        new StringContent(requestBody, Encoding.UTF8, "application/json"));
                return await ProcessResponse(result);
            }
        }

        private async Task<string> ReadAndModifyRequest()
        {
            var requestObject =  JObject.Parse(await Request.Content.ReadAsStringAsync());

            //::::PERSONALISATION
            SetPersonalisationFilters(requestObject);

            return JsonConvert.SerializeObject(requestObject);
        }

        private static void SetPersonalisationFilters(JObject requestObject)
        {
            //Get existing filters
            var filtersDict = GetDictionaryFromJObject(requestObject, "filters");

            //Add new filter
            //filtersDict.Add("UserRoles", new[] { "1337", "42" });
            
            //Update the body with the new filters
            requestObject["filters"] = JToken.FromObject(filtersDict);
        }

        private static Dictionary<string, string[]> GetDictionaryFromJObject(JObject requestObject, string property)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string[]>>((requestObject[property] ?? "").ToString())
                   ?? new Dictionary<string, string[]>();
        }


        [AllowAnonymous]
        [HttpOptions]
        public HttpResponseMessage Options()
        {
            return Request.CreateResponse(HttpStatusCode.OK);
        }

        /// <summary>
        ///     Simple get request to api
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> MakeGetRequest(int customerId, string url)
        {
            using (var client = GetClient(customerId))
            {
                var result = await client.GetAsync(url);
                return await ProcessResponse(result);
            }
        }

        /// <summary>
        ///     This method is simply reading response from api and send it back. We are highly recommending to return actual
        ///     response from our api, unless you really need to extend something.
        ///     Since search script will rely on message body to be as we expect, tampering with it could break the search.
        /// </summary>
        /// <param name="result"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> ProcessResponse(HttpResponseMessage result)
        {
            if (!result.IsSuccessStatusCode)
            {
                var errorMessage = await result.Content.ReadAsStringAsync();
                return Request.CreateErrorResponse(result.StatusCode, errorMessage);
            }

            var response = Request.CreateResponse(HttpStatusCode.OK);
            response.Content = new StringContent(await result.Content.ReadAsStringAsync(), Encoding.UTF8,
                "application/json");
            return response;
        }

        /// <summary>
        ///     Initialize http client with default settings and authorization key
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        private HttpClient GetClient(int customerId)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(CludoSearchApiUrl)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(Encoding.UTF8.GetBytes(customerId + ":" + CustomerKey)));
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }
    }
}