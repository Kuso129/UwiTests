using System.Net.Http.Json;
using UwiTests.Model;

var client = new HttpClient();
Console.WriteLine("pleas enter url");
var url = Console.ReadLine();

var surl = $"{url}/statistics";

// INITIALIZE
{
    Login login = new Login();
    login.UserLogin = "OK";
    login.Role = "User";
    login.Password = "1234";

    using var response = await client.PostAsJsonAsync($"{url}/accaunt", login);
    Console.WriteLine($"Accaunt registred: {response.Content.ReadAsStringAsync().Result}.");
}

// POST
{
    Statistics statistics = new Statistics();
    statistics.TestAmount = 2;
    statistics.AvgResult = "Failure";
    statistics.AvgCompletionTime = DateTime.Now;
    statistics.TestId = new int[]{
        1, 2, 3
    };
    statistics.UserID = 1;

    using var response = await client.PostAsJsonAsync(surl, statistics);
    Console.WriteLine($"POST: {response.Content.ReadAsStringAsync().Result}");
}

// GET
{
    var stUrl = $"{surl}?userId=1";
    using var response = await client.GetAsync(stUrl);
    Console.WriteLine($"GET: {response.Content.ReadAsStringAsync().Result}");
}





