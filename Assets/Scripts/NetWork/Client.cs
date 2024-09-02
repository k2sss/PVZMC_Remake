using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

namespace NetWork
{
    public class Client
    {
        public Socket socket;
        private string HostIp = "127.0.0.1";//需要链接的IP地址
        public int port;//端口好
        private byte[] buffer = new byte[1024 * 1024];
        // Start is called before the first frame update
        public Client(string ip,int port)
        {
            this.port = port;
            HostIp = ip;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(ip, port);
            StartRecive();
        }
        private void StartRecive()
        {
            socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallBack, null);
        }
        private void ReceiveCallBack(IAsyncResult iar)
        {
            int len = socket.EndReceive(iar);

            if (len == 0)//没收到消息
            {
                return;
            }
            string json = Encoding.UTF8.GetString(buffer, 0, len);
            Debug.Log("收到服务器的信息为:" + json);
            StartRecive();//网络循环
        }
        public void SendToServer(string json)
        {
            socket.Send(Encoding.UTF8.GetBytes(json));
        }
    }


}

