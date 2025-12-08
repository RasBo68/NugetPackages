using Newtonsoft.Json;
using RGuide.Fundamental.WebApiInteraction.Models;
using System.Net;
using System.Text;

namespace RestApiClient
{
    public class RestApiClientService : IRestApiClientService
    {
        private readonly string _mediaType = "application/json";

        public async Task<HttpResponseMessage> RequestPost(string apiUrl, object sendObject, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {

            using (var client = new HttpClient())
            {
                if (httpRequestHeaders != null)
                {
                    foreach (var httpRequestHeader in httpRequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(httpRequestHeader.Name, httpRequestHeader.Value);
                    }
                }

                if (sendObject == null)
                {
                    return new HttpResponseMessage()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                    };
                }

                var sendObjectAsJsonString = JsonConvert.SerializeObject(sendObject);
                var apiBody = new StringContent(sendObjectAsJsonString, Encoding.UTF8, _mediaType);

                var apiResponse = await client.PostAsync(apiUrl, apiBody);
                return apiResponse;
            }
        }

        public async Task<HttpResponseMessage> RequestGet(string apiUrl, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {
            using (var client = new HttpClient())
            {
                if (httpRequestHeaders != null)
                {
                    foreach (var httpRequestHeader in httpRequestHeaders)
                    {
                        client.DefaultRequestHeaders.Add(httpRequestHeader.Name, httpRequestHeader.Value);
                    }
                }

                var apiResponse = await client.GetAsync(apiUrl);
                return apiResponse;
            }
        }
    }

}
