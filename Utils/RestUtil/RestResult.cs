using System.Net;

namespace Utils.RestUtil
{
    public class RestResult<T>
    {
        public HttpStatusCode StatusCode { get; }
        public T Content { get; }

        public RestResult(HttpStatusCode statusCode,
                          T content)
        {
            StatusCode = statusCode;
            Content = content;
        }
    }
}