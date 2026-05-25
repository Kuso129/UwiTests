using System.Net.Http;

namespace UwiTests.Client
{
   public abstract class ApiClient
    {
        protected readonly HttpClient _http;
        protected ApiClient(HttpClient http)
        {
            _http = http;
        }
        public void SetAuthToken (string token)
        {
            _http.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue ("Bearer", token);
        }
    }
}
   
