using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;
using UwiTests.Model;
using UwiTests.Client;

namespace UwiTests.Services
{
    public class UserRepository : ApiClient, IUserRepository
    {
        public UserRepository(HttpClient http) : base(http)
        {
        }

        public async Task<UserData> GetUserByLogin(string login)
        {
            try
            {
                var response = await _http.GetAsync("/accaunt");
                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<List<UserData>>();
                    return users?.Find(u => u.Login == login);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetUserByLogin error: {ex.Message}");
            }
            return null;
        }

        public async Task<UserData> CreateUser(UserData user)
        {
            try
            {
                var loginObj = new Login
                {
                    UserLogin = user.Login,
                    Password = user.Password,
                    Role = user.Role ?? "User"
                };

                var response = await _http.PostAsJsonAsync("/accaunt", loginObj);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<UserData>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateUser error: {ex.Message}");
            }
            return null;
        }

        public async Task<List<TestData>> GetAllTests()
        {
            try
            {
                var response = await _http.GetAsync("/tests");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<TestData>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetAllTests error: {ex.Message}");
            }
            return new List<TestData>();
        }

        public async Task<TestData> CreateTest(TestData testData)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/tests", testData);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<TestData>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"CreateTest error: {ex.Message}");
            }
            return null;
        }

        public async Task<bool> AddQuestion(Question question)
        {
            try
            {
                var response = await _http.PostAsJsonAsync("/questions", question);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Statistics> GetStatistics(int userId)
        {
            try
            {
                var response = await _http.GetAsync($"/statistics?userId={userId}");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<Statistics>();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetStatistics error: {ex.Message}");
            }
            return null;
        }
        public async Task<bool> DeleteTest(int testId)
        {
            try
            {
                var response = await _http.DeleteAsync($"/tests?id={testId}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DeleteTest error: {ex.Message}");
                return false;
            }
        }

        public async Task<List<Question>> GetTestQuestions(int testId)
        {
            try
            {
                var response = await _http.GetAsync($"/questions?testId={testId}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<Question>>(json, new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"GetTestQuestions error: {ex.Message}");
            }
            return new List<Question>();
        }
    }
}