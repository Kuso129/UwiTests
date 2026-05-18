
using Utytyrge;

static class Programm
{   
    private static void RunUserTest(string Url)    
    {
        UserTest test =new UserTest(Url); 
        test.Run();
    }

    public static void Main(string[] args)
    {
        Console.WriteLine("Please enter URL:");
        string url = Console.ReadLine();

        Console.WriteLine("Please enter test case: 1 2 3 4");
        string method = Console.ReadLine();

        if (method == "1") RunUserTest(url);
        while (true) ;
    }













}