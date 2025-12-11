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
            return await RequestInternal(apiUrl, RequestType.post, httpRequestHeaders, sendObject);
        }

        public async Task<HttpResponseMessage> RequestPut(string apiUrl, object sendObject, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {
            return await RequestInternal(apiUrl, RequestType.put, httpRequestHeaders, sendObject);
        }

        public async Task<HttpResponseMessage> RequestGet(string apiUrl, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {
            return await RequestInternal(apiUrl, RequestType.get, httpRequestHeaders);
        }

        private async Task<HttpResponseMessage> RequestInternal(string apiUrl, RequestType requestType, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null, object ? sendObject=null)
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

                HttpResponseMessage apiResponse = new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                };

                if (requestType == RequestType.post || requestType == RequestType.put)
                {
                    if (sendObject == null)
                        return apiResponse;

                    var sendObjectAsJsonString = JsonConvert.SerializeObject(sendObject);
                    var apiBody = new StringContent(sendObjectAsJsonString, Encoding.UTF8, _mediaType);

                    if (requestType == RequestType.post)
                        apiResponse = await client.PostAsync(apiUrl, apiBody);

                    if (requestType == RequestType.put)
                        apiResponse = await client.PutAsync(apiUrl, apiBody);
                }
                else
                {
                    apiResponse = await client.GetAsync(apiUrl);
                }

                return apiResponse;
            }
        }
    }

}
