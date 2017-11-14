using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSGBOX_CODE : MonoBehaviour {
    public Text Caption;
    public Text MsgText;
    public Button Yes_butt;
    public Button No_butt;
    //public delegate void ClickEvent();
    //public event ClickEvent YesClick;
    //public event ClickEvent NoClick;
    // Use this for initialization
    void Start () {
        No_butt.onClick.AddListener(MSGBOX_CODE_NoClick);
        //NoClick += MSGBOX_CODE_NoClick;
    }
    public void SetListener(bool NoButt,bool YesButt)
    {
        if(NoButt) No_butt.onClick.AddListener(MSGBOX_CODE_NoClick);
        else No_butt.onClick.RemoveListener(MSGBOX_CODE_NoClick);

        if (YesButt) Yes_butt.onClick.AddListener(MSGBOX_CODE_NoClick);
        else Yes_butt.onClick.RemoveListener(MSGBOX_CODE_NoClick);


    }

    private void MSGBOX_CODE_NoClick()
    {
        DestroyObject(this.gameObject);
    }

    // Update is called once per frame
    void Update () {        
		
	}
    public void Show(string _Msg, string _caption,string YesText="",string NoText="")
    {
        Caption.text = _caption;
        MsgText.text = _Msg;
        if(YesText!="")
        {
            Yes_butt.GetComponentInChildren<Text>().text = YesText;
        }
        if (NoText != "")
        {
            No_butt.GetComponentInChildren<Text>().text = NoText;
        }
    }
    public void Hide()
    {

        DestroyObject(this.gameObject);
    }
}
