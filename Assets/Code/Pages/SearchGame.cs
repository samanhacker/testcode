using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UPersian.Utils;
using System;
public class SearchGame : MonoBehaviour {



    public Transform GameList;
    public GameObject Item;
	public Slider min;
	public Slider max;
	public InputField min_inp;
	public InputField max_inp;
	public Dropdown GameMode;
	public Toggle Playing_show;

	public static double _MinVal=0;
	public static double _MaxVal=100000;
    public int counter = 0;
	public static int _GameMode=0;
	public static bool _show_playing=true;
	public static Toggle Playing_show2;

	private string _sync_string="";
	public string sync_string
	{
		get{
			return sync_hash;
		}
		set{
			_sync_string = value;
			sync_hash = Socket.GetHash (value);
			sync_lastsend = DateTime.Now;
		}
	}
	private string sync_hash = "";

	public bool sync_state = false;
	private DateTime sync_lastsend=DateTime.Now;


	void OnEnable()  { 
		
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
		sync_string = "";
		Refresh();
	}
	void OnDisable() { 	
		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;        
    }
	public void OnValueChanged_slider_min(float newValue)
	{
		var money= Fa.num_to_money(Convert.ToInt64(newValue));
		min_inp.text = money;
		_MinVal = min.value;

	}
	public void OnValueChanged_slider_max(float newValue)
	{
		var money= Fa.num_to_money(Convert.ToInt64(newValue));
		max_inp.text = money;
		_MaxVal = max.value;
	}
	public void GameModeChange()
	{
		_GameMode = GameMode.value;
	}
	public void ToggleChange_playing()
	{
		_show_playing = Playing_show.isOn;
	}

	public void OnValueChanged_inp_min(string newValue)
	{
		newValue = newValue.ToLower(); 
		var lastchar = newValue[newValue.Length - 1];
		try
		{
			switch (lastchar)
			{
			case 'k':
				var val = newValue.Substring(0, newValue.Length - 1);
				min.value = Convert.ToInt64(val)*1000;
				break;
			default:
				min.value = Convert.ToInt64(newValue) ;
				break;

			}	

		}
		catch
		{
			min.value = 1000;
		}
		_MinVal = min.value;
	}

	public void OnValueChanged_inp_max(string newValue)
	{
		newValue = newValue.ToLower(); 
		var lastchar = newValue[newValue.Length - 1];
		try
		{
			switch (lastchar)
			{
			case 'k':
				var val = newValue.Substring(0, newValue.Length - 1);
				max.value = Convert.ToInt64(val)*1000;
				break;
			default:
				max.value = Convert.ToInt64(newValue) ;
				break;

			}	

		}
		catch
		{
			min.value = 100000;
		}

		_MaxVal = max.value;
	}

    private void SOC_PackeTRecived(string Value)
    {
		Debug.Log ("Search Event" + Value);
        var com = Value.Split('&');
        if (com[0] == "addlist")
		{
			GameList.DetachChildren();
			sync_string = Value;
			
             if (com.Length > 1)
            {
                for (int i = 1; i < com.Length; i++)
                {
                    if (com[i] != "")
                    {
                        var Gitem = com[i].Split('#');
                        var GameCount = Gitem[1];
                        var Time = Gitem[2];
                        var GameType = Gitem[3];
                        var WinPrice = Gitem[4];
                        var ID = Gitem[5];
						var _p1username= Gitem [6];
						var _p1avatar = Gitem [7];
						if (_p1username != Socket.SOC.MyUsername) {
							var x = Instantiate (Item);
							var xcode = x.GetComponent<WaitListItem> ();
							xcode.ID = ID;
							xcode.price = Convert.ToInt64 (WinPrice);
							xcode.count = GameCount;
							xcode.type = GameType;
							xcode.time = Time;
							xcode.p1username = _p1username;
							xcode.p1avatar = _p1avatar;
						
							switch (Gitem [0]) {
							case "bp":
								
								xcode.state = WaitListItem.StateList.Playing;
								xcode.Game = WaitListItem.GameList.BackGammon;
								xcode.p2username = Gitem [8];
								xcode.p2avatar = Gitem [9];
								break;
							case "hp":
								xcode.state = WaitListItem.StateList.Playing;
								xcode.Game = WaitListItem.GameList.Hokm;
								xcode.p2username = Gitem [8];
								xcode.p2avatar = Gitem [9];
								break;
							case "bw":
								xcode.state = WaitListItem.StateList.Wait;
								xcode.Game = WaitListItem.GameList.BackGammon;
								break;
							case "hw":
								xcode.state = WaitListItem.StateList.Wait;
								xcode.Game = WaitListItem.GameList.Hokm;
								break;				
							}
							xcode.SetPara ();


                       
							x.transform.SetParent (GameList);
							x.transform.localScale = new Vector3 (1, 1);
                
						}

                    }
                }
            }
            else
            {

            }
         
        }

       
    }
	void Update () {
        if (counter > 60)
        {
            counter = 0;

            if (DateTime.Now.Subtract(sync_lastsend).TotalSeconds > 4)
            {
                sync_lastsend = DateTime.Now;
                Socket.SOC.sndCM(new string[] { "SearchGame", sync_string });
            }

        }
        counter++;

	}
    public void BackToMain()
    {
      
        SceneManager.LoadScene("MainMenu");
        Socket.SOC.writeSocket("SearchGameExit&");
    }
    public void Refresh()
    {
        GameList.DetachChildren();
        Socket.SOC.writeSocket("SearchGame&");

    }   
   
}
