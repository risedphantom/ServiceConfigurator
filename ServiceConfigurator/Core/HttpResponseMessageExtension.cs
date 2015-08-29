using System.Net;
using System.Net.Http;

namespace ServiceConfigurator.Core
{
    static class HttpResponseMessageExtension
    {
        static public HttpResponseMessage CreateHtmlResponse(this HttpRequestMessage request, HttpStatusCode statusCode, string value)
        {
            var response = request.CreateResponse(statusCode);
            response.Content = new StringContent(value, System.Text.Encoding.UTF8, "text/html");

            return response;
        }
    }
}
