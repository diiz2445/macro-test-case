using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;
//using Client.Client;

namespace Client.Client
{
    public class MyClient
    {
        private TcpClient _client;
        private const int Port = 8888; // Порт сервера
        private const string ServerIp = "127.0.0.1"; // IP-адрес сервера

        public MyClient()
        {
            _client = new TcpClient();
        }
        public static async Task Run()
        {
            
            Console.WriteLine("Запуск клиента....");
            try
            {
                using TcpClient tcpClient = new TcpClient(ServerIp, Port);
                using Connection connection = new Connection(tcpClient);
                Console.WriteLine($"Подключен к серверу: {Port}");
                while (true)
                {
                    string input = Console.ReadLine();
                    if (input.Length == 0)
                        break;
                    await connection.SendMessageAsync(input);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine("Нажмите любую клавишу для выхода...");
            Console.ReadKey(true);
        }
        

        public static string GetStringFromFile(string path)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                StreamReader sr = File.OpenText(path);
                string s = sr.ReadToEnd();
                sb.Insert(0, s);

            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            return sb.ToString();
        }
    }
}

