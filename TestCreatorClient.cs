using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UwiTests.Client;

//создание тестов
public class TestCreatorClient : ApiClientBase
{
	public TestCreatorClient(HttpClient http) : base(http) { }
	
	public async Task<int> CreateTestAsync(CreateTestDto testData)    //новый тест
	{
		var response = await _http.PostAsJsonAsync("api/tests/create", testData);
		response.EnsureSuccessStatusCode();
		var result = await response.Content.ReadFromJsonAsync<TestCreateResponse>();
		return result?.TestId ?? 0;
	}
	
	public async Task AddQuestionAsync(int testId, CreateQuestionDto question)  // ƒобавление вопроса 
	{
		var response = await _http.PostAsJsonAsync($"api/tests/{testId}/questions", question);
		response.EnsureSuccessStatusCode();
	}

	public async Task PublishTestAsync(int testId)  // ѕубликаци€
	{
		var response = await _http.PostAsync($"api/tests/{testId}/publish", null);
		response.EnsureSuccessStatusCode();
	}

	public async Task<UserTestDto[]> GetMyTestsAsync()  // просмотр созданых тестов
	{
		var response = await _http.GetAsync("api/tests/my");
		response.EnsureSuccessStatusCode();
		return await response.Content.ReadFromJsonAsync<UserTestDto[]>() ?? [];
	}
}