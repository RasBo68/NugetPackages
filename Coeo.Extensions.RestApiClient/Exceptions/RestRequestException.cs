namespace Coeo.Extensions.RestApiClient.Exceptions
{
    public class RestRequestException:Exception
    {
        public RestRequestException(string message)
            : base(message)
        {
        }
        public RestRequestException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
