using Microsoft.AspNetCore.Mvc;
using UwiTests.Models;

namespace UwiTests.Controllers
{
    [ApiController]
    [Route("reg")]
    public class UserController : ControllerBase
    {
        public static List<User> Users { get; set; } = new List<User>();
        private static int _lastID = 0;

        [HttpPost]
        public IActionResult AddNewUser([FromBody] UserEnter userData)
        {
            var user = ImplAddNewUser(userData);

            if (user != null)
            {
                Console.WriteLine($"Added new user: Id: {user.Id}, Login: {userData.Login}, Emain: {userData.Email}, Password: {userData.Password}, Role: {userData.Role}.");
                return Ok(user.Id);
            }

            return Ok("Failure");
        }


        private User ImplAddNewUser(UserEnter userData)
        {
            User user = new User();

            _lastID++;
            user.Id = _lastID.ToString();
            user.Email = userData.Email;
            user.Login = userData.Login;
            user.Password = userData.Password;
            user.Role = userData.Role;

            if (user.Role != "User" || user.Role != "Developer")
                user.Role = "User";

            Users.Add(user);
            return Users[Users.Count - 1];
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }

        private User GetUserFromId(string id)
        {
            for (int i = 0; i < Users.Count; i++)
            {
                if (Users[i].Id == id)
                    return Users[i];
            }

            return null;
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery]string id)
        {
            var user = GetUserFromId(id);

            if (user != null)
            {
                Users.Remove(user);
                Console.WriteLine($"Removed user from base: Id: {user.Id}, Login: {user.Login}, Email: {user.Email}, Password: {user.Password}, Role: {user.Id}.");
                return Ok("Success");
            }

            return Ok("Failure");
        }
    }
}
