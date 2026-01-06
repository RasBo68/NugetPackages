
namespace Coeo.Extensions.RestApiClient
{
    public interface IRestApiClientService
    {
        Task<HttpResponseMessage> RequestGet(string apiUrl, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null);
        Task<HttpResponseMessage> RequestPost(string apiUrl, object sendObject, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null);
        Task<HttpResponseMessage> RequestPut(string apiUrl, object sendObject, CancellationToken? cancellationToken = null, List<HttpRequestHeaderSimplified>? httpRequestHeaders = null);
    }
}