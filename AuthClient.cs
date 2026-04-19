using System.Net.Http.Json;

namespace UwiTests.Client;

public class AuthClient : ApiClientBase
{
	public AuthClient(HttpClient http) : base(http) { }

	//регистрация
	public async Task<string?> RegisterAsync(string userName, string email, string password)
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
	public async Task<string?> LoginAsunc(string email, string password)
	{
		var rasponnse = await _http.PostAsJsongAsync("api/auth/login", new
		{
			email,
			password
		});
		rasponnse.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsyn<TokenResponse>
			();
		return result?.Token;
	}

	internal class TokenResponse
	{
		public string Token { get; set; } = string.Empty;
	}
}
