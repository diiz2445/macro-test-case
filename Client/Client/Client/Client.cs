//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Sockets;
//using System.Text;
//using System.Threading.Tasks;

//namespace Client
//{
//    public class Client
//    {

//        public static void Run()
//        {
//            string server = "127.0.0.1"; // IP-адрес сервера
//            int port = 8888;           // Порт сервера
//            while(true)
//            {
//                try
//                {
//                    // Создаем клиентский сокет
//                    using (TcpClient client = new TcpClient(server, port))
//                    {
//                        Console.WriteLine("Подключено к серверу!");

//                        // Получаем сетевой поток для чтения и записи
//                        using (NetworkStream stream = client.GetStream())
//                        {
//                            // Отправляем сообщение серверу
//                            string message = "Привет, сервер!";
//                            byte[] data = Encoding.UTF8.GetBytes(message);
//                            stream.Write(data, 0, data.Length);
//                            Console.WriteLine($"Отправлено: {message}");

//                            // Читаем ответ от сервера
//                            byte[] buffer = new byte[1024];
//                            int bytesRead = stream.Read(buffer, 0, buffer.Length);
//                            string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
//                            Console.WriteLine($"Ответ от сервера: {response}");
//                        }
                        
//                    }
//                }
//                catch (SocketException ex)
//                {
//                    Console.WriteLine($"Ошибка подключения: {ex.Message}");
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine($"Ошибка: {ex.Message}");
//                }
//                Console.ReadLine();
//            }
//        }
//    }
//}

using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class Client
{
    private TcpClient _client;
    private const int Port = 8888; // Порт сервера
    private const string ServerIp = "127.0.0.1"; // IP-адрес сервера

    public Client()
    {
        _client = new TcpClient();
    }

    public async Task StartAsync()
    {
        try
        {
            await _client.ConnectAsync(ServerIp, Port);
            Console.WriteLine("Подключение к серверу установлено.");

            using var stream = _client.GetStream();
            string message;

            Console.WriteLine("Введите сообщение для отправки (или 'exit' для выхода):");
            while ((message = Console.ReadLine()) != "exit")
            {
                // Отправка сообщения на сервер
                byte[] data = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(data, 0, data.Length);
                Console.WriteLine($"Отправлено: {message}");

                // Получение ответа от сервера
                byte[] buffer = new byte[1024];
                int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                Console.WriteLine($"Ответ от сервера: {response}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
        finally
        {
            _client.Close();
            Console.WriteLine("Соединение закрыто.");
        }
    }

    
}

