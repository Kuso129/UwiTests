using UwiTests.Model;

namespace UwiTests
{
    public class TestsManager
    {
        public List<TestState> Tests { get; private set; }
        private int _lastTestID;

        public TestsManager()
        {
            // TODO: save and load
            Tests = new List<TestState>();
            _lastTestID = 0;
        }

        // TESTS

        public TestData IsHaveTest(TestData data)
        {
            foreach (var test in Tests)
            {
                if (test.Data.TestName == data.TestName && test.Data.CreatorId == data.CreatorId)
                {
                    return test.Data;
                }
            }

            return null;
        }

        public TestState IsHaveTest(int id)
        {
            foreach (var test in Tests) {
                if (test.Data.TestId == id)
                {
                    return test;
                }
            }

            return null;
        }

        private int GenerateTestID()
        {
            _lastTestID++;
            return _lastTestID;
        }

        public TestData? AddNewTest(TestData data)
        {
            var tst = IsHaveTest(data);
            if (tst != null)
                return tst;

            if (!DataBase.Instance.IsHaveUserID(data.CreatorId))
                return null;

            tst = new TestData();
            tst.TestName = data.TestName;
            tst.CreatorId = data.CreatorId;
            tst.TestId = GenerateTestID();

            TestState state = new TestState();
            state.Data = tst;
            Tests.Add(state);

            Console.WriteLine($"Added new test: {tst.TestName}, {tst.CreatorId}, {tst.TestId}");

            return tst;
        }

        public bool DeleteTest(int id)
        {
            foreach (var test in Tests)
            {
                if (test.Data.TestId == id)
                {
                    Console.WriteLine($"Deletion test: {test.Data.TestName}, {test.Data.TestId}, {test.Data.CreatorId}.");
                    Tests.Remove(test);
                    return true;
                }
            }

            return false;
        }

        // QUESTIONS
        public bool AddTestQuestion(Question question)
        {
            var test = IsHaveTest(question.TestId);
            if (test == null)
            {
                Console.WriteLine($"Thare is no test to add questions {question.TestId}.");
                return false;
            }

            test.Questions.Add(question);

            return true;
        }

        public Question[] GetTestQuestions(int id)
        {
            var test = IsHaveTest(id);
            if (test == null)
                return null;

            return test.Questions.ToArray();
        }

        public bool DeleteTestQuestions(int id)
        {
            var test = IsHaveTest(id);
            if (test == null)
                return false;

            test.Questions.Clear();

            return true;
        }

    }
}
