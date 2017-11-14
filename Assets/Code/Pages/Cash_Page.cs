using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Cash_Page : MonoBehaviour {
    public Text Balance;
    public Text User_Count;
	public Text AgentText;
	public Text AgentButt;
    public InputField Payment;
    public InputField Payback;
    public Button PaybackButt;

	private bool isagent = false;

    public bool cash_wait = false;

	public Text Sample;

	public GameObject AgentContent;
	public GameObject TransContent;
	public GameObject PanelAgent;
	public GameObject PanelTrans;


	void OnEnable()  { 
		Balance.text =string.Format("موجودی: {0} ", Socket.SOC.MyBalance);   
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
		Socket.SOC.sndCM(new string[] { "cash" });
	}
	void OnDisable() { 		
		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}
        
	private void SOC_PackeTRecived(string Value)
	{
		var com = Value.Split('&');
		switch (com[0]) {
		case "cash":
			AgentText.text = string.Format ("کد معرفی شما: {0} ", com [2]);
			if (com [2] == "----") {
				isagent = false;
			} else {
				isagent = true;
			}

			break;
		case "paycash":
			Application.OpenURL (com [1]);
			break;
		case "payback":
			{				
				if (com [1] == "ok") {
					if (cash_wait == true) {
						cash_wait = false;
						Payback.text = "";
					} else {
						cash_wait = true;
					}
				}
			}
			break;
		case "translist":
			{
				TransContent.transform.DetachChildren();
				var items = com [1].Split ('#');		
				foreach (var item in items) {
					var x = Instantiate (Sample);
					x.GetComponent<UPersian.Components.RtlText>().text = item;
					x.transform.SetParent (TransContent.transform);
					x.transform.localScale = new Vector3 (1, 1);
				}

			}
			break;
		case "agentlist":
			{
				AgentContent.transform.DetachChildren();
				var items = com [1].Split ('#');		
				foreach (var item in items) {
					var x = Instantiate (Sample);
					x.GetComponent<UPersian.Components.RtlText>().text = item;
					x.transform.SetParent (AgentContent.transform);
					x.transform.localScale = new Vector3 (1, 1);
				}
			}
			break;
		case "cashwait":
			{		
				if (com [1] == "-1")
					cash_wait = false;
				else {
					cash_wait = true;
					Payback.text = com [1];
				}
			}
			break;      

		}

	}
   
	public void Show_Hide_Agent()
	{
		if (isagent) {
			PanelAgent.SetActive (!PanelAgent.active);
		} else {
			Socket.SOC.sndCM(new string[] { "AgentRequest"});
		}



	}
	public void Show_Hide_Trans()
	{
		PanelTrans.SetActive (!PanelTrans.active);
	}

    public void SendPayback()
    {
        if(cash_wait==true)
        {
            Socket.SOC.sndCM(new string[] { "payback","-1" });
        }else
        {
            Socket.SOC.sndCM(new string[] { "payback", Payback.text }); 
        }
    }
    public void ReCheckPayment()
    {
        Socket.SOC.sndCM(new string[] { "ReCheckPayment"});
    }
    public void paycash()
    {
        Socket.SOC.sndCM(new string[] { "paycash", Payment.text });
    }

    // Update is called once per frame
    void Update () {


        //F4B100FF
		if (isagent==false) {
			AgentButt.text = "درخواست کد معرفی";
		} else {
			AgentButt.text = "لیست کاربران معرفی شده";
		}

        if (cash_wait)
        {

            PaybackButt.image.color = new Color32(255, 0, 0,255);
            PaybackButt.GetComponentInChildren<Text>().text = "لغو درخواست";
            PaybackButt.GetComponentInChildren<Text>().color = Color.white;
            Payback.readOnly = true;
        }
        else
        {
            PaybackButt.image.color = new Color32(244, 177,0,255);
            PaybackButt.GetComponentInChildren<Text>().text = "درخواست بازگشت";
            PaybackButt.GetComponentInChildren<Text>().color = Color.black;
            Payback.readOnly = false;
        }
	}
    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
