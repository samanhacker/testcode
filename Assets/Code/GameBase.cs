using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBase : MonoBehaviour {


	public static bool IsGamePreview = false;

	public static string RoundID = "";
	public static string GameID ="";
	public static string GameCount = "";
	public static string GameType = "";
	public static string GamePrice = "";
	public static string GameTime = "";

	public static int P1Avatar = 0;
	public static string P1ID = "";
	public static string P1Username = "";

	public static int P2Avatar = 0;
	public static string P2ID = "";
	public static string P2Username = "";

	//time Variable

	public static float P1_TimeRed = 0;
	public static float P1_TimeRedMax = 20;
	public static float P1_TimeGreen = 0;
	public static float P1_TimeGreenMax = 20;

	public static float P2_TimeRed = 0;
	public static float P2_TimeRedMax = 20;
	public static float P2_TimeGreen = 0;
	public static float P2_TimeGreenMax = 20;

	public static int p1total = 0;
	public static int p2total = 0;

	public string MyName="";
    public static string OnlineCount ="----";

	void Update () {
        switch (MyName)
        {
            case "OnlineCount":
                GetComponent<UPersian.Components.RtlText>().text = "آنلاین: " + OnlineCount;
                break;
            case "p1total":
                GetComponent<UPersian.Components.RtlText>().text = p1total.ToString();
                break;
            case "p2total":
                GetComponent<UPersian.Components.RtlText>().text = p2total.ToString();
                break;
            case "GameCount":
                GetComponent<UPersian.Components.RtlText>().text = GameCount;
                break;
            case "GameType":
                GetComponent<UPersian.Components.RtlText>().text = GameType;
                break;
            case "GamePrice":
                GetComponent<UPersian.Components.RtlText>().text =Fa.num_to_money(Convert.ToInt64(GamePrice));
                break;
            case "GameTime":
                GetComponent<UPersian.Components.RtlText>().text = GameTime;
                break;
            case "username1":
                GetComponent<UPersian.Components.RtlText>().text = P1Username;
                break;
            case "username2":
                GetComponent<UPersian.Components.RtlText>().text = P2Username;
                break;
            case "avatar1":
                GetComponent<AvatarSelector>().num = P1Avatar;
                break;
            case "avatar2":
                GetComponent<AvatarSelector>().num = P2Avatar;
                break;

        }


	}

}
