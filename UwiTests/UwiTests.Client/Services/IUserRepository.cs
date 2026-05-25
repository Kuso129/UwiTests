using System.Collections.Generic;
using System.Threading.Tasks;
using UwiTests.Model;

namespace UwiTests.Services
{
    public interface IUserRepository
    {
        /// <summary>
        /// Получить пользователя по логину
        /// </summary>
        Task<UserData> GetUserByLogin(string login);

        /// <summary>
        /// Создать нового пользователя
        /// </summary>
        Task<UserData> CreateUser(UserData user);

        /// <summary>
        /// Получить все тесты
        /// </summary>
        Task<List<TestData>> GetAllTests();

        /// <summary>
        /// Создать новый тест
        /// </summary>
        Task<TestData> CreateTest(TestData testData);

        /// <summary>
        /// Добавить вопрос к тесту
        /// </summary>
        Task<bool> AddQuestion(Question question);

        /// <summary>
        /// Получить статистику пользователя
        /// </summary>
        Task<Statistics> GetStatistics(int userId);

        /// <summary>
        /// Удалить тест (опционально)
        /// </summary>
        Task<bool> DeleteTest(int testId);

        /// <summary>
        /// Получить вопросы теста
        /// </summary>
        Task<List<Question>> GetTestQuestions(int testId);
    }
}