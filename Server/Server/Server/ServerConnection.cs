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
        private int _MaxConnecthions = 0;//максимальное кол-во обрабатываемых клиентов
        private string _Adress;
        Socket _socket;
        public ServerConnection(string Adress, int MaxConnections)
        {
            _MaxConnecthions = MaxConnections;
            _Adress = Adress;
            IPEndPoint AdressEndPoint = new IPEndPoint(long.Parse(_Adress),8888);
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _socket.Bind(AdressEndPoint);

            Console.WriteLine(_socket.LocalEndPoint);
        }
    }
}

