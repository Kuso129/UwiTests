using UwiTests.Model;

namespace UwiTests
{
    public class DataBase
    {
        public static DataBase Instance { get; private set; } = null;

        public AccauntManager AccauntManager { get; private set; }
        public TestsManager TestsManager { get; private set; }

        public DataBase()
        {
            if (Instance == null)
                Instance = this;

            AccauntManager = new AccauntManager();
            TestsManager = new TestsManager();
        }

        // ACCAUNT
        public UserData IsHaveUser(Login login)
        {
            return AccauntManager.IsHaveUser(login);
        }

        public bool IsHaveUserID(int id)
        {
            foreach (var user in AccauntManager.Users)
            {
                if (user.UserId == id) return true;
            }

            return false;
        }

        public UserData AddNewLogin(Login login)
        {
            return AccauntManager.AddNewLogin(login);
        }

        public List<UserData> Users { get => AccauntManager.Users; }

        public bool DeleteUser(int id) => AccauntManager.DeleteUser(id);

        // TESTS
        public TestData? AddNewTest(TestData data) => TestsManager.AddNewTest(data);

        public List<TestData> GetAllTests()
        {
            List<TestData> tests = new List<TestData>();

            foreach (var i in TestsManager.Tests)
                tests.Add(i.Data);

            return tests;
        }

        public bool DeleteTest(int id) => TestsManager.DeleteTest(id);
    }
}
