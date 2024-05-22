using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

using UnityEngine;

public class ClientNetManager : Singleton<ClientNetManager>
{
    Socket socket;
    byte[] data = new byte[1024];
    Queue<byte[]> queue = new Queue<byte[]>();

    public void Init()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        socket.BeginConnect("127.0.0.1", 7777, OnAsyncConnectCall, null);
    }

    private void OnAsyncConnectCall(IAsyncResult ar)
    {
        try
        {
            socket.EndConnect(ar);

            socket.BeginReceive(data, 0, data.Length, SocketFlags.None, OnAsyncReceiveCall, null);

            Debug.Log("连接到服务器");
        }
        catch (Exception)
        {

            throw;
        }
    }

    private void OnAsyncReceiveCall(IAsyncResult ar)
    {
        try
        {
            int len = socket.EndReceive(ar);
            Debug.Log($"客户端接收长度 {len} ");

            if (len > 0)
            {
                byte[] bytes = new byte[len];
                Buffer.BlockCopy(data, 0, bytes, 0, len);

                while (bytes.Length > 8)
                {
                    int bodyLen = BitConverter.ToInt32(bytes, 0);
                    byte[] bodyData = new byte[bodyLen];
                    Buffer.BlockCopy(bytes, 4, bodyData, 0, bodyLen);

                    queue.Enqueue(bodyData);

                    int remainLen = bytes.Length - 4 - bodyLen;
                    byte[] remainData = new byte[remainLen];
                    Buffer.BlockCopy(bytes, 4 + bodyLen, remainData, 0, remainLen);
                    bytes = remainData;
                }

                socket.BeginReceive(data, 0, data.Length, SocketFlags.None, OnAsyncReceiveCall, null);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void OnSendToServer(int id, byte[] info)
    {

        int bodyLen = 4 + info.Length;

        byte[] bytes = new byte[0].Concat(BitConverter.GetBytes(bodyLen)).Concat(BitConverter.GetBytes(id)).Concat(info).ToArray();

        Debug.Log($"客户端发送消息号{id}");

        socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, OnAsyncSendCall, null);

    }

    private void OnAsyncSendCall(IAsyncResult ar)
    {


        try
        {
            int len = socket.EndSend(ar);

            Debug.Log($"客户端发送长度 {len}");
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    public void UpDate()
    {
        while (queue.Count > 0)
        {
            byte[] bytes = queue.Dequeue();

            int id = BitConverter.ToInt32(bytes, 0);
            byte[] info = new byte[bytes.Length - 4];
            Buffer.BlockCopy(bytes, 4, info, 0, bytes.Length - 4);

            Debug.Log($"接收消息号{id}");

            MessageCenter<byte[]>.Instance.Ondispatch(id, info);
        }
    }
}
