using UwiTests.Model;

namespace UwiTests
{
    public class DataBase
    {
        public static DataBase Instance { get; private set; } = null;

        public AccauntManager AccauntManager { get; private set; }

        public DataBase()
        {
            if (Instance == null)
                Instance = this;

            AccauntManager = new AccauntManager();
        }

        public UserData IsHaveUser(Login login)
        {
            return AccauntManager.IsHaveUser(login);
        }

        public UserData AddNewLogin(Login login)
        {
            return AccauntManager.AddNewLogin(login);
        }

        public List<UserData> Users { get => AccauntManager.Users; }

        public bool DeleteUser(int id) => AccauntManager.DeleteUser(id);

    }
}
