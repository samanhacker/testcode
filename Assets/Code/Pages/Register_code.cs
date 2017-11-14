using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Register_code : MonoBehaviour {

    public bool IsAgentBuild = false;
  
    // Use this for initialization
    public InputField username;
    public InputField password;
    public InputField email;
    public InputField mobile;
	public GameObject RegisterPanel;
	public InputField _AgentCode;
	public static int Avatarnum=0;
    public string AgentID="-1";

	void OnEnable()  {
        if (IsAgentBuild)
            RegisterPanel.SetActive(true);
        Socket.SOC.PackeTRecived += SOC_PackeTRecived;
      

    }
	void OnDisable() { 

		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}
    private void SOC_PackeTRecived(string Value)
    {
        var com = Value.Split('&');
		if (com [0] == "Register" && com [1] == "ok") {
			BackToMAinenu ();
			Socket.SOC.PM ("ثبت نام با موفقیت انجام شد");

		}
		switch (com [0]) {
		case "Register":
			break;
		case "CheckParent":
			AgentID = com [1];
			show_hide_RegisterPanel ();

			break;
		}
    }
    public void BackToMAinenu()
    {
        SceneManager.LoadScene("LoginScene");
    }


    public void Register()
    {

        var us=username.text;
        var pass = password.text;
        var em = email.text;
        var mob = mobile.text;

		Socket.SOC.sndCM(new string[]{ "Register", us,pass,"",em,mob,AgentID,Avatarnum.ToString()});

    }
	public void show_hide_RegisterPanel()
	{
        if (IsAgentBuild)
        {
            BackToMAinenu();
        }
        else
        {
            RegisterPanel.SetActive(!RegisterPanel.active);
        }
	}

	public void checkAgent()
	{
		
		Socket.SOC.sndCM(new string[]{ "CheckParent", _AgentCode.text});
	}
     

	// Update is called once per frame
	void Update () {
		
	}
}
