using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TCPSocketManager tcpMgr=TCPSocketManager.Instance;
		TCPSocketManager tcpMgr1=TCPSocketManager.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(GUILayout.Button("Click")){
			Debug.Log("重新进入 "+(TCPSocketManager.Instance==TCPSocketManager.Instance));
			
			if(SceneManager.GetActiveScene().name=="AAA"){
					SceneManager.LoadScene("BBB");
			}else{
				SceneManager.LoadScene("AAA");
			}
			
		}

		if(GUILayout.Button("send")){
			 TCPSocketManager.Instance.Broadcast("i'm server msg!");
		}
	}
}
