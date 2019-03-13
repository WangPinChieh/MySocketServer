using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace MySocketServer
{
    class Program
    {
        public static Socket m_SocketServer;
        static void Main(string[] args)
        {
            StartSocketServer();
        }
        public static void StartSocketServer() {
            IPAddress _IPAddress = IPAddress.Parse("127.0.0.1");
            IPEndPoint _IPEndPoint = new IPEndPoint(_IPAddress, 11000);
            try {
                m_SocketServer = new Socket(_IPAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                m_SocketServer.Bind(_IPEndPoint);
                m_SocketServer.Listen(10);
                Console.WriteLine("Waiting for a connection ...");
                Socket _Handler = m_SocketServer.Accept();

                string _Data = null;
                byte[] _Bytes = null;

                while (true)
                {
                    _Data = null;
                    _Bytes = new byte[1024];
                    int _BytesReceived = _Handler.Receive(_Bytes);
                    _Data += Encoding.ASCII.GetString(_Bytes, 0, _BytesReceived);

                    Console.WriteLine("Text Received : " + _Data);

                    byte[] _ResponseMessage = Encoding.ASCII.GetBytes("Text Received : " + _Data);
                    _Handler.Send(_ResponseMessage);

                    if (_Data.IndexOf("<EOF>") != -1)
                        break;
                }
                _Handler.Shutdown(SocketShutdown.Both);
                _Handler.Close();

            }
            catch (Exception exp) {
                Console.WriteLine(exp.Message);
            }
        }
    }
}
