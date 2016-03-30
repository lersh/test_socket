using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SocketServer
{
    class Program
    {
        //负责监听端口
        static Socket sokWelcome = null;
        //负责和客户端Socket通信
        static Socket sokConnection = null;
        //负责监听
        static Thread threadWatchPort = null;

        static void Main(string[] args)
        {
            Console.WriteLine("Server is Running...");
            StartListeningUDP();

            Console.ReadLine();

        }

        static public void StartListeningUDP()
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(address, 1234);

            sokWelcome = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            sokWelcome.Bind(endpoint);

            threadWatchPort = new Thread(WatchPortUDP);
            threadWatchPort.Start();


            Console.WriteLine("Stating UDP Listenning...");

        }

        private static void WatchPortUDP()
        {
            while (true)
            {
                try
                {
                    byte[] udpMsg = new byte[4096];
                    IPEndPoint iep = new IPEndPoint(IPAddress.Any, 0);
                    EndPoint endpoint = (EndPoint)iep;
                    string udpMsgString = string.Empty;
                    int msgLength = 0;

                    msgLength = sokWelcome.ReceiveFrom(udpMsg, ref endpoint);
                    iep = (IPEndPoint)endpoint;
                    udpMsgString = System.Text.Encoding.UTF8.GetString(udpMsg, 0, msgLength);

                    Console.WriteLine("收到来自 {0}:{1} 的消息：{2}", iep.Address.ToString(), iep.Port.ToString(), udpMsgString);

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }





        static public void StartListeningTCP()
        {
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(address, 1234);

            sokWelcome = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sokWelcome.Bind(endpoint);
            sokWelcome.Listen(10);

            threadWatchPort = new Thread(WatchPortTCP);
            threadWatchPort.Start();

            Console.WriteLine("Stating TCP Listenning...");


        }


        private static void WatchPortTCP()
        {
            while (true)
            {
                try
                {

                    sokConnection = sokWelcome.Accept();
                    IPEndPoint remoteIPEndPoint = (IPEndPoint)sokConnection.RemoteEndPoint;
                    Console.WriteLine("监听到新的连接了,IP:" + remoteIPEndPoint.Address + ":" + remoteIPEndPoint.Port);
                    SendMessageTCP("你好！！");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("错误：" + ex.Message);
                }
                //break;
            }
        }

        static public void SendMessageTCP(string message)
        {
            byte[] messageBytes = System.Text.Encoding.UTF8.GetBytes(message);
            sokConnection.Send(messageBytes, messageBytes.Length, SocketFlags.None);
        }

    }
}
