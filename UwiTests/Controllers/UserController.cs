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
                Console.WriteLine($"Added new user: Id: {user.Id}, Login: {userData.Login}, Emain: {userData.Email}, Password: {userData.Password}.");
                return Ok(user.Id);
            }

            return Ok(-1);
        }

        private User ImplAddNewUser(UserEnter userData)
        {
            User user = new User();

            _lastID++;
            user.Id = _lastID;
            user.Email = userData.Email;
            user.Login = userData.Login;
            user.Password = userData.Password;

            Users.Add(user);
            return Users[Users.Count - 1];
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            return Ok(Users);
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery]int index)
        {
            var user = Users[index];
            Users.Remove(user);
            Console.WriteLine($"Removed user from base: Id: {user.Id}, Login: {user.Login}, Email: {user.Email}, Password: {user.Password}.");
            return Ok();
        }
    }
}
