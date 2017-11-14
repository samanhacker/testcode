using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UPersian.Components;

public class TicketPage : MonoBehaviour {
    public InputField answer;
    public InputField subject;
    public InputField comment;
    public Text status;
    // Use this for initialization
	void OnEnable()  { 
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
		Socket.SOC.sndCM(new string[] { "ticket", "load" });
	}
	void OnDisable() { 

		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}
 
    public void SendTicket()
    {
        Socket.SOC.sndCM(new string[] { "ticket", "write",subject.text,comment.text });
    }
    private void SOC_PackeTRecived(string Value)
    {
        var com = Value.Split('&');
        if (com[0] == "ticket")
        {
            switch(com[1])
            {
                case "empty":
                    subject.text = "";
                    comment.text = "";
                    answer.text = "";
                    break;
                case "load":
                    subject.text = com[2];
                    comment.text = com[3];
                    answer.text = com[4];
                    status.text = (com[5] == "1" ? "وضعيت پيام: خوانده شده" : "وضعيت پيام: خوانده نشده");
                    break;
                case "saved":
                    Socket.SOC.PM("درخواست شما ثبت شد.");
                    break;
            }

        }
    }
    public void Goto_Page(string pagename)
    {
        SceneManager.LoadScene(pagename);
    }
    // Update is called once per frame
    void Update () {
       
    }
}
