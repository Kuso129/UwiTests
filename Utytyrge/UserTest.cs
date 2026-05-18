using System.Net.Http;
using System.Net.Http.Json;
using UwiTests.Model;

namespace Utytyrge
{
    internal class UserTest
    {
        private string _Url;
        public UserTest(string Url)
        {
            _Url = Url;
        }

        public async Task Run()
        {
            Console.WriteLine("Тестирутся функционал user-а");
            using HttpClient client = new HttpClient();
            Login[] logins = new Login[4];
            for (int i = 0; i < 4; i++)
            {
                Console.WriteLine("Введите ваше имя");
                var name =Console.ReadLine();

                Console.WriteLine("Введите ваш пароль");
                var password = Console.ReadLine();

                logins[i].Role = "User";
                logins[i].UserLogin = name;
                logins[i].Password = password;

                var response = await client.PostAsJsonAsync(_Url + "/accaunt", logins[i]);
                Console.WriteLine($"{i} {response}");
            }
        }
    }
}
