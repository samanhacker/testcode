using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MSGBOX : MonoBehaviour {

    // Use this for initialization
    public Text Caption;
    public Text MsgText;
    public Button Ok;
	void Start () {
        
	}

    public void Show(string _Msg, string _caption)
    {
        Caption.text = Fa.faConvertLine(_caption);
        MsgText.text = _Msg;
    }
    public void Hide()
    {

        DestroyObject(this.gameObject);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
