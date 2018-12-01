using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TCPSocketManager tcpMgr=TCPSocketManager.Instance;
        tcpMgr.Init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(GUILayout.Button("Click")){

			
			if(SceneManager.GetActiveScene().name=="AAA"){
					SceneManager.LoadScene("BBB");
			}else{
				SceneManager.LoadScene("AAA");
			}
			
		}

		if(GUILayout.Button("send")){
			 TCPSocketManager.Instance.Broadcast("i'm server msg!");
		}

        GUILayout.Label("当前客户端数量:" + TCPSocketManager.Instance.NumClients);
	}
}
