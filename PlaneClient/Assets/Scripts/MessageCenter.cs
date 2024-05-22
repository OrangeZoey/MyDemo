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
    public const int SC_MoveH = 1001;//转弯
    public const int SC_MoveV = 1002;//前进
    public const int SC_Bend = 1003;//俯角
    public const int SC_FaceUpward = 1004;//仰角
    public const int SC_Left = 1005;//左滚
    public const int SC_Right = 1006;//右滚
    public const int SC_SpeedUp = 1007;//加速
    public const int SC_SpeedCut = 1008;//减速

    public const int CS_Login = 2001;//减速
    public const int SC_LoginCall = 2002;//减速 
    public const int SC_MoveCall = 2003;//减速
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