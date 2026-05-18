using System.Net.Http.Json;
using UwiTests.Model;

var client = new HttpClient();
Console.WriteLine("pleas enter url");
var url = Console.ReadLine();

Console.WriteLine("enter user name");
var username = Console.ReadLine();

Console.WriteLine("enter password");
var password = Console.ReadLine();

Login login = new Login();
login.UserLogin = username;
login.Role = "User";
login.Password = password;

var response = await client.PostAsJsonAsync($"{url}/accaunt", login);
Console.WriteLine($"POST: {response.Content.ReadAsStringAsync().Result}");

// GET
response = await client.GetAsync($"{url}/accaunt");
Console.WriteLine($"GET: {response.Content.ReadAsStringAsync().Result}");

// DELETE
response = await client.DeleteAsync($"{$"{url}/accaunt"}?id=1");
Console.WriteLine($"DELETE: {response.Content.ReadAsStringAsync().Result}");








