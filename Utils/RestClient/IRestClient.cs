using System.Threading.Tasks;

namespace Utils.RestClient
{
    public interface IRestClient
    {
        Task<RestResult<T>> Get<T>(string serviceKey,
                                   string route);

        Task<RestResult<T>> Put<T>(string serviceKey,
                                   string route,
                                   string body);

        Task<RestResult<T>> Post<T>(string serviceKey,
                                    string route,
                                    string body);
    }
}
