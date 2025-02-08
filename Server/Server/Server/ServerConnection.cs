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
        private int _MaxConnections = 10;
        TcpListener listener;
        
        public ServerConnection(string host, int port, int MaxConnections)
        {
            listener = new TcpListener(IPAddress.Parse(host), port);
            _MaxConnections = MaxConnections;

            _connectionLimiter = new SemaphoreSlim(MaxConnections, MaxConnections);
            
        }
        public async Task Listen()
        {
            while (true)
            {
                // Ожидаем подключения
                var client = listener.AcceptTcpClient();
                Console.WriteLine("Новое входящее соединение...");

                
                HandleClient(client);
                
                
            }
        }
        public void Start()
        {
            listener.Start();
            Console.WriteLine("Сервер запущен. Ожидание подключений...");

            //прослушивание на подключение клиента
            while (true)
            {
                var client = listener.AcceptTcpClient();
                Task.Run(() => HandleClient(client));
            }
        }
        public void Stop() { try { listener.Stop(); Console.WriteLine("Сервер остановлен"); } catch { Console.WriteLine("ошибка в остановке сервера"); } }
        private void HandleClient(TcpClient client)
        {
            Console.WriteLine("Клиент подключен.");
            using (var stream = client.GetStream())
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                try
                {
                    while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                    {
                        //получение с клиента строки на сравнение
                        string receivedData = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                        Console.WriteLine($"Получено: {receivedData}");
                        if (_connectionLimiter.CurrentCount != 0)
                        {
                            //отправка результата
                            byte[] responseData = Encoding.UTF8.GetBytes(isItPalindrom(receivedData));
                            stream.Write(responseData, 0, responseData.Length);
                        }
                        else
                        {
                            stream.Write(Encoding.UTF8.GetBytes("ошибка: сервер перегружен"));
                        }
                      
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка: {ex.Message}");
                }
                finally
                {
                    client.Close();
                    Console.WriteLine("Клиент отключен.");
                }
            }
        }
        public static string isItPalindrom(string input)
        {
            //while (_connectionLimiter.CurrentCount == 0) { }//ожидание освобождения потока обработки
            Thread.Sleep(4000);
            _connectionLimiter.Wait();//блокируем поток
            for (int i = 0; i < input.Length / 2; i++)
            {
                if (input[i] != input[input.Length - i - 1])
                {
                    _connectionLimiter.Release();//освобождаем поток
                    return "false";
                }
            }
            _connectionLimiter.Release();//освобождаем поток
            return "true";

        }
    }
}

