
namespace UwiTests.Model 
{
    public class TestTake
    {
        public int UserId { get; set; }
        public int TestId { get; set; }
        public string Result { get; set; }
        public DateTime Complete { get; set; }
    }

    public class TestData
    {
        public int TestId { get; set; }
        public int CreatorId { get; set; }
        public string TestName { get; set; }
    }

    public class Question
    {
        public string QuestionText { get; set; }
        public string [] Answers { get; set; }
        public int QuestionsAmount { get; set; }
        public int CorrectAnswerID { get; set; }
    }

    public class QuestionRequest
    {
        public int TestId { get; private set; }
        public Question Question { get; private set; }
    }
}