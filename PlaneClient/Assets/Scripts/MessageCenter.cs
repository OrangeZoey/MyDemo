using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;

public class MessageCenter<T> : Singleton<MessageCenter<T>>
{
    Dictionary<int, Action<T>> loaded = new Dictionary<int, Action<T>>();

    public void OnAddListener(int id, Action<T> action)
    {
        if (loaded.ContainsKey(id))
        {
            loaded[id] += action;
        }
        else
        {
            loaded.Add(id, action);
        }
    }

    public void Ondispatch(int id, T t)
    {
        if (loaded.ContainsKey(id))
        {
            loaded[id](t);
        }
    }
}


public class Define
{
    public const int SC_MoveH = 1001;//ת��
    public const int SC_MoveV = 1002;//ǰ��
    public const int SC_Bend = 1003;//����
    public const int SC_FaceUpward = 1004;//����
    public const int SC_Left = 1005;//���
    public const int SC_Right = 1006;//�ҹ�
    public const int SC_SpeedUp = 1007;//����
    public const int SC_SpeedCut = 1008;//����

    public const int CS_Login = 2001;//����
    public const int SC_LoginCall = 2002;//���� 
    public const int SC_MoveCall = 2003;//����
}
public class Message
{
    public byte[] Data = new byte[1024];
    public Client Client;
}
public class Client
{
    public Socket Socket;
    public string SocketPort;
    public byte[] Data = new byte[1024];  
}