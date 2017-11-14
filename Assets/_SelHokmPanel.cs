using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _SelHokmPanel : MonoBehaviour {
	public GameObject Panel;
	public GameObject NarasPanel;
	// Use this for initialization
	void Start () {
		
	}
	public void HideHokmSelect()
	{
		Panel.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (HokmScript.SelHokmPanel >0) {
			Panel.SetActive (true);
			NarasPanel.SetActive (HokmScript.SelHokmPanel == 2);			
		} else {
			Panel.SetActive (false);
		}
	}
}
