using System.DateTime;

namespace UwiTests.Model
{
    public class Statistics 
    {
        public int TestAmount { get; set; }
        public DateTime AvgCompletionTime { get; set; }
        public string AvgResult { get; set; }
        public int [] TestId { get; set; }
    }
}