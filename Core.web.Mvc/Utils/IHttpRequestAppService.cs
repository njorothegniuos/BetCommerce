using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;

namespace Core.web.Mvc.Utils
{
    public interface IHttpRequestAppService
    {
        Task<Tuple<HttpStatusCode, string>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string username, string password);

        Task<Tuple<HttpStatusCode, string>> GetAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string username, string password);
    }
}
