using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class _HokmIndicate : MonoBehaviour {
	public Text HokmLabel;
	public Sprite Pik;
	public Sprite del;
	public Sprite khesht;
	public Sprite ghish;
	public Sprite naras;
	public Sprite saras;
	public Sprite unknow;

	private string old_hokm;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		var H = HokmScript.Hokm;
		if (old_hokm !=  H) {
			old_hokm =  H;
			switch (H) {
			case "c":
				GetComponent<Image> ().sprite = ghish;
				HokmLabel.text = "گیشنیز";
				break;
			case "d":
				GetComponent<Image> ().sprite = khesht;
				HokmLabel.text = "خشت";
				break;
			case "h":
				GetComponent<Image> ().sprite = del;
				HokmLabel.text = "دل";
				break;
			case "s":
				GetComponent<Image> ().sprite = Pik;
				HokmLabel.text = "پیک";
				break;
			case "naras":
				GetComponent<Image> ().sprite = naras;
				HokmLabel.text = "نرس";
				break;
			case "saras":
				GetComponent<Image> ().sprite = saras;
				HokmLabel.text = "سرس";
				break;

			default:
				GetComponent<Image> ().sprite = unknow;
				HokmLabel.text = "نامشخص";
				break;
			}

		}
	}
}
