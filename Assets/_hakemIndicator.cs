using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class _hakemIndicator : MonoBehaviour {
	public bool IsMe=true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (HokmScript.Hakem!=0) {
			if (HokmScript.Hakem==1) {
				GetComponent<Image> ().enabled = IsMe;
			} else {
				GetComponent<Image> ().enabled = !IsMe;
			}
		} else {
			GetComponent<Image> ().enabled = false;
		}
	}
}
