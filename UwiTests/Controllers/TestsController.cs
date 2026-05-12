using Microsoft.AspNetCore.Mvc;
using UwiTests.Model;

namespace UwiTests.Controllers
{
    [ApiController]
    [Route("tests")]
    public class TestsController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddTest([FromBody] TestData data)
        {
            var test = DataBase.Instance.AddNewTest(data);
            return Ok(test);
        }

        [HttpGet]
        public IActionResult GetTests()
        {
            Console.WriteLine("Getting tests request.");
            return Ok(DataBase.Instance.GetAllTests());
        }

        [HttpDelete]
        public IActionResult DeleteTest(int id)
        {
            return Ok(DataBase.Instance.DeleteTest(id));
        }
    }
}
