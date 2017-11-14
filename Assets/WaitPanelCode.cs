using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitPanelCode : MonoBehaviour {
    public Text Caption;
    public Text User1;
    public Text User2;
    public Text Money;
    public Text Count;
    public Text Time;
    public Text Title;
    public string RoundID;
    public Button Cancel_Butt;
    public Button ok_Butt;
    public Button Cancel2_Butt;



    // Use this for initialization
    void OnEnable() {
        
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
    }
	void OnDisable()
	{
		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}
	private void SOC_PackeTRecived(string Value)
	{
		var com = Value.Split('&');
		if (com [0] == "room")
		if (com [1] == "main")
			DestroyObject (transform.parent.gameObject);
			
	}

    public void Show(string _user1, string _user2, string _money, string _count,string _time ,bool isready)
    {
        this.gameObject.SetActive(true);
        User1.text = _user1;
        User2.text = _user2;
        Money.text = _money;
        Count.text = _count;
        Time.text = _time;
        if(isready)
        {
            Cancel_Butt.gameObject.SetActive(true);
            ok_Butt.gameObject.SetActive(true);
            Cancel2_Butt.gameObject.SetActive(false);
        }
        else
        {
            Cancel_Butt.gameObject.SetActive(false);
            ok_Butt.gameObject.SetActive(false);
            Cancel2_Butt.gameObject.SetActive(true);
        }
     

    }
    public void MSGBOX_CODE_Ready()
    {
        Socket.SOC.writeSocket(string.Format("imready&{0}&",RoundID));
       
    }
    public void MSGBOX_CODE_NotReady()
    {
   
        Socket.SOC.writeSocket(string.Format("imnotready&{0}&", RoundID));
     
    }
    public void MSGBOX_CODE_NotReady2()
    {
     
        Socket.SOC.writeSocket(string.Format("imnotready2&{0}&", RoundID));
   
    }
    // Update is called once per frame
    void Update () {
		
	}
}
