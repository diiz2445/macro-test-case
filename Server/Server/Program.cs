using Server.Server;

internal class Program
{
    private static void Main(string[] args)
    {
        ServerConnection server = new ServerConnection("127.0.0.1", 8888,1);
        server.Listen();
        
        
        Console.ReadLine();
    }
}