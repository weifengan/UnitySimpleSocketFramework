using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Threading;
using System.Text;
using System;
 

public delegate void SocketChange(SocketState status);
public class TCPUserToken
{

    public event SocketChange OnStatusChange=null;
    
    private Socket _socket;
    private Thread _thread;

    private byte[] buffer = new byte[4096];
    private bool loop = true;
    public TCPUserToken(Socket sk)
    {
        this._socket = sk;

        _thread = new Thread(ReceiveMsg);
        _thread.IsBackground = true;
        _thread.Start();
    }

    public string IPE{
        get{
            return _socket.RemoteEndPoint.ToString();
        }
    }
	///接收数据
    public void ReceiveMsg()
    {
        while (loop)
        {
            SocketState socketEvent;
            int length = -1;
            try
            {
                length = _socket.Receive(buffer);
            }
            catch (SocketException se)
            {
                Debug.Log("Socket Exception:" + se.Message);
                
                socketEvent=new SocketState(SocketStateType.DISCONNECTED,this);
                if(OnStatusChange!=null){
                    OnStatusChange(socketEvent);   
                }
                ShutDown();
                return;
            }
            catch (Exception ex)
            {
                Debug.Log("Socket Exception:" + ex.Message);
               
                socketEvent=new SocketState(SocketStateType.DISCONNECTED,this);
                if(OnStatusChange!=null){
                    OnStatusChange(socketEvent);   
                }
                 ShutDown();
                return;
            }

            if (length > 0)
            {
                byte[] data = new byte[length];
                Buffer.BlockCopy(buffer, 0, data, 0, length);

                Debug.Log("收到消息" + Encoding.UTF8.GetString(data));

                socketEvent=new SocketState(SocketStateType.RECEIVED,this);
                if(OnStatusChange!=null){
                    OnStatusChange(socketEvent);   
                }
            }

        }
    }

    public void SendData(string msg){
         if(_socket.Connected){
            _socket.Send(Encoding.UTF8.GetBytes(msg));
        }
    }

    public void ShutDown()
    {
        //停止接收线程
        if (_thread != null && _thread.IsAlive)
        {
            _thread.Interrupt();
            _thread.Abort();
        }

        if (_socket != null)
        {
            _socket.Close();
        }

		_socket=null;
		_thread=null;

        
    }

}
