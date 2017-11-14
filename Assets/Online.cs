using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Online : MonoBehaviour {
    DateTime SendTime;
	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(gameObject);
        SendTime = DateTime.Now;
    }
	
	// Update is called once per frame
	void Update () {
        if(DateTime.Now.Subtract(SendTime).TotalSeconds>3)
        {
            SendTime = DateTime.Now;
            Socket.SOC.sndCM(new string[] { "onlinexxx" });
        }
		
	}
}
