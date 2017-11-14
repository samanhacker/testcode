using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class _OnlineSender : MonoBehaviour {

	private DateTime Lastsend;
	// Use this for initialization
	void Start () {
		Lastsend = DateTime.Now;
	}
	
	// Update is called once per frame
	void Update () {
		if (DateTime.Now.Subtract (Lastsend).TotalSeconds > 3) {
			Lastsend = DateTime.Now;
			if(Socket.SOC.socket_ready)Socket.SOC.writeSocket ("online&");
		}
	}
}
