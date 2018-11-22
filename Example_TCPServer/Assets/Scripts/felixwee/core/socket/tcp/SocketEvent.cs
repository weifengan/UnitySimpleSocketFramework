using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SocketStateType{
	NONE,CONNECTED,DISCONNECTED,RECEIVED
}
public class SocketState  {
	private SocketStateType _type;
	private object _target;

	private byte[] _bytes;

	private object _data;
	public SocketState(SocketStateType type,object target,byte[] bytes=null,object data=null){
		this._bytes=bytes;
		this._target=target;
		this._type=type;
		this._data=data;
	}


	

	public override string ToString(){
       return string.Format("type={0} target={1} bytes={2} data={3}",_type,_target,_bytes,_data);
	}
    public byte[] Bytes
    {
        get
        {
            return _bytes;
        }

    }

    public object Target
    {
        get
        {
            return _target;
        }
    }

    public SocketStateType Type
    {
        get
        {
            return _type;
        }
    }

    public object Data
    {
        get
        {
            return _data;
        }
    }
}
