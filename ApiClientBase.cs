using System.Net.Http;
using System.Net.Http.Headers;

namespace UwiTests.Client;

public abstract class ApiClientBase
{
    protected readonly HttpClient _http;

    protected ApiClientBase(HttpClient http)
    {
        _http = http;
    }

    public void SetAuthToken(string token)
    {
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);
    }
}