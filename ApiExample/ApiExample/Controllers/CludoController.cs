using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Newtonsoft.Json;

namespace ApiExample.Controllers
{
    /// <summary>
    ///     Controller which override search behaviour and allow customers to hide their keys. This solutions is recommended for
    ///     sensitive data.
    /// </summary>
    public class CludoController : ApiController
    {
        //
        private const string CustomerKey = "__CUSTOMER_KEY__"; // Bonus points: Add it to your config file
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
            return MakeGetRequest(customerId, websiteId, requestUrl);
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
            return MakeGetRequest(customerId, websiteId, requestUrl);
        }

        /// <summary>
        ///     Actual search method
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="websiteId"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Route("~/api/Cludo/{customerId:int}/{websiteId:int}/Search")]
        [HttpPost]
        public async Task<HttpResponseMessage> Search(int customerId, int websiteId, [FromBody] SearchRequest request)
        {
            using (var client = GetClient(websiteId))
            {
                var requestUrl = string.Format("{0}/{1}/search", customerId, websiteId);
                var result = await
                    client.PostAsync(requestUrl,
                        new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
                return await ProcessResponse(result);
            }
        }

        /// <summary>
        ///     Simple get request to api
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="websiteId"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> MakeGetRequest(int customerId, int websiteId, string url)
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
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid request");

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

    /// <summary>
    ///     Class which represents search request.
    /// </summary>
    public class SearchRequest
    {
        public SearchRequest()
        {
            Page = 1;
            Facets = new Dictionary<string, string[]>();
        }

        public string ResponseType
        {
            get { return "JsonHtml"; }
        }

        public string Query { get; set; }
        public string Template { get; set; }

        /// <summary>
        ///     Filter ONLY results on fields and does not affect facets statititcs
        /// </summary>
        public Dictionary<string, string[]> Facets { get; set; }
        /// <summary>
        /// Filter results and facets on that particular field values
        /// </summary>
        public Dictionary<string, string[]> Filters { get; set; }

        public int? PerPage { get; set; }
        public string DefaultOperator { get; set; }
        public int Page { get; set; }
    }
}