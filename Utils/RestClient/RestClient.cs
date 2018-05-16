using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Utils.RestClient
{
    [ExcludeFromCodeCoverage]
    public class RestClient : IRestClient
    {
        private const string MimeType = "application/json";

        private readonly List<HttpStatusCode> _statusCodeError = new List<HttpStatusCode>
        {
            HttpStatusCode.InternalServerError,
            HttpStatusCode.NotImplemented,
            HttpStatusCode.BadGateway,
            HttpStatusCode.ServiceUnavailable,
            HttpStatusCode.GatewayTimeout,
            HttpStatusCode.HttpVersionNotSupported
        };

        private static HttpClient CreateClient(string baseAddress)
        {
            var handler = new HttpClientHandler { UseDefaultCredentials = true };
            var client = new HttpClient(handler);

            var url = new UriBuilder(HttpUtility.HtmlEncode(baseAddress) ?? throw new ArgumentNullException(nameof(baseAddress)));
            client.BaseAddress = url.Uri;

            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MimeType));

            return client;
        }

        /// <summary>
        /// Perform a GET operation.
        /// </summary>
        /// <typeparam name="T">Type of return, byte[] or string</typeparam>
        /// <param name="serviceKey">web.config key</param>
        /// <param name="route">Route of the WebApi</param>
        /// <returns></returns>
        public async Task<RestResult<T>> Get<T>(string serviceKey, string route)
        {
            using (var client = CreateClient(serviceKey))
            {
                return await HttpActionAsync<T>(client.GetAsync(route));
            }
        }

        /// <summary>
        /// Perform a PUT operation.
        /// </summary>
        /// <typeparam name="T">Type of return, byte[] or string</typeparam>
        /// <param name="serviceKey">web.config key</param>
        /// <param name="route">Route of the WebApi</param>
        /// <param name="body">Content of the body</param>
        /// <returns></returns>
        public async Task<RestResult<T>> Put<T>(string serviceKey, string route, string body)
        {
            using (var client = CreateClient(serviceKey))
            {
                return await HttpActionAsync<T>(client.PutAsync(route,
                                                new StringContent(body, Encoding.UTF8, MimeType)));
            }
        }

        /// <summary>
        /// Perform a POST operation.
        /// </summary>
        /// <typeparam name="T">Type of return, byte[] or string</typeparam>
        /// <param name="serviceKey">web.config key</param>
        /// <param name="route">Route of the WebApi</param>
        /// <param name="body">Content of the body</param>
        /// <returns></returns>
        public async Task<RestResult<T>> Post<T>(string serviceKey, string route, string body)
        {
            using (var client = CreateClient(serviceKey))
            {
                return await HttpActionAsync<T>(client.PostAsync(route,
                                                new StringContent(body, Encoding.UTF8, MimeType)));
            }
        }

        /// <summary>
        /// HttpAction that needs to be called and return the Result
        /// </summary>
        /// <param name="httpAction"></param>
        /// <returns><see cref="RestResult{T}"/> contaning the Http status code ans the json obtained from the call</returns>
        private async Task<RestResult<T>> HttpActionAsync<T>(Task<HttpResponseMessage> httpAction)
        {
            var response = await httpAction;

            var httpContent = response.Content;
            object result;

            if (typeof(T) == typeof(byte[]))
            {
                result = httpContent?.ReadAsByteArrayAsync().Result;
            }
            else
            {
                result = httpContent?.ReadAsStringAsync().Result;
            }

            var restContent = (T)Convert.ChangeType(result, typeof(T));

            LogIfError(response,
                       restContent);

            return new RestResult<T>(response.StatusCode, restContent);
        }

        /// <summary>
        /// Log the error if a HTTPError 500 or + occured
        /// </summary>
        /// <param name="response"></param>
        /// <param name="data"></param>
        private void LogIfError<T>(HttpResponseMessage response, T data)
        {
            try
            {
                if (_statusCodeError.Contains(response.StatusCode))
                {
                    Logging.Log($"{response.RequestMessage?.RequestUri?.OriginalString} => {data}");
                }
            }
            catch (Exception e)
            {
                Logging.Log(e.ToString());
            }
        }
    }
}