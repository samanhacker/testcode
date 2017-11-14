using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class Create_Game : MonoBehaviour {
    public enum GamesList{hokm,backgammmon}
    // Use this for initialization
    public GamesList SelGame=GamesList.backgammmon;
    public Dropdown GameCount;
    public Dropdown GameType;
    public Dropdown GameTime;
	long Step1 = 3000;
	long Step2 = 10000;
    long Step3 = 50000;



    public InputField GameMoney;
    public Slider Sl;
	void Start () {
        if(SelGame==GamesList.backgammmon)
        {
            Step1 = 5000;
            Step2 = 50000;
            Step3 = 1000000;
        }
        GameCount.value = 0;
        GameType.value = 0;
        GameTime.value = 1;
    }
    public void OnValueChanged(float newValue)
    {
        var money= Fa.num_to_money(Convert.ToInt64(newValue));
        GameMoney.text = money;

        if (Sl.value>Step2 && Sl.value <Step3 && GameCount.value == 3) GameCount.value = 2;
        if (Sl.value>Step1 && Sl.value<Step2 && GameCount.value==2)GameCount.value=1;
		if(Sl.value<Step1 && GameCount.value>0)GameCount.value=0;
    }
	public void OnValueChanged_GameCount()
	{

		var money = (Sl.value - (Sl.value % 1000));
        switch (GameCount.value)
        {
            case 0:
                break;
            case 1:
                if (money < Step1)
                    Sl.value = Step1;
                break;
            case 2:
                if (money < Step2)
                    Sl.value = Step2;
                break;
            case 3:
                if (money < Step3)
                    Sl.value = Step3;
                break;
        }

	}
    public void OnValueChanged2(string newValue)
    {
        newValue = newValue.ToLower(); 
        var lastchar = newValue[newValue.Length - 1];
        try
        {
            switch (lastchar)
            {
                case 'k':
                    var val = newValue.Substring(0, newValue.Length - 1);
                    Sl.value = Convert.ToInt64(val)*1000;
                    break;
                default:
                    Sl.value = Convert.ToInt64(newValue) ;
                    break;

            }
			if(Sl.value<Step1 && GameCount.value<1)GameCount.value=0;
			else if(Sl.value<Step2 && GameCount.value<2)GameCount.value=1;
            else if (Sl.value < Step3 && GameCount.value < 3) GameCount.value = 2;
        }
        catch
        {
            Sl.value = 1000;
        }
   

    }

    // Update is called once per frame
    void Update () {
   

    }
    public void Show()
    {
        gameObject.SetActive(true);
    }
    public void CreateGame(string GameName="BackGammon")
    {
        if (GameName == "Hokm")
        {
            Socket.SOC.sndCM(new string[] {
                "NewGame&h",
            GameTime.value.ToString(),
            GameCount.value.ToString(),
            GameType.value.ToString(),     
            (Sl.value-(Sl.value%1000)).ToString()
            });
        }
        else
        {
            Socket.SOC.sndCM(new string[] {
                "NewGame&b",
			GameTime.value.ToString(),
            GameCount.value.ToString(),
            GameType.value.ToString(), 
            (Sl.value-(Sl.value%1000)).ToString()
            });
        }
    }
    public void BackToMain()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
