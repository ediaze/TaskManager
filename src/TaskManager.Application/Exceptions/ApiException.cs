using System.Net;

namespace TaskManager.Application.Exceptions
{
    public class ApiException: Exception
    {
        public ApiException(HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(statusCode.ToString())
        {
            StatusCode = statusCode;
        }

        public ApiException(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest) : base(message)
        {
            StatusCode = statusCode;
        }

        public HttpStatusCode StatusCode { get; }
    }
}
