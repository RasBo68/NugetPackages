
using System.Net;
using System.Text;
using System.Text.Json;

namespace Coeo.Extensions.RestApiClient
{
    public class RestApiClientService : IRestApiClientService
    {
        private const string MEDIA_TYPE = "application/json";
        private const string HTTP_REQUEST_EXCEPTION_MESSAGE = "Http request failed! Detailed Informations:\nStatusCode: {0} \nHeaders: {1}\nContent: {2}" +
            "\nRequestMessage: {3}\nReasonPhrase: {4}\nVersion: {5}\nTrailingHeaders: {6}";

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
            if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrWhiteSpace(apiUrl))
                throw new InvalidOperationException("apiUrl is empty or just whitespace.");

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

                    var sendObjectAsJsonString = JsonSerializer.Serialize(sendObject);
                    var apiBody = new StringContent(sendObjectAsJsonString, Encoding.UTF8, MEDIA_TYPE);

                    if (requestType == RequestType.post)
                        apiResponse = await client.PostAsync(apiUrl, apiBody);

                    if (requestType == RequestType.put)
                        apiResponse = await client.PutAsync(apiUrl, apiBody);
                }
                else
                    apiResponse = await client.GetAsync(apiUrl);

                if (!apiResponse.IsSuccessStatusCode)
                {
                    throw new HttpRequestException(string.Format(HTTP_REQUEST_EXCEPTION_MESSAGE, 
                        apiResponse.StatusCode, 
                        apiResponse.Headers, 
                        apiResponse.Content, 
                        apiResponse.RequestMessage,
                        apiResponse.ReasonPhrase,
                        apiResponse.Version,
                        apiResponse.TrailingHeaders));
                }

                return apiResponse;
            }
        }
    }

}
