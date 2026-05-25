using System.Net.Http.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace UwiTests.Client;

public class AuthClient : ApiClientBase
{
    public AuthClient(HttpClient http) : base(http) { }

    //регистрация
    public async Task<string> RegisterAsync(string userName, string email, string password)
    {
        var response = await _http.PostAsJsonAsync("api/auth/register", new
        {
            userName,
            email,
            password
        });
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenResponse>
            ();
        return result?.Token;
    }

    //вход
    public async Task<string> LoginAsunc(string email, string password)
    {
        var rasponse = await _http.PostAsJsonAsync("api/auth/login", new { email, password });
        rasponse.EnsureSuccessStatusCode();
        var result = await rasponse.Content.ReadFromJsonAsync<TokenResponse>
            ();
        return result?.Token;
    }

    internal class TokenResponse
    {
        public string Token { get; set; } = string.Empty;
    }
}
