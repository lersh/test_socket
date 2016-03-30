using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;

namespace SockectClient
{
    class Program
    {
        static Socket sokClient = null;
        static Thread threadClient = null;
        static void Main(string[] args)
        {
            Console.WriteLine("Client Start!");
            ConnectingUDP();
            Console.ReadLine();
        }


        private static void ConnectingUDP()
        {
            sokClient = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(address, 1234);

            //sokClient.Connect(endpoint);
            SendMsgUdp(endpoint, "你好，服务器！");
            Thread.Sleep(2000);
            SendMsgUdp(endpoint, "你好，服务器！这是第二条消息！");
            Thread.Sleep(2000);
            SendMsgUdp(endpoint, "你好，服务器！这是第三条消息！");
        }

        private static void SendMsgUdp(IPEndPoint endpoint, string msg)
        {
            byte[] byteMsg = System.Text.Encoding.UTF8.GetBytes(msg);
            sokClient.SendTo(byteMsg, endpoint);
        }



        static private void ConnectingTCP()
        {
            sokClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress address = IPAddress.Parse("127.0.0.1");
            IPEndPoint endpoint = new IPEndPoint(address, 1234);

            sokClient.Connect(endpoint);
            ReciveMsgTCP();
        }

        static private void ReciveMsgTCP()
        {
            while (true)
            {
                byte[] byteMsg = new byte[1024 * 1024 * 4];
                int length = sokClient.Receive(byteMsg);
                string strMsg = Encoding.UTF8.GetString(byteMsg, 0, length);
                Console.WriteLine(strMsg);
            }
        }
    }
}
