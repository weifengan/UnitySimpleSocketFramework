using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
		TCPClientManager.Instance.Connect();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnGUI(){
		if(GUILayout.Button("send")){
			TCPClientManager.Instance.SendData("hello");
		}
	}
}
