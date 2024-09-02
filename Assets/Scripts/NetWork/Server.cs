using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System;
using System.Text;

namespace NetWork
{
    
    public class Server
    {
        private Socket socket;
        public int port;
        private byte[] buffer = new byte[1024 * 1024];
        private List<Socket> clientList = new List<Socket>(); 
        public Server(int port)
        {
            this.port = port;
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(new IPEndPoint(IPAddress.Any, port));
            socket.Listen(4);
            StartAccept();
            Debug.Log("�Ѵ���������");
        }
        private void StartAccept()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }
        private void StartReceive(Socket client)
        {
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallBack, client);

        }
        private void ReceiveCallBack(IAsyncResult iar)
        {
            Socket client = iar.AsyncState as Socket;
            int len = client.EndReceive(iar);
            if (len == 0)
            {
                return;
            }
            string json = Encoding.UTF8.GetString(buffer, 0, len);
            Debug.Log("�ܵ��ͻ�����Ϣ" + json);

            //��Ӧ
            string responce = "�������Ѿ��յ������Ϣ:" + json;
            client.Send(Encoding.UTF8.GetBytes(responce));

           StartReceive(client) ; 
        }
        private void AcceptCallBack(IAsyncResult iar)
        {
           Debug.Log("��⵽�ͻ��˽���");
           Socket client = socket.EndAccept(iar);
           clientList.Add(client);
           client.Send(Encoding.UTF8.GetBytes("���Ѿ����ӵ�������"));
            //�ѿͻ�����������
           StartReceive(client);
           StartAccept();
        }
       public void SendMessageToClient(int index,string message)
        {
            
            if (index < clientList.Count)
            {
                clientList[index].Send(Encoding.UTF8.GetBytes(message));
            }
        }

    }
}

