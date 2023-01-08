using System.Net.Http;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Core.web.Mvc.Utils
{
    public class HttpRequestAppService : IHttpRequestAppService
    {
        public async Task<Tuple<HttpStatusCode, string>> PostAsync(LimitedPool<HttpClient> httpClientPool, string payload, string url, string username, string password)
        {
            using (var httpClientContainer = httpClientPool.Get())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                var httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

                httpClientContainer.Value.DefaultRequestHeaders.Clear();

                httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)));

                var response = await httpClientContainer.Value.PostAsync(url, httpContent);

                var content = await response.Content.ReadAsStringAsync();

                return new Tuple<HttpStatusCode, string>(response.StatusCode, content);
            }
        }

        public async Task<Tuple<HttpStatusCode, string>> GetAsync(LimitedPool<HttpClient> httpClientPool, string thirdPartyTransactionId, string url, string username, string password)
        {
            using (var httpClientContainer = httpClientPool.Get())
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

                httpClientContainer.Value.DefaultRequestHeaders.Clear();

                httpClientContainer.Value.DefaultRequestHeaders.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(username + ":" + password)));

                var response = await httpClientContainer.Value.GetAsync($"{url}{thirdPartyTransactionId}");

                string content = await response.Content.ReadAsStringAsync();

                return new Tuple<HttpStatusCode, string>(response.StatusCode, content);
            }
        }
    }
}
