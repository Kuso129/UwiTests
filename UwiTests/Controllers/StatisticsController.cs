using Microsoft.AspNetCore.Mvc;
using UwiTests.Model;

namespace UwiTests.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatisticsController : ControllerBase
    {
        [HttpPost]
        public IActionResult SetStatistics([FromQuery] int userId, [FromBody] Statistics data)
        {
            Console.WriteLine($"Statistics setup {userId}.");
            return Ok(DataBase.Instance.SetStatistics(userId, data));
        }

        [HttpGet]
        public IActionResult GetStatistics([FromQuery]int userId)
        {
            Console.WriteLine($"Getting statistics request");
            return Ok(DataBase.Instance.GetStatistics(userId));
        }
    }
}
