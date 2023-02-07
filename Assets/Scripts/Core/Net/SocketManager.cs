using System;
using System.ComponentModel;
using System.Net.Sockets;
using UnityEditor.Build.Pipeline.Tasks;
using UnityEngine.Networking;

public class SocketManager
{
    private Socket _socket;

    private static SocketManager _inst = null;
    public static SocketManager Inst
    {
        get
        {
            if (_inst == null)
                _inst = new SocketManager();
            return _inst;
        }
    }
    
    public void Init()
    {
        //socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //socket
    }
}
