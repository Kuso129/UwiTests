using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Json;

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
   
