/**************************************
          TCPSocketManager

    desc: Socket TCP协议服务器端核心 
    author:felixwee
	email:felixwee@163.com
	website: www.felixwee.com
****************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System;

public class TCPSocketManager : MonoBehaviour
{

    private Socket _socket;
    private IPEndPoint _ipe;

    private Thread _listenThread;

    /// <summary>
    /// 客户端列表
    /// </summary>
    private Dictionary<string,TCPUserToken> _userList=new Dictionary<string, TCPUserToken>();
    private bool loop = true;
    private static TCPSocketManager _instance;
    public static TCPSocketManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("_" + typeof(TCPSocketManager).Name);
                _instance = go.AddComponent<TCPSocketManager>();
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }
    /// <summary>
    /// 初始化TCP Socket Manager
    /// </summary>
    public void Init(string host = "127.0.0.1", int port = 9339)
    {
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        _ipe = new IPEndPoint(IPAddress.Parse(host), port);

        try
        {
            _socket.Bind(_ipe);
            _socket.Listen(0);
            Debug.Log(string.Format("server {0} is running ....", _ipe.ToString()));

            //开启客户端连接线程
            _listenThread = new Thread(OnListenConnectHandler);
            _listenThread.IsBackground = true;
            _listenThread.Start();

        }
        catch (SocketException se)
        {
            Debug.Log(string.Format("start server {0} failed:{1}", _ipe.ToString(), se.Message));
        }
    }


    ///处理客户端的连接
    private void OnListenConnectHandler()
    {
        while (loop)
        {
            //
            Socket clientsk = _socket.Accept();
            Debug.Log(clientsk.GetHashCode());
            Debug.Log(string.Format("client {0} connected to server!", clientsk.RemoteEndPoint.ToString()));

            TCPUserToken token=new TCPUserToken(clientsk);
            token.OnStatusChange+=OnUserTokenStatusChanged;

            //添加到客户端列表中
            _userList.Add(token.IPE,token);
        }
    }

    
    ///用户状态有改变
    private void OnUserTokenStatusChanged(SocketState status)
    {
        switch(status.Type){
            case SocketStateType.DISCONNECTED:
               _userList.Remove(((TCPUserToken)status.Target).IPE);
            break;
        }
    }

    ///广播消息
    public void Broadcast(string msg)
    {   
         foreach (var item in this._userList.Values)
         {
             item.SendData(msg);
         }
    }
    private void ShutDown()
    {
        //关闭等待链接县城
        if (_listenThread != null)
        {
            _listenThread.Interrupt();
            _listenThread.Abort();
        }

        if (_socket != null)
        {
            _socket.Close();
        }
        _listenThread = null;
        _socket.Close();
    }

    private void OnDestroy()
    {
        loop = false;
        ShutDown();
    }


    public int NumClients
    {
        get
        {
            return _userList.Count;
        }
    }
}
