using Microsoft.AspNetCore.Mvc;
using UwiTests.Model;

namespace UwiTests.Controllers
{
    [ApiController]
    [Route("statistics")]
    public class StatisticsController : ControllerBase
    {
        [HttpPost]
        public IActionResult SetStatistics([FromBody] Statistics data)
        {
            Console.WriteLine($"Statistics setup {data.UserID}.");
            return Ok(DataBase.Instance.SetStatistics(data));
        }

        [HttpGet]
        public IActionResult GetStatistics([FromQuery]int userId)
        {
            Console.WriteLine($"Getting statistics request");
            return Ok(DataBase.Instance.GetStatistics(userId));
        }
    }
}
