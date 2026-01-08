
using Coeo.Converters.Json;
using Coeo.Extensions.RestApiClient.Exceptions;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json;

namespace Coeo.Extensions.RestApiClient
{
    public class RestApiClientService : IRestApiClientService
    {
        private readonly IJsonConverter _jsonConverter;

        private const string MEDIA_TYPE = "application/json";
        private const string API_URL_STRING_EMPTY_OR_WHITESPACE = "ApiUrl string is empty or just whitespace.";
        private const string REST_REQUEST_FAILED_EXCEPTION_MESSAGE = "Http request failed! Detailed Informations:\nStatusCode: {0} \nHeaders: {1}\nContent: {2}" +
            "\nRequestMessage: {3}\nReasonPhrase: {4}\nVersion: {5}\nTrailingHeaders: {6}\nObject to send: {7}";
        private const string REST_REQUEST_PROBLEM_EXCEPTION_MESSAGE2 = "Http request to the api url {0} failed during network issues.";

        public RestApiClientService()
        {
            _jsonConverter = new JsonConverter();
        }

        public async Task<HttpResponseMessage> RequestPost(string apiUrl, object sendObject, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {
            return await RequestInternal(apiUrl, RequestType.post, cancellationToken, httpRequestHeaders, sendObject);
        }
        public async Task<HttpResponseMessage> RequestPut(string apiUrl, object sendObject, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {
            return await RequestInternal(apiUrl, RequestType.put, cancellationToken, httpRequestHeaders, sendObject);
        }
        public async Task<HttpResponseMessage> RequestGet(string apiUrl, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null)
        {
            return await RequestInternal(apiUrl, RequestType.get, cancellationToken, httpRequestHeaders);
        }
        private async Task<HttpResponseMessage> RequestInternal(string apiUrl, RequestType requestType, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null, object? sendObject = null)
        {
            if (string.IsNullOrEmpty(apiUrl) || string.IsNullOrWhiteSpace(apiUrl))
                throw new InvalidOperationException(API_URL_STRING_EMPTY_OR_WHITESPACE);

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

                StringContent apiBody = new StringContent(string.Empty);

                if (requestType == RequestType.post || requestType == RequestType.put)
                {
                    if (sendObject == null)
                        return apiResponse;

                    var sendObjectAsJsonString = _jsonConverter.ConvertObjectToJsonString(sendObject);

                    apiBody = new StringContent(sendObjectAsJsonString, Encoding.UTF8, MEDIA_TYPE);

                }

                try
                {
                    switch (requestType)
                    {
                        case RequestType.post:
                            apiResponse = await client.PostAsync(apiUrl, apiBody, cancellationToken ?? new CancellationTokenSource().Token);
                            break;
                        case RequestType.put:
                            apiResponse = await client.PutAsync(apiUrl, apiBody, cancellationToken ?? new CancellationTokenSource().Token);
                            break;
                        case RequestType.get:
                            apiResponse = await client.GetAsync(apiUrl, cancellationToken ?? new CancellationTokenSource().Token);
                            break;
                        case RequestType.delete:
                            break;
                        default:
                            break;
                    }
                }
                catch(Exception ex)
                {
                    throw new RestRequestException(string.Format(REST_REQUEST_PROBLEM_EXCEPTION_MESSAGE2, apiUrl), ex);
                }

                if (!apiResponse.IsSuccessStatusCode)
                {
                    throw new RestRequestException(string.Format(REST_REQUEST_FAILED_EXCEPTION_MESSAGE,
                        apiResponse.StatusCode,
                        apiResponse.Headers,
                        apiResponse.Content,
                        apiResponse.RequestMessage,
                        apiResponse.ReasonPhrase,
                        apiResponse.Version,
                        apiResponse.TrailingHeaders,
                        sendObject?.ToString()));
                }

                return apiResponse;
            }
        }
    }

}
