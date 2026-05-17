using Microsoft.AspNetCore.Mvc;
using UwiTests.Model;

namespace UwiTests.Controllers
{
    [ApiController]
    [Route("questions")]
    public class QuestionsController : ControllerBase
    {
        [HttpPost]
        public IActionResult AddTestQuestion([FromBody] Question question)
        {
            Console.WriteLine($"Questions setup {question.TestId}");
            return Ok(DataBase.Instance.AddTestQuestion(question));
        }

        [HttpGet]
        public IActionResult GetTestQuestions([FromQuery] int testId)
        {
            Console.WriteLine($"Getting test questions request {testId}.");
            return Ok(DataBase.Instance.GetTestQuestions(testId));
        }

        [HttpDelete]
        public IActionResult DeleteTestQuestions([FromQuery] int id)
        {
            Console.WriteLine($"Questions deleteion {id}");
            return Ok(DataBase.Instance.DeleteTestQuestions(id));
        }

    }
}
