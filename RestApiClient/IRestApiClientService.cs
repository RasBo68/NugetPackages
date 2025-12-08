using RGuide.Fundamental.WebApiInteraction.Models;

namespace RestApiClient
{
    public interface IRestApiClientService
    {
        Task<HttpResponseMessage> RequestGet(string apiUrl, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null);
        Task<HttpResponseMessage> RequestPost(string apiUrl, object sendObject, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null);
    }
}