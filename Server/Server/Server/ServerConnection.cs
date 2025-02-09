using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Server.Server
{
    internal class ServerConnection:IDisposable
    {
        private static SemaphoreSlim _connectionLimiter;
        private int _MaxConnections = 10;
        
        private readonly TcpListener _listener;
        private readonly List<Connection> _clients; // это пул подключений, нужен чтобы нормально отключить всех подключенных при остановке сервера
        bool disposed;

        public ServerConnection(string host, int port, int MaxConnections)
        {
           
            _MaxConnections = MaxConnections;

            _connectionLimiter = new SemaphoreSlim(MaxConnections, MaxConnections);

            _listener = new TcpListener(IPAddress.Any, port);
            _clients = new List<Connection>();

        }
        public async Task ListenAsync()
        {
            try
            {
                _listener.Start();
                Console.WriteLine("Сервер стартовал на " + _listener.LocalEndpoint);
                while (true)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    Console.WriteLine("Подключение: " + client.Client.RemoteEndPoint + " > " + client.Client.LocalEndPoint);
                    lock (_clients)
                    {
                        _clients.Add(new Connection(client, c => { lock (_clients) { _clients.Remove(c); } c.Dispose(); }));
                    }
                }
            }
            catch (SocketException)
            {
                Console.WriteLine("Сервер остановлен.");
            }
        }

        public void Stop()
        {
            _listener.Stop();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposed)
                throw new ObjectDisposedException(typeof(ServerConnection).FullName);
            disposed = true;
            _listener.Stop();
            if (disposing)
            {
                lock (_clients)
                {
                    if (_clients.Count > 0)
                    {
                        Console.WriteLine("Отключаю клиентов...");
                        foreach (Connection client in _clients)
                        {
                            client.Dispose();
                        }
                        Console.WriteLine("Клиенты отключены.");
                    }
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


        ~ServerConnection() => Dispose(false);
    }
}

