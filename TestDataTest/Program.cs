using System.Net.Http.Json;
using UwiTests.Model;

var client = new HttpClient();
Console.WriteLine("pleas enter url");
var url = Console.ReadLine();

var testsUrl = $"{url}/tests";

//Я ебал этот C#
{
    Login login = new Login();
    login.UserLogin = "Adolf";
    login.Role = "User";
    login.Password = "1234";

    var response = await client.PostAsJsonAsync($"{url}/accaunt", login);
    Console.WriteLine($"Accaunt registred: {response.Content.ReadAsStringAsync().Result}.");
}
// POST
{
    Console.WriteLine("enter test name");
    var testname = Console.ReadLine();
    TestData testData = new TestData();
    testData.TestId = 0;
    testData.TestName = testname;
    testData.CreatorId = 1;

    var response = await client.PostAsJsonAsync(testsUrl, testData);
    Console.WriteLine($"POST: {response.Content.ReadAsStringAsync().Result}");
}

// GET
{
    var response = await client.GetAsync(testsUrl);
    Console.WriteLine($"GET: {response.Content.ReadAsStringAsync().Result}");
}

// DELETE
{
    var response = await client.DeleteAsync($"{testsUrl}?id=1");
    Console.WriteLine($"DELETE: {response.Content.ReadAsStringAsync().Result}");
}







