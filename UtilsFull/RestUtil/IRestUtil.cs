using System.Threading.Tasks;

namespace Utils.RestUtil
{
    public interface IRestUtil
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
