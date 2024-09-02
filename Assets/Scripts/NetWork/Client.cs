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
        private string HostIp = "127.0.0.1";//��Ҫ���ӵ�IP��ַ
        public int port;//�˿ں�
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

            if (len == 0)//û�յ���Ϣ
            {
                return;
            }
            string json = Encoding.UTF8.GetString(buffer, 0, len);
            Debug.Log("�յ�����������ϢΪ:" + json);
            StartRecive();//����ѭ��
        }
        public void SendToServer(string json)
        {
            socket.Send(Encoding.UTF8.GetBytes(json));
        }
    }


}

