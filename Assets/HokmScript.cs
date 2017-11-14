using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class HokmScript : MonoBehaviour {


    public delegate void HokmEvent();
    public static event HokmEvent GameUpdate=delegate { };
	public CardPack p1hand;
	public CardPack p2hand;
	public CardPack p1s1;
	public CardPack p1s2;
	public CardPack p1s3;
	public CardPack p2s1;
	public CardPack p2s2;
	public CardPack p2s3;
	public CardPack deck;
	public CardPack HakemMe;
	public CardPack HakemOp;
    public  void PlayCardSound()
    {
        Debug.Log("SoundPlay");
        try
        {
            audioSource.PlayOneShot(CardAudio);
        }
        catch
        {
            Debug.Log("audio problem");
        }
    }

    public static int SelHokmPanel = 0;
	public static string Hokm;
	public static int Hakem;
	public static int p1win=0;
	public static int p2win=0;

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


    private static string _AvailableStr = "";

    public static string AvailableStr
    {
        get
        {
            return _AvailableStr;
        }
        set
        {
            if (value != "")
            {
                var packs = value.Split('-');
                AVCard = new List<string>(packs);
            }
            else
            {
                AVCard.Clear();
            }
            
        
            _AvailableStr = value;
            GameUpdate();
        }
    }
    public static List<string> AVCard = new List<string>();
	public static int Nobat=0;




	AudioSource audioSource;
	public AudioClip CardAudio;


	void OnEnable()  { 

		audioSource = GetComponent<AudioSource> ();
		Socket.SOC.PackeTRecived += InComing_Packet;
		Socket.SOC.writeSocket ("GetGame&" + GameBase.GameID);
	}
	void OnDisable() { 
		Socket.SOC.PackeTRecived -= InComing_Packet;	

	}
    // Use this for initialization
    void Start()
    {

	
    }

	internal void InComing_Packet(string Command)
	{
		
		var com = Command.Split ('&');

		switch (com [0]) {
		case "GetGame":
			sync_string = Command;
			AvailableStr = com [10];
			p1hand.handstr = com [1];
			p1s1.handstr = com [2];
			p1s2.handstr = com [3];
			p1s3.handstr = com [4];
			p2hand.handstr = com [5];
			p2s1.handstr = com [6];
			p2s2.handstr = com [7];
			p2s3.handstr = com [8];
			deck.handstr = com [9];
			Hokm = com [13];
			p1win = Convert.ToInt16 (com [14]);
			p2win = Convert.ToInt16 (com [15]);
			LoadGameString (com [16], "Timers");
			Hakem = Convert.ToInt16 (com [18]);
                GameBase.p1total = Convert.ToInt32(com[19]);
                GameBase.p2total = Convert.ToInt32(com[20]);
                SelHokmPanel = 0;
			HakemMe.handstr="";
			HakemOp.handstr="";
			break;
		case "SelHokm":   
			sync_string = Command;
			p1hand.handstr = com [1];
			p1s1.handstr = "";
			p1s2.handstr = "";
			p1s3.handstr = "";
			p2hand.handstr = com [2];
			p2s1.handstr = "";
			p2s2.handstr = "";
			p2s3.handstr = "";
			deck.handstr = "";
			HakemMe.handstr="";
			HakemOp.handstr="";
			var remain = Convert.ToInt16 (com [3]);
			var state = Convert.ToInt16 (com [4]);
			var htype = Convert.ToInt16 (com [5]);
			HokmScript.AvailableStr = "";

			if (state == 1) {				
				SelHokmPanel = 1+htype;
				GameBase.P1_TimeGreen = remain;
				GameBase.P1_TimeGreenMax = remain;
			}

			
			break;
		case "SelHakem":   
			sync_string = Command;
			p1hand.handstr = "";
			p1s1.handstr = "";
			p1s2.handstr = "";
			p1s3.handstr = "";
			p2hand.handstr = "";
			p2s1.handstr = "";
			p2s2.handstr = "";
			p2s3.handstr = "";
			deck.handstr = "";
			HakemMe.handstr = com [1];
			HakemOp.handstr = com [2];
			break;
		case "PreGame":   
			sync_string = Command;
			AvailableStr = "";
			p1hand.handstr = com [1];
			p1s1.handstr = com [2];
			p1s2.handstr = com [3];
			p1s3.handstr = com [4];
			p2hand.handstr = com [5];
			p2s1.handstr = com [6];
			p2s2.handstr = com [7];
			p2s3.handstr = com [8];
			deck.handstr = com [9];
			Hokm = com [10];
			p1win = Convert.ToInt16 (com [11]);
			p2win = Convert.ToInt16 (com [12]);
			Hakem = Convert.ToInt16 (com [13]);
			SelHokmPanel = 0;
			HakemMe.handstr="";
			HakemOp.handstr="";
			break;
		case "sync":
			if (com [1] == "problem") {
				var reload = "";
				for (int i = 4; i < com.Length; i++) {
					reload += com [i]+"&";
				}
				reload=reload.Substring (0, reload.Length - 1);
				Debug.Log ("Reload: " + reload);
				InComing_Packet (reload);

			}
			break;
		}
	}

	internal void LoadGameString(string inp_string,string type)
	{
		
		switch (type)
		{
		case "Timers":
			{
				var wait_str = inp_string.ToLower().Split('*');

				if (wait_str.Length == 4)
				{
					var gre = Convert.ToInt64(wait_str[0]);
					var gremax = Convert.ToInt64(wait_str[1]);
					var red = Convert.ToInt64(wait_str[2]);
					var red2 = Convert.ToInt64(wait_str[3]);

					if (AvailableStr!="")
					{
						GameBase.P2_TimeRed = 0;
						GameBase.P2_TimeRedMax = 0;
						GameBase.P2_TimeGreen = 0;
						GameBase.P2_TimeGreenMax = 0;

						GameBase.P1_TimeRed = red;
						GameBase.P1_TimeRedMax = red;
						GameBase.P1_TimeGreen = gre;
						GameBase.P1_TimeGreenMax = gremax;
					}
					else
					{
						GameBase.P1_TimeRed = 0;
						GameBase.P1_TimeRedMax = 0;
						GameBase.P1_TimeGreen = 0;
						GameBase.P1_TimeGreenMax = 0;

						GameBase.P2_TimeRed = red2;
						GameBase.P2_TimeRedMax = red2;
						GameBase.P2_TimeGreen = gre;
						GameBase.P2_TimeGreenMax = gremax;
					}
				}
			}
			break;
		}
	}
 
	void Update () {
		if (DateTime.Now.Subtract (sync_lastsend).TotalSeconds > 3) {
			sync_lastsend = DateTime.Now;
			Socket.SOC.sndCM (new string[]{"sync",sync_string });
		}

	}
 
	public void SndCommand(string command)
	{
		Socket.SOC.writeSocket (command);
	}

    
}
