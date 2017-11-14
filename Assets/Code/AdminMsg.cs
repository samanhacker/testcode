using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AdminMsg : MonoBehaviour {
    public Text Caption;
    public Text MsgText;
    public string BackLink = "";
    public Button No_butt;
    // Use this for initialization
	void OnEnable()  { 
		No_butt.onClick.AddListener(MSGBOX_CODE_NoClick);
	


	}
	void OnDisable() { 
		
		No_butt.onClick.RemoveListener(MSGBOX_CODE_NoClick);

	}
    void Start () {
     
    }
    public void Show(string _Msg, string _caption, string NoText = "")
    {
        Caption.text = _caption;
        MsgText.text = _Msg;
    
        if (NoText != "")
        {
            No_butt.GetComponentInChildren<Text>().text = NoText;
        }
    }
    private void MSGBOX_CODE_NoClick()
    {
        if (BackLink != "") Socket.SOC.writeSocket(BackLink);
		DestroyObject (transform.parent.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
