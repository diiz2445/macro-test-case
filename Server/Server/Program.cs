using Server.Server;

//ServerConnection server = new ServerConnection("127.0.0.1", 8888, 1);
//try
//{
//    server.Start();    
//}
//finally
//{
//    server.Stop();
//}

internal class Program
{
    
        public static async Task Main(string[] args)
        {
            int port = 8888;
            Console.WriteLine("Запуск сервера....");
            using (ServerConnection server = new ServerConnection("127.0.0.1", 8888, 1))
            {
                Task servertask = server.ListenAsync();
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input == "stop")
                    {
                        Console.WriteLine("Остановка сервера...");
                        server.Stop();
                        break;
                    }
                }
                await servertask;
            }
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey(true);
        }
    }
