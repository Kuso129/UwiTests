using System.Net.Http.Json;
using UwiTests.Model;

var client = new HttpClient();
Console.WriteLine("pleas enter url");
var url = Console.ReadLine();

var qurl = $"{url}/questions";

//Инициализация
{
    Console.WriteLine("question test");

    Login login = new Login();
    login.UserLogin = "Trump";
    login.Role = "User";
    login.Password = "1234";

    using var response = await client.PostAsJsonAsync($"{url}/accaunt", login);
    Console.WriteLine($"Accaunt registred: {response.Content.ReadAsStringAsync().Result}.");

    TestData testData = new TestData();
    testData.TestId = 0;
    testData.TestName = "If we need Maduro?";
    testData.CreatorId = 1;

    using var response2 = await client.PostAsJsonAsync($"{url}/tests", testData);
    Console.WriteLine($"TEST REGISTRED: {response2.Content.ReadAsStringAsync().Result}");
}

// POST
{
    Question q1 = new Question();
    q1.QuestionText = "UWA";
    q1.CorrectAnswerID = 0;
    q1.Answers = new List<string>
    {
        "uwa",
        "uwa a...",
        "ura"
    };
    q1.TestId = 1;

    Question q2 = new Question();
    q2.QuestionText = "MIWAMBA";
    q2.CorrectAnswerID = 2;
    q2.Answers = new List<string>
    {
        "humble",
        "miwa...",
        "uwa"
    };
    q2.TestId = 1;

    using var response1 = await client.PostAsJsonAsync(qurl, q1);
    using var response2 = await client.PostAsJsonAsync(qurl, q2);

    Console.WriteLine($"QUEST REGISTRED: {response1.Content.ReadAsStringAsync().Result}");
    Console.WriteLine($"QUEST REGISTRED: {response2.Content.ReadAsStringAsync().Result}");
}

// GET
{
    using var response = await client.GetAsync($"{qurl}?testId=1");
    Console.WriteLine($"GET: {response.Content.ReadAsStringAsync().Result}");
}

// DELETE
{
    var dqUrl = $"{qurl}?id=1";
    using var response = await client.DeleteAsync(dqUrl);
    Console.WriteLine($"DELETE: {response.Content.ReadAsStringAsync().Result}");
}

