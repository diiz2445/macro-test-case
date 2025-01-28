using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class ServerConnection
    {
        private static SemaphoreSlim _connectionLimiter;
        private const int MaxConnections = 10;
        TcpListener listener;
        
        public ServerConnection(string host, int port, int MaxConnections)
        {
            listener = new TcpListener(IPAddress.Any, port);

            _connectionLimiter = new SemaphoreSlim(MaxConnections, MaxConnections);

            listener.Start();
            Console.WriteLine($"Сервер запущен на порту {port}. Максимум подключений: {MaxConnections}");

        }
        public void Listen()
        {
            while (true)
            {
                // Ожидаем подключения
                var client = listener.AcceptTcpClient();
                Console.WriteLine("Новое входящее соединение...");

                // Ограничиваем количество подключений
                if (!_connectionLimiter.Wait(0))
                {
                    Console.WriteLine("Максимальное количество подключений достигнуто. Соединение отклонено.");
                    client.Close();
                    continue;
                }

                // Обрабатываем подключение в отдельном потоке
                ThreadPool.QueueUserWorkItem(HandleClient, client);
            }
        }
        private static void HandleClient(object clientObj)
        {
            var client = (TcpClient)clientObj;

            try
            {
                var stream = client.GetStream();
                byte[] buffer = Encoding.UTF8.GetBytes("Добро пожаловать на сервер!\n");
                stream.Write(buffer, 0, buffer.Length);

                // Пример простой обработки данных
                buffer = new byte[1024];
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                Console.WriteLine($"Получено: {Encoding.UTF8.GetString(buffer, 0, bytesRead)}");

                stream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
            finally
            {
                client.Close();
                _connectionLimiter.Release(); // Освобождаем слот
                Console.WriteLine("Соединение закрыто.");
            }
        }


    }
}

