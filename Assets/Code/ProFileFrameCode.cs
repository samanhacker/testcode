using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProFileFrameCode : MonoBehaviour {

    public GameObject username;
    public GameObject Balance;


    // Use this for initialization
    void Start () {
  
    }
	
	// Update is called once per frame
	void Update () {
        string Bal = "------";
        try
        {
            Bal = Fa.num_to_money(Convert.ToInt64(Socket.SOC.MyBalance));
            username.GetComponent<Text>().text = Socket.SOC.MyUsername;
            Balance.GetComponent<Text>().text = Bal;
        }
        catch
        {
          
        }

       
    }
}
