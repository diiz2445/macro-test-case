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

            
        }
        public async void Listen()
        {
            while (true)
            {
                // Ожидаем подключения
                var client = listener.AcceptTcpClient();
                Console.WriteLine("Новое входящее соединение...");

                
                HandleClient(client);
                
                
            }
        }
        public void Start() { try { listener.Start(); Console.WriteLine($"Сервер запущен. Количество потоков обработки = {_connectionLimiter.CurrentCount}"); } catch { } }
        public void Stop() { try { listener.Stop(); Console.WriteLine("Сервер остановлен"); } catch { Console.WriteLine("ошибка в остановке сервера"); } }
        private static void HandleClient(object clientObj)
        {
            var client = (TcpClient)clientObj;
            var stream = client.GetStream();
            while(true)
            
            {
                //_connectionLimiter.Wait();
                try
                {
                    // Ожидаем подключения и работаем с клиентом

                    {
                        //_connectionLimiter.Wait(); // Блокируем слот

                        
                        byte[] buffer = Encoding.UTF8.GetBytes("Добро пожаловать на сервер!\n");
                        stream.Write(buffer, 0, buffer.Length);

                        // Пример простой обработки данных
                        buffer = new byte[1024];
                        int bytesRead = stream.Read(buffer, 0, buffer.Length);
                        Console.WriteLine($"Получено: {Encoding.UTF8.GetString(buffer, 0, bytesRead)}");
                            //_connectionLimiter.Release();
                            stream.Close();
                        }
                }
                catch (Exception ex)
                {
                    //Console.WriteLine($"Ошибка: {ex.Message}");
                }
                finally
                {
                    // Всегда освобождаем слот, независимо от успеха или ошибки
                    
                    //client.Close();
                    //Console.WriteLine("Соединение закрыто.");
                    Thread.Sleep(100);
                }
            }
        }

    }
}

