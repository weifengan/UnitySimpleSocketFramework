/**************************************
          TCPClientManager

    desc: Socket TCP协议客户端核心类
    author:felixwee
	email:felixwee@163.com
	website: www.felixwee.com
****************************************/
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class TCPClientManager : MonoBehaviour
{

     private Socket _socket;
     private Thread _thread;

     private bool loop=true;

     private byte[] buffer=new byte[2048];

    
    private static TCPClientManager _instance;


    public static TCPClientManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("_" + typeof(TCPClientManager).Name);
                _instance = go.AddComponent<TCPClientManager>();

                _instance._socket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
                //_instance._socket.SendTimeout=3000;
                //_instance._socket.ReceiveTimeout=3000;
                //_instance._socket.NoDelay=true;
                DontDestroyOnLoad(_instance);
            }
            return _instance;
        }
    }

    public void Connect(string host="127.0.0.1",int port=9339){
        try{
            
            _socket.Connect(new IPEndPoint(IPAddress.Parse(host),port));

            Debug.Log("connect to server "+host+":"+port+ "  success!");
        }catch(Exception ex){
            Debug.Log(string.Format("connect to server{0} failed!:{1}",host+":"+port,ex.Message));
            return;
        }

        _thread=new Thread(new ThreadStart(ReceiveMessage));
        _thread.IsBackground=true;
        _thread.Start();
    }
 
    private void ReceiveMessage()
    {
        while(loop){
            int length=-1;
            try{
                length=_socket.Receive(buffer);
            }catch(SocketException se){
                Debug.Log("Socket Exception:"+se.Message);
                ShutDown();
                return;
            }catch(Exception ex){
                Debug.Log("Socket Exception:"+ex.Message);
                ShutDown();
                return;
            }

            if(length>0){
                byte[] data=new byte[length];
                Buffer.BlockCopy(buffer,0,data,0,length);

                Debug.Log("收到消息"+Encoding.UTF8.GetString(data));

            }
        }
    }


    public void SendData(string msg){
        if(_socket.Connected){
            _socket.Send(Encoding.UTF8.GetBytes(msg));
        }
    }
    private void ShutDown(){
        if(_socket!=null){
            _socket.Close();
        }

        if(_thread!=null){
            _thread.Interrupt();
            _thread.Abort();
        }
    }
    private void OnDestroy(){
        loop=false;
        ShutDown();
    }
}
