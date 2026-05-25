using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UwiTests.Model;

namespace UwiTests.Services  // ← ВАЖНО: это пространство имён
{
    public class AuthService
    {
        private readonly IUserRepository _userRepository;
        private UserData _currentUser;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<bool> Login(string login, string password)
        {
            try
            {
                var user = await _userRepository.GetUserByLogin(login);
                if (user != null && user.Password == password)
                {
                    _currentUser = user;
                    return true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Login error: {ex.Message}");
            }
            return false;
        }

        public async Task<bool> Register(string login, string password)
        {
            try
            {
                var existingUser = await _userRepository.GetUserByLogin(login);
                if (existingUser != null)
                    return false;

                var newUser = new UserData
                {
                    Login = login,
                    Password = password,
                    Role = "User"
                };

                var created = await _userRepository.CreateUser(newUser);
                return created != null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Register error: {ex.Message}");
            }
            return false;
        }

        public UserData GetCurrentUser()
        {
            return _currentUser;
        }

        public void Logout()
        {
            _currentUser = null;
        }

        public async Task<bool> IsAuthenticated()
        {
            return _currentUser != null;
        }

        public async Task<List<TestData>> GetAllTests()
        {
            try
            {
                return await _userRepository.GetAllTests();
            }
            catch
            {
                return new List<TestData>();
            }
        }

        public async Task<TestData> CreateTest(TestData testData)
        {
            return await _userRepository.CreateTest(testData);
        }

        public async Task<bool> AddQuestion(Question question)
        {
            return await _userRepository.AddQuestion(question);
        }

        public async Task<Statistics> GetUserStatistics(int userId)
        {
            return await _userRepository.GetStatistics(userId);
        }
    }
}