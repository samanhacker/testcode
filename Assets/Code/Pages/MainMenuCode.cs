using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCode : MonoBehaviour {

    public GameObject username;
    public GameObject Balance;
	public GameObject Butt_CreateGame;

    public Text Sample;
    public GameObject OnlineList;


    public GameObject Butt_CancelGame;
    bool IsWait = false;
    //public GameObject CreateGame;

    public void setinfo(string[] para)
    {
        username.GetComponent<UnityEngine.UI.Text>().text = para[1]; //Fa.faConvertLine("سعید");
        Balance.GetComponent<UnityEngine.UI.Text>().text = para[2];
    }


    public void SendCommand(string command)
    {
        Socket.SOC.writeSocket(command);//"CanselGameB&");
    }
  
    public void Load_scene(string name)
    {
        SceneManager.LoadScene(name);
    }
	void OnEnable()  { 
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;

    //Socket.SOC.writeSocket("Status&");
        


    }
    void OnDisable() { 

		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}
   

    private void SOC_PackeTRecived(string Value)
    {       
 
        var com = Value.Split('&');

        if (com[0]=="mainmenu" )
        {
            GameBase.OnlineCount = com[1];         
            if(com[4] != "") Socket.SOC.AdminMsg(com[4], "readmsg");
        }
       

        if (com[0] == "OnlineList" )
        {
            OnlineList.transform.DetachChildren();
            var items = com[1].Split('#');
            foreach (var item in items)
            {
                var x = Instantiate(Sample);
                x.GetComponent<UPersian.Components.RtlText>().text = item;
                x.transform.SetParent(OnlineList.transform);
                x.transform.localScale = new Vector3(1, 1);
            }
        }
        

    }
    public void TelegramOpen()
    {
        Application.OpenURL("http://telegram.me/gamesstars");
    }

    // Update is called once per frame
    void Update () {
        if (Socket.SOC.IsWaiting == true)
        {
            Butt_CancelGame.SetActive(true);
            Butt_CreateGame.SetActive(false);        
            IsWait = true;
        }
        else
        {
            Butt_CancelGame.SetActive(false);
            Butt_CreateGame.SetActive(true);     
            IsWait = false;
        }
    }


}
