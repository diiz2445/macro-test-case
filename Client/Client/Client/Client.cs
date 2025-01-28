using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    public class Client
    {

        public static void Run()
        {
            string server = "127.0.0.1"; // IP-адрес сервера
            int port = 12345;           // Порт сервера

            try
            {
                // Создаем клиентский сокет
                using (TcpClient client = new TcpClient(server, port))
                {
                    Console.WriteLine("Подключено к серверу!");

                    // Получаем сетевой поток для чтения и записи
                    using (NetworkStream stream = client.GetStream())
                    {
                        // Отправляем сообщение серверу
                        string message = "Привет, сервер!";
                        byte[] data = Encoding.UTF8.GetBytes(message);
                        stream.Write(data, 0, data.Length);
                        Console.WriteLine($"Отправлено: {message}");

                        // Читаем ответ от сервера
                        byte[] buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        string response = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Ответ от сервера: {response}");
                    }
                }
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Ошибка подключения: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}
