using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BackGammonStart : MonoBehaviour {


	public GameObject _MyDice;
	public GameObject _OpDice;
	public GameObject MyColor;
	public GameObject OpColor;
	public GameObject CenterText;


	public DateTime CreateTime=DateTime.Now;


	public void Show(int MyDice,int OppDice,bool IsmeWhite)
	{
		CreateTime = DateTime.Now;
		this.gameObject.SetActive (true);

		_MyDice.GetComponent<DiceSelector> ().num = MyDice;
		_OpDice.GetComponent<DiceSelector> ().num = OppDice;
		if (IsmeWhite) {
			Socket.SOC.GameBG.GameColor = BeadCode.BeadColor.White;
			CenterText.GetComponent<UPersian.Components.RtlText> ().text = "نوبت شماست";

		} else {
			Socket.SOC.GameBG.GameColor = BeadCode.BeadColor.Black;
			CenterText.GetComponent<UPersian.Components.RtlText> ().text = "نوبت حریف";
		}

	}

		
	// Update is called once per frame
	void Update () {
		if (DateTime.Now.Subtract (CreateTime).TotalSeconds > 3) {
			this.gameObject.SetActive (false);
		}
		
	}
}
