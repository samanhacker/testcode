using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class _GameWinIndicator : MonoBehaviour {

	private int old_win;
	public bool isme=true;
	public Text MyLabel;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var win = 0;
		if (isme)win = HokmScript.p1win;
		else win = HokmScript.p2win;

		if (old_win != win) {
			old_win = win;
			MyLabel.text = win.ToString();
			var temp = "";
			for (int i = 0; i < win; i++) {
				temp += "BB-";
			}
			if (temp.Length > 0)
				temp = temp.Substring (0, temp.Length - 1);
			GetComponent<CardPack> ().handstr = temp;


		}
	}

}
