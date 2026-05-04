using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using UwiTests.Client;

namespace UwiTests.Client
{
    public class TestApplication
    {
        private string _currentToken = null;
        private AuthClient _authClient;
        private TestCreatorClient _testCreator;
        private TestTakingClient _testTaking;
        private HttpClient _http;

        public event Action<string> OnMessage;
        public event Action<bool> OnAuthStateChanged;
        public event Action<List<UserTestDto>> OnMyTestsLoaded;
        public event Action<List<TestResultDto>> OnResultsLoaded;
        public event Action<int> OnTestCreated;

        public TestApplication()
        {
            InitializeHttpClient();
        }

        private void InitializeHttpClient()
        {
            _http = new HttpClient();
            _http.BaseAddress = new Uri("https://localhost:5000/");

            _authClient = new AuthClient(_http);
            _testCreator = new TestCreatorClient(_http);
            _testTaking = new TestTakingClient(_http);
        }

        public async Task<bool> LoginAsync(string email, string password) //для авторризации
        {
            try
            {
                string token = await _authClient.LoginAsunc(email, password);

                if (!string.IsNullOrEmpty(token))
                {
                    _currentToken = token;
                    SetAuthTokenToAllClients(token);

                    OnMessage?.Invoke("Вход выполнен успешно");
                    OnAuthStateChanged?.Invoke(true);
                    return true;
                }

                OnMessage?.Invoke("Ошибка входа. Проверьте email и\или пароль.");
                return false;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при входе: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> RegisterAsync(string userName, string email, string password)
        {
            try
            {
                string token = await _authClient.RegisterAsync(userName, email, password);

                if (!string.IsNullOrEmpty(token))
                {
                    _currentToken = token;
                    SetAuthTokenToAllClients(token);

                    OnMessage?.Invoke("Регистрация успешна");
                    OnAuthStateChanged?.Invoke(true);
                    return true;
                }

                OnMessage?.Invoke("Ошибка регистрации. Попробуйте другой email.");
                return false;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при регистрации: {ex.Message}");
                return false;
            }
        }

        public void Logout()
        {
            _currentToken = null;
            SetAuthTokenToAllClients(null);
            OnMessage?.Invoke("Вы вышли из аккаунта.");
            OnAuthStateChanged?.Invoke(false);
        }

        public bool IsAuthenticated()
        {
            return !string.IsNullOrEmpty(_currentToken);
        }

        private void SetAuthTokenToAllClients(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                _authClient.SetAuthToken("");
                _testCreator.SetAuthToken("");
                _testTaking.SetAuthToken("");
            }
            else
            {
                _authClient.SetAuthToken(token);
                _testCreator.SetAuthToken(token);
                _testTaking.SetAuthToken(token);
            }
        }

        //для рабоиы с течтами
        public async Task<List<UserTestDto>> GetMyCreatedTestsAsync()
        {
            try
            {
                if (!IsAuthenticated())
                {
                    OnMessage?.Invoke("Необходима авторизация");
                    return new List<UserTestDto>();
                }

                var tests = await _testCreator.GetMyTestsAsync();
                var testList = new List<UserTestDto>(tests);

                OnMyTestsLoaded?.Invoke(testList);
                return testList;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при загрузке теста: {ex.Message}");
                return new List<UserTestDto>();
            }
        }

        public async Task<TestResultDto[]> GetMyResultsAsync()
        {
            try
            {
                if (!IsAuthenticated())
                {
                    OnMessage?.Invoke("Необходима авторизация");
                    return Array.Empty<TestResultDto>();
                }

                var results = await _testTaking.GetMyResultsAsync();
                OnResultsLoaded?.Invoke(new List<TestResultDto>(results));
                return results;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при загрузке результата: {ex.Message}");
                return Array.Empty<TestResultDto>();
            }
        }

        public async Task<int> CreateNewTestAsync(string title, string description, int timeLimitMinutes, bool isPublic)
        {
            try
            {
                if (!IsAuthenticated())
                {
                    OnMessage?.Invoke("Только авторизованные пользователи могут создавать тест");
                    return 0;
                }

                var newTest = new CreateTestDto
                {
                    Title = title,
                    Description = description,
                    TimeLimitMinutes = timeLimitMinutes,
                    IsPublic = isPublic
                };

                int testId = await _testCreator.CreateTestAsync(newTest);
                OnTestCreated?.Invoke(testId);
                OnMessage?.Invoke($"Тест успешно создан ID: {testId}");
                return testId;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при создании теста: {ex.Message}");
                return 0;
            }
        }

        public async Task<bool> AddQuestionToTestAsync(int testId, string questionText, List<string> options, int correctOptionIndex, int points)
        {
            try
            {
                if (!IsAuthenticated())
                {
                    OnMessage?.Invoke("Необходима авторизация");
                    return false;
                }

                var question = new CreateQuestionDto
                {
                    QuestionText = questionText,
                    Options = options,
                    CorrectOptionIndex = correctOptionIndex,
                    Points = points
                };

                await _testCreator.AddQuestionAsync(testId, question);
                OnMessage?.Invoke("Вопрос успешно добавлен");
                return true;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при добавлении вопроса: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> PublishTestAsync(int testId)
        {
            try
            {
                if (!IsAuthenticated())
                {
                    OnMessage?.Invoke("Необходима авторизация");
                    return false;
                }

                await _testCreator.PublishTestAsync(testId);
                OnMessage?.Invoke("Тест опубликован и доступен для прохождения");
                return true;
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при публикации теста: {ex.Message}");
                return false;
            }
        }

        // Эти методы переопределать\расширить при добавлении UI

        public async Task<TestForTakingDto> GetTestForTakingAsync(int testId)
        {
            try
            {
                return await _testTaking.GetTestForTakingAsync(testId);
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при загрузке теста: {ex.Message}");
                return null;
            }
        }

        public async Task<TestResultDto> SubmitAnswersAsync(int testId, Dictionary<int, int> answers, int timeSpentSeconds)
        {
            try
            {
                var submitData = new SubmitAnswersDto
                {
                    TestId = testId,
                    Answers = answers,
                    TimeSpentSeconds = timeSpentSeconds
                };

                return await _testTaking.SubmitAnswersAsync(testId, submitData);
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка при отправке ответов: {ex.Message}");
                return null;
            }
        }

        public async Task<UserTestDto[]> GetAvailableTestsAsync()
        {
            try
            {
                // Метод нужно добавить в TestTakingClient если его нет
                // return await _testTaking.GetAvailableTestsAsync();
                OnMessage?.Invoke("Функция получения доступных тестов в разработке");
                return Array.Empty<UserTestDto>();
            }
            catch (Exception ex)
            {
                OnMessage?.Invoke($"Ошибка: {ex.Message}");
                return Array.Empty<UserTestDto>();
            }
        }
        public void Dispose()
        {
            _http?.Dispose();
        }
    }

    // упрощеннвц для консольного теста вариант
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var app = new TestApplication();

            // Подписываемся на события для вывода в консоль
            app.OnMessage += (msg) => Console.WriteLine($"[Сообщение] {msg}");
            app.OnAuthStateChanged += (isAuth) => Console.WriteLine($"Статус авторизации: {(isAuth ? "Вошли" : "Не авторизованы")}");
            app.OnMyTestsLoaded += (tests) => Console.WriteLine($"Загружено тестов: {tests.Count}");
            app.OnResultsLoaded += (results) => Console.WriteLine($"Загружено результатов: {results.Count}");

            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\n=== Система тестирования ===");

                if (!app.IsAuthenticated())
                {
                    Console.WriteLine("1 - Вход");
                    Console.WriteLine("2 - Регистрация");
                    Console.WriteLine("3 - Выход");
                    Console.Write("Выберите действие: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            Console.Write("Email: ");
                            string email = Console.ReadLine();
                            Console.Write("Пароль: ");
                            string pass = Console.ReadLine();
                            await app.LoginAsync(email, pass);
                            break;
                        case "2":
                            Console.Write("Имя: ");
                            string name = Console.ReadLine();
                            Console.Write("Email: ");
                            string regEmail = Console.ReadLine();
                            Console.Write("Пароль: ");
                            string regPass = Console.ReadLine();
                            await app.RegisterAsync(name, regEmail, regPass);
                            break;
                        case "3":
                            exit = true;
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("1 - Мои тесты");
                    Console.WriteLine("2 - Создать тест");
                    Console.WriteLine("3 - Мои результаты");
                    Console.WriteLine("4 - Выйти");
                    Console.Write("Выберите действие: ");

                    string choice = Console.ReadLine();

                    switch (choice)
                    {
                        case "1":
                            await app.GetMyCreatedTestsAsync();
                            break;
                        case "2":
                            Console.Write("Название теста: ");
                            string title = Console.ReadLine();
                            Console.Write("Описание: ");
                            string desc = Console.ReadLine();
                            Console.Write("Лимит времени (мин): ");
                            int timeLimit = int.Parse(Console.ReadLine() ?? "0");
                            Console.Write("Публичный? (true/false): ");
                            bool isPublic = bool.Parse(Console.ReadLine() ?? "true");

                            int testId = await app.CreateNewTestAsync(title, desc, timeLimit, isPublic);

                            if (testId > 0)
                            {
                                Console.Write("Добавить вопросы? (да/нет): ");
                                if (Console.ReadLine()?.ToLower() == "да")
                                {
                                    await AddQuestionsToTest(app, testId);
                                }

                                Console.Write("Опубликовать тест? (да/нет): ");
                                if (Console.ReadLine()?.ToLower() == "да")
                                {
                                    await app.PublishTestAsync(testId);
                                }
                            }
                            break;
                        case "3":
                            await app.GetMyResultsAsync();
                            break;
                        case "4":
                            app.Logout();
                            break;
                    }
                }
            }

            app.Dispose();
        }

        static async Task AddQuestionsToTest(TestApplication app, int testId)
        {
            bool addMore = true;
            int questionNum = 1;

            while (addMore)
            {
                Console.WriteLine($"\n--- Вопрос {questionNum} ---");
                Console.Write("Текст вопроса: ");
                string text = Console.ReadLine();

                Console.Write("Количество вариантов: ");
                int optionsCount = int.Parse(Console.ReadLine() ?? "2");

                var options = new List<string>();
                for (int i = 0; i < optionsCount; i++)
                {
                    Console.Write($"Вариант {i + 1}: ");
                    options.Add(Console.ReadLine());
                }

                Console.Write("Правильный ответ (номер): ");
                int correct = int.Parse(Console.ReadLine() ?? "1") - 1;

                Console.Write("Баллы: ");
                int points = int.Parse(Console.ReadLine() ?? "1");

                await app.AddQuestionToTestAsync(testId, text, options, correct, points);

                Console.Write("Добавить ещё? (да/нет): ");
                addMore = Console.ReadLine()?.ToLower() == "да";
                questionNum++;
            }
        }
    }
}