using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace UwiTests.Client;

public class TestTakingClient : ApiClientBase
{
    public TestTakingClient(HttpClient http) : base(http) { }
  
    public async Task<TestForTakingDto> GetTestForTakingAsync(int testId)  // Получение теста
    {
        var response = await _http.GetAsync($"api/tests/{testId}/take");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TestForTakingDto>();
    }

    public async Task<TestResultDto> SubmitAnswersAsync(int testId, SubmitAnswersDto answers)  // Отправка ответа
    {
        var response = await _http.PostAsJsonAsync($"api/tests/{testId}/submit", answers);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TestResultDto>();
    }

    public async Task<TestResultDto[]> GetMyResultsAsync()  //история результатов
    {
        var response = await _http.GetAsync("api/results/my");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<TestResultDto[]>() ?? [];
    }
}