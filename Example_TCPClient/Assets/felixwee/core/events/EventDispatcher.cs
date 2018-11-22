using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventHandler(SocketEvent evt);
public class EventDispatcher : MonoBehaviour
{

    private Dictionary<string, List<EventHandler>> mDic = new Dictionary<string, List<EventHandler>>();

    /// <summary>
    /// 添加监听
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="handler">回调函数</param>
    public void AddEventListener(string type, EventHandler handler)
    {
        if (mDic.ContainsKey(type))
        {
            mDic[type].Add(handler);
        }
        else
        {
            List<EventHandler> lstHandler = new List<EventHandler>();
            lstHandler.Add(handler);
            mDic[type] = lstHandler;
        }
    }
    /// <summary>
    /// 移除监听
    /// </summary>
    /// <param name="type">事件类型</param>
    /// <param name="handler">回调函数</param>
    public void RemoveEventLisener(string type, EventHandler handler)
    {

        if (mDic.ContainsKey(type))
        {
            List<EventHandler> lstHandler = mDic[type];
            lstHandler.Remove(handler);

            if (lstHandler.Count == 0)
            {
                mDic.Remove(type);
            }
        }
    }

	public void DispatchEvent(SocketEvent evt){
		if(mDic.ContainsKey(evt.Type)){
			List<EventHandler> lstHandler=mDic[evt.Type];
			if(lstHandler!=null && lstHandler.Count>0){
				for(int i=0;i<lstHandler.Count;i++){
					if(lstHandler[i]!=null){
						lstHandler[i](evt);
					}
				}
			}
		}
	}
}
