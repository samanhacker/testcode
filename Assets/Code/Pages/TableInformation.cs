
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public static class Extensions
{
	public static Transform Search(this Transform target, string name)
	{
		if (target.name == name) return target;

		for (int i = 0; i < target.childCount; ++i)
		{
			var result = Search(target.GetChild(i), name);

			if (result != null) return result;
		}

		return null;
	}
}
public class TableInformation : MonoBehaviour { 
	    
    public Button UndoButt;
    public GameObject Bubble;
    public GameObject Bubbleme;
	public GameObject InGameMsg;

    private DateTime mytime = DateTime.Now;
    private DateTime opptime = DateTime.Now;
	private DateTime InGameMsgTime = DateTime.Now;

    public GameObject chatlist;
    public GameObject mainmenu;
	public Button MainMenuButt;
	public GameObject PanelStartGame;





    // Use this for initialization
    void OnEnable()
    {
        Socket.SOC.PackeTRecived += SOC_PackeTRecived;
        Bubble = Extensions.Search(transform, "ShowChatPanel").gameObject;
        Bubbleme = Extensions.Search(transform, "ShowChatPanelme").gameObject;
        InGameMsg = Extensions.Search(transform, "InGameMsgPanel").gameObject;

    }
	void OnDisable() { 

		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}

    private void SOC_PackeTRecived(string Value)
    {
        var com = Value.Split('&');
		if(com[0]== "chat")
        {
            showchat(com[1], false);
        }
		if(com[0]== "PM3")
		{
			ShowInGameMsg(com[1]);
		}
		if(com[0]== "StartResult")
		{
			PanelStartGame.SetActive (true);
			PanelStartGame.GetComponent<BackGammonStart>().Show(Convert.ToInt16(com[1]),Convert.ToInt16(com[2]),com[3]=="w");
		}
    }

	public void ShowInGameMsg(string msg)
	{
		InGameMsg.SetActive(true);
		var code = InGameMsg.GetComponentInChildren<UPersian.Components.RtlText>();
		code.text = msg;
		InGameMsgTime = DateTime.Now.AddSeconds(2);
	}
    public void showchat(string msg,bool ismy)
    {
        if(ismy)
        {
            Bubbleme.SetActive(true);
            var code = Bubbleme.GetComponentInChildren<UPersian.Components.RtlText>();
            code.text = msg;
            mytime = DateTime.Now.AddSeconds(2);
        }
        else
        {
            Bubble.SetActive(true);
            var code = Bubble.GetComponentInChildren<UPersian.Components.RtlText>();
            code.text = msg;
            opptime = DateTime.Now.AddSeconds(2);
        }
    }
    // Update is called once per frame
    void Update () {
        var diff = mytime.Subtract(DateTime.Now).TotalSeconds;
        if (diff <= 0) Bubbleme.SetActive(false);
        diff = opptime.Subtract(DateTime.Now).TotalSeconds;
        if (diff <= 0) Bubble.SetActive(false);
		diff = InGameMsgTime.Subtract(DateTime.Now).TotalSeconds;
		if (diff <= 0) InGameMsg.SetActive(false);



        

		if (UndoButt != null && GameBase.IsGamePreview==false)
        {
            if (BackGammon.Undo.Count() > 0 && Socket.SOC.GameBG.GameColor == Socket.SOC.GameBG.Nobat)
                UndoButt.gameObject.SetActive(true);
            else UndoButt.gameObject.SetActive(false);
        }
		if (GameBase.IsGamePreview == false) {
			MainMenuButt.GetComponentInChildren<UPersian.Components.RtlText> ().text = "منوی بازی";
		} else {
			MainMenuButt.GetComponentInChildren<UPersian.Components.RtlText> ().text = "بازگشت";
		}


      
    }
    public void sendmsg(string text)
    {

        chatlist.SetActive(false);
        showchat(text, true);
        Socket.SOC.sndCM(new string[] { "chat", text });
    }
    public void Undo()
    {
        Socket.SOC.GameBG.UndoMove();
        Socket.SOC.sndCM(new string[] { "undo" });
    }
	public void RollDice()
	{		
		//Socket.SOC.GameBG.PlayRollDiceSound ();
		
		Socket.SOC.sndCM(new string[] { "RollDice" });
	}

    public void Resign()
    {
        mainmenu_show_hide();
        var x = Instantiate(Socket.SOC.YesNoPanel);
        var code = x.GetComponent<MSGBOX_CODE>();
        code.SetListener(true, true);
        code.Show("آیا مطمئن هستید؟.", "اعلام باخت");
        code.Yes_butt.onClick.AddListener(sendResign);
    }


    public void CloseApp()
    {
        mainmenu_show_hide();
        var x = Instantiate(Socket.SOC.YesNoPanel);
        var code = x.GetComponent<MSGBOX_CODE>();
        code.Show("آیا مطمئن هستید؟", "خروج");
        code.Yes_butt.onClick.AddListener(Application.Quit);
    }
    public void refresh()
    {
        Socket.SOC.sndCM(new string[] { "Refresh" });
		mainmenu.SetActive(false);

    }
    public void chat_show_hide()
    {
		if(GameBase.IsGamePreview == false) chatlist.SetActive(!chatlist.active);
    }
    public void mainmenu_show_hide()
    {
		if (GameBase.IsGamePreview == false) {
			mainmenu.SetActive(!mainmenu.active);
		} else {
			Socket.SOC.sndCM(new string[] { "ExitView","BackGammon" });
			SceneManager.LoadScene ("SearchGame");

		}

       
    }

    public void sendResign()
    {
        Socket.SOC.sndCM(new string[] { "ResignGame" });
		mainmenu.SetActive(false);
    }

}
