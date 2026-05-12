using UwiTests.Model;

namespace UwiTests
{
    public class AccauntManager
    {
        private List<UserData> _users;
        private int _lastUserID;

        public List<UserData> Users { get { return _users; } }

        public AccauntManager()
        {
            // TODO: save and load
            _lastUserID = 0;
            _users = new List<UserData>();
        }

        public UserData IsHaveUser(Login login)
        {
            foreach (var user in _users)
            {
                if (user.Login == login.UserLogin)
                    return user;
            }

            return null;
        }

        // TODO: Save and load
        private int GenerateUserId()
        {
            _lastUserID++;
            return _lastUserID;
        }

        public UserData AddNewLogin(Login login)
        {
            var usr = IsHaveUser(login);
            if (usr != null)
                return usr;

            usr = new UserData();
            usr.Password = login.Password;
            usr.UserId = GenerateUserId();
            usr.Role = login.Role;
            usr.Login = login.UserLogin;

            _users.Add(usr);

            Console.WriteLine($"Added new user: {usr.UserId}, {usr.Role}, {usr.Login}, {usr.Password}.");

            return usr;
        }

        public bool DeleteUser(int id)
        {
            foreach (var user in _users)
            {
                if (user.UserId == id)
                {
                    Users.Remove(user);
                    Console.WriteLine($"Deleted user id {id}.");
                    return true;
                }
            }

            return false;
        }
    }
}
