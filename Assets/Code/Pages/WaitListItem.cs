using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class WaitListItem : MonoBehaviour {

    // Use this for initialization
	public enum GameList
	{
		Hokm,BackGammon
	}
	public enum StateList
	{
		Wait,Playing
	}
	public StateList state=StateList.Wait;
	public GameList Game=GameList.BackGammon;
	public long price=1000;
	public string count="--";
	public string time="نرمال";
	public string type ="معمولی";
	public string p1avatar="1";
	public string p2avatar="1";
	public string p1username="player1";
	public string p2username="player2";
	public string ID="xxx";
	private bool visible_state = true;


    public Text Title;
    public Button SelectButt;
	public  Image Icon;



	void Start () {
		
	}

    public void Send()
    {
		if (state == StateList.Wait) {
			Socket.SOC.writeSocket ("JoinGame&" + ID + "&");
		} else {
			Socket.SOC.writeSocket (string.Format ("ViewGame&{0}&{1}&",Game,ID));
		}

      
    }
	public void SetPara()
	{
		
		var txt = string.Format (" {0} {1} {2} {3} " ,Fa.num_to_money(price), count, type, time) + Environment.NewLine;

		if (state == StateList.Playing) {
			GetComponent<Image> ().color = new Color32 (41, 41, 52, 255);
			SelectButt.GetComponentInChildren<UPersian.Components.RtlText>().text= "جزئیات";
			txt+=string.Format("{0} - {1}",p1username,p2username);
		} else {
			GetComponent<Image> ().color = new Color32 (255, 255, 255, 100);
			SelectButt.GetComponentInChildren<UPersian.Components.RtlText>().text= "شروع بازی";
			txt+=string.Format("{0}",p1username);
		}
		if (Game == GameList.BackGammon) {
			Icon.GetComponent<Image> ().sprite =  Resources.Load<Sprite>("GameIcon");
		} else {
			Icon.GetComponent<Image> ().sprite =  Resources.Load<Sprite>("HokmIcon");
		}

		Title.GetComponent<UPersian.Components.RtlText> ().text = txt;

	}
	void hide()
	{
		if (visible_state != false) {
			visible_state = false;
			RectTransform rt = GetComponent<RectTransform> ();
			rt.sizeDelta = new Vector2 (200, 0);
		}
			
	}
	void show()
	{
		if (visible_state != true) {
			visible_state = true;
			RectTransform rt = GetComponent<RectTransform> ();
			rt.sizeDelta = new Vector2 (200, 50);
		}

	}
	// Update is called once per frame
	void Update () {

		//SetPara ();

		if (price < SearchGame._MinVal || price > SearchGame._MaxVal)
			hide ();
		else if (SearchGame._show_playing == false && state == StateList.Playing)
			hide ();
		else if (SearchGame._GameMode == 1 && Game == GameList.BackGammon)
			hide ();
		else if (SearchGame._GameMode == 2 && Game == GameList.Hokm)
			hide ();
		else
			show ();

			
	}
}
