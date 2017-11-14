using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceSelector : MonoBehaviour {

	public int num=1;
	private int oldnum = 1;
	// Use this for initialization
	void Start () {
		GetComponent<Image>().sprite = Resources.Load<Sprite>("l1");
	}
	public void updateAvatar(float _num)
	{
		num =(int)_num;
		Register_code.Avatarnum = num;
	}
	// Update is called once per frame
	void Update () {

		if(oldnum!=num)
		{
			if (num >= 0 && num <= 63)
			{
				oldnum = num;
				GetComponent<Image>().sprite = Resources.Load<Sprite>("l" + num.ToString());
			}else
			{
				num = 0;
			}
		}
	}
}
