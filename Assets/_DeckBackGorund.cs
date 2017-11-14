using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class _DeckBackGorund : MonoBehaviour {
	public static bool State = false;
	public static bool IsOver=false;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {

	
		if (State) {
			if (IsOver) {
				GetComponent<Image> ().color = new Color32 (0, 0, 0, 100);
			} else {
				GetComponent<Image> ().color = new Color32 (255, 53, 39, 100);
			}
		} else {
			GetComponent<Image> ().color = new Color32 (255, 53, 39, 0);
		}
	}

	void OnMouseEnter()
	{
		IsOver = true;
	}
	void OnMouseExit()
	{
		IsOver = false;
	}
}
