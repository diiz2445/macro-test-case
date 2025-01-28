using Server.Server;

internal class Program
{
    private static void Main(string[] args)
    {
        string[] tests = {
            "aba",
            "abba",
            "dre",
            "abdba" };
        foreach (string test in tests)
        {
            Console.WriteLine($"string: {test}\nresult:{ServerLogic.isItPalindrom(test)}");
        }

        Console.ReadLine();
    }
}