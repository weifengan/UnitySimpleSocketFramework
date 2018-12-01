using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SocketEvent
{

	public const string CONNECTED="connect";
	public const string RECEIVED="received";
	public const string DISCONNECT="disconnect";
	
    private object _target;
    private string _type;


	private byte[] _data;

    /// <summary>
    /// SocketEvent为网络事件类
    /// </summary>
    /// <param name="ms"></param>
    public SocketEvent(string type, object target,byte[] data)
    {
        this._target = target;
        this._type = type;
		this._data=data;
    }

    public string Type
    {
        get
        {
            return _type;
        }

        set
        {
            _type = value;
        }
    }

    public object Target
    {
        get
        {
            return _target;
        }

        set
        {
            _target = value;
        }
    }

    public byte[] Data
    {
        get
        {
            return _data;
        }

        set
        {
            _data = value;
        }
    }
}
