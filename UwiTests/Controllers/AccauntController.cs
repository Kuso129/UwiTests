using UwiTests.Model;
using Microsoft.AspNetCore.Mvc;

namespace UwiTests.Controllers
{
    [ApiController]
    [Route("accaunt")]
    public class AccauntController : ControllerBase
    {

        [HttpPost]
        public IActionResult AddNewUser([FromBody] Login login)
        {
            var user = DataBase.Instance.AddNewLogin(login);
            return Ok(user);
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            Console.WriteLine("Getting users request.");
            return Ok(DataBase.Instance.Users);
        }

        [HttpDelete]
        public IActionResult DeleteUser([FromQuery] int id)
        {
            return Ok(DataBase.Instance.DeleteUser(id));
        }

    }
}
