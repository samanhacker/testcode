using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BackGammon : MonoBehaviour {



    public Sprite targetBounds;
    public Transform R1;
    public Transform R6;
    public Transform R7;
    public Transform R13;
    public Transform R19;
    public Transform Out1;
    public Transform Out1t;
    public Transform Out2;
    public Transform Rend1;
    public Transform Rend1t;
    public Transform Rend2;
    public GameObject inp;
    public BeadCode.BeadColor GameColor;
    private BeadCode.BeadColor _Nobat=BeadCode.BeadColor.White;
    AudioSource audioSource;
    public AudioClip audio_Dice;
    public AudioClip audio_move;
    public AudioClip audio_nobat;
    public GameObject HighLight_obj;
	public GameObject DiceRollButt;
    public bool IsAndroidDevice = false;


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

    

    public BeadCode.BeadColor Nobat
    {
        get
        {
            return _Nobat;
        }
        set
        {
            if (value != _Nobat)
            {

            
                Undo.clear();
                _Nobat = value;
            }
        }
    }
    public int Selected_Row = 0;
	public void PlayRollDiceSound()
	{
		try {
			audioSource.PlayOneShot(audio_Dice);
		} catch {
			Debug.Log ("audio problem");
		}
	}
    public List<int> DICE = new List<int>(4);
    public enum TableState
    {
        StartGame, white_move, black_move, white_out, black_out, white_home, black_home, finish
    }
    List<GameObject> BeadList = new List<GameObject>();
    List<GameObject> BeadAvList = new List<GameObject>();
    List<BeadRows> BeadTemp = new List<BackGammon.BeadRows>();
    public Dictionary<int, BeadRow> GameRows = new Dictionary<int, BeadRow>();
    public Dictionary<BeadCode.BeadColor, BeadRow> WaitRow = new Dictionary<BeadCode.BeadColor, BeadRow>();
    public Dictionary<BeadCode.BeadColor, BeadRow> OutRow = new Dictionary<BeadCode.BeadColor, BeadRow>();
    class BeadRows
    {
        public int num;
        public BeadCode.BeadColor color;
        public int count;
        public BeadRows(int _num,int _count,BeadCode.BeadColor _color)
        {
            num = _num;
            color = _color;
            count = _count;
        }
    }
	void OnEnable()  {
        if(IsAndroidDevice==false)
        Camera.main.orthographicSize = 10.65f / Screen.width * Screen.height;

        audioSource = GetComponent<AudioSource>();	
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
		Socket.SOC.GameBG = this;
   


		// DICE_random();
		for (int i = 1; i < 25; i++)
		{

			GameRows[i] = new BeadRow(i, 0, BeadCode.BeadColor.White, inp);
		}
		OutRow[BeadCode.BeadColor.White]= new BeadRow(25, 0, BeadCode.BeadColor.White, inp);
		OutRow[BeadCode.BeadColor.Black] = new BeadRow(26, 0, BeadCode.BeadColor.Black, inp);
		WaitRow[BeadCode.BeadColor.White] = new BeadRow(25, 0, BeadCode.BeadColor.White, inp);
		WaitRow[BeadCode.BeadColor.Black] = new BeadRow(26, 0, BeadCode.BeadColor.Black, inp);

		//DICE_random();
		BeadTemp.Add(new BeadRows(1, 2, BeadCode.BeadColor.White));
		BeadTemp.Add(new BeadRows(6, 5, BeadCode.BeadColor.Black));
		BeadTemp.Add(new BeadRows(8, 3, BeadCode.BeadColor.Black));
		BeadTemp.Add(new BeadRows(12, 5, BeadCode.BeadColor.White));
		BeadTemp.Add(new BeadRows(13, 5, BeadCode.BeadColor.Black));
		BeadTemp.Add(new BeadRows(17, 3, BeadCode.BeadColor.White));
		BeadTemp.Add(new BeadRows(19, 5, BeadCode.BeadColor.White));
		BeadTemp.Add(new BeadRows(24, 2, BeadCode.BeadColor.Black));

		foreach (var item in BeadTemp)
		{
			GameRows[item.num] = new BeadRow(item.num, item.count, item.color, inp);
		}

		Socket.SOC.writeSocket ("GetGame&" + GameBase.GameID);
	}
	void OnDisable() { 
		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
		foreach (var d in BeadCode._list) {
			d.DestroyMe ();
		}
	
	}
    // Use this for initialization
    void Start () {
		

    }

	private void SOC_PackeTRecived(string Value)
	{
		var com = Value.Split('&');
		switch (com[0]) {
		case "MoveBead":
			{
				
				LoadGameString(com[1], "Move");
				LoadGameString(com[2], "Nobat");
				LoadGameString(com[3], "Dice");                
				LoadGameString(com[4], "OutBead");
				LoadGameString(com[5], "WaitBead");
				LoadGameString(com[7], "Timers");
				Generate_hash ();
				if (com[6] != GameString("Beads"))
				{
					Debug.Log("----"+com[6]);
					Debug.Log("----" + GameString("Beads"));
					LoadGame(com[6]);
				}
			}
			break;
		case "GetGame":
			{				
				LoadGameString(com[2], "Nobat");
				LoadGameString(com[3], "GameColor");
				LoadGameString(com[4], "Dice");
				LoadGameString(com[5], "OutBead");
				LoadGameString(com[6], "WaitBead");
				LoadGameString(com[7], "Timers");
				LoadGame(com[1]);
				Generate_hash ();
			}
			break;
           

        case "sync":
			if (com [1] == "problem") {
				var GameStr = com [4].Split('#');
				LoadGameString(GameStr[4], "Nobat");
				LoadGameString(GameStr[5], "GameColor");
				LoadGameString(GameStr[1], "Dice");
				LoadGameString(GameStr[2], "OutBead");
				LoadGameString(GameStr[3], "WaitBead");			
				LoadGame(GameStr[0]);
				Generate_hash();

//				Debug.Log (com [4]);
//				Debug.Log (_sync_string);
//				Socket.SOC.writeSocket ("GetGame&");
			}
			break;
		default:
			break;
		}
	}
	internal void Generate_hash()
	{
		var str = "";
		str += GameString("Beads");
		str += "#";
		str += GameString("Dice");
		str += "#";
		str += GameString("Out");
		str += "#";
		str += GameString("Wait");
		str += "#";
		str += GameString("Nobat");
		str += "#";
		str += GameString("GameColor");

		sync_string = str;
	}

	internal void LoadGameString(string inp_string,string type)
	{
		Debug.Log(type + "==>" + inp_string);
		switch (type)
		{
		case "Timers":
			{
				var wait_str = inp_string.ToLower().Split('*');
				Debug.Log( "count==>" + wait_str.Length);
				if (wait_str.Length == 4)
				{
					var gre = Convert.ToInt64(wait_str[0]);
					var gremax = Convert.ToInt64(wait_str[1]);
					var red = Convert.ToInt64(wait_str[2]);
					var red2 = Convert.ToInt64(wait_str[3]);

					if (Nobat == GameColor)
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
		case "OutBead":
			{
				if (inp_string != GameString("Out"))
				{
					var wait_str = inp_string.ToLower().Split('-');
					if (wait_str.Length == 2)
					{
						OutRow[BeadCode.BeadColor.Black].CleanRow();
						OutRow[BeadCode.BeadColor.White].CleanRow();
						foreach (var item in wait_str)
						{
							var lc = item[item.Length - 1];
							int num = Convert.ToInt16(item.Substring(0, item.Length - 1));

							if (lc == 'b')
								OutRow[BeadCode.BeadColor.Black] = new BackGammon.BeadRow(26, num, BeadCode.BeadColor.Black, inp);
							if (lc == 'w')
								OutRow[BeadCode.BeadColor.White] = new BackGammon.BeadRow(25, num, BeadCode.BeadColor.White, inp);
						}
					}
				}
			}
			break;
		case "WaitBead":
			{
				if (inp_string != GameString("Wait"))
				{
					var wait_str = inp_string.ToLower().Split('-');
					if (wait_str.Length == 2)
					{
						WaitRow[BeadCode.BeadColor.Black].CleanRow();
						WaitRow[BeadCode.BeadColor.White].CleanRow();
						foreach (var item in wait_str)
						{
							var lc = item[item.Length - 1];
							int num = Convert.ToInt16(item.Substring(0, item.Length - 1));

							if (lc == 'b')
								WaitRow[BeadCode.BeadColor.Black] = new BackGammon.BeadRow(-2, num, BeadCode.BeadColor.Black, inp);
							if (lc == 'w')
								WaitRow[BeadCode.BeadColor.White] = new BackGammon.BeadRow(-1, num, BeadCode.BeadColor.White, inp);
						}
					}
				}
			}     
			break;
		case "Nobat":
			Nobat = (inp_string == "b" ? BeadCode.BeadColor.Black : BeadCode.BeadColor.White);
			break;
		case "GameColor":
			GameColor = (inp_string == "b" ? BeadCode.BeadColor.Black : BeadCode.BeadColor.White);
			break;
		case "Dice":
                if (inp_string.Contains("*"))
                {
                    var iswaitfordice = DICE.Contains(9);

                    var ds = inp_string.Split('*');
                    for (int i = 0; i < ds.Length; i++)
                    {
                        DICE[i] = Convert.ToInt16(ds[i]);
                    }
                    for (int i = ds.Length; i < 4; i++)
                    {
                        DICE[i] = 0;
                    }

                    var isDiceDrop = DICE.Contains(9);
                    if(iswaitfordice==true && isDiceDrop==false)
                    {
                        PlayRollDiceSound();
                    }
                    
                    if(GameBase.IsGamePreview==true)
                    {
                        DiceRollButt.SetActive(false);
                    }
                    else
                    {
                        bool DiceRollVisible = (Nobat == GameColor);
                        for (int i = 0; i < 4; i++)
                        {
                            if (DICE[i] != 9)
                                DiceRollVisible = false;
                        }

                        DiceRollButt.SetActive(DiceRollVisible);
                    }

               

                }
			break;
		case "Move":
			if (inp_string != "")
			{
				
				var moves = inp_string.Split('#');

				foreach (var move in moves)
				{
					if (move != "")
					{
						var pack = move.Split('*');

						int f = Convert.ToInt16(pack[0]);//from
						int d = Convert.ToInt16(pack[1]);//dice
						moveBead(f, new List<int> { d },false);    
					}
				}
			}
			break;


		}
	}
    public TableState _TableState()
    {
        
        if (Nobat == BeadCode.BeadColor.Black)
        {
            if (WaitRow[BeadCode.BeadColor.Black].Beads.Count > 0) return TableState.black_out;
            else
            {
                for (int i = 24; i >6; i--)
                {
                    if (_GameRow(i).Beads.FindAll(x => x.GetComponent<BeadCode>().Color == BeadCode.BeadColor.Black).Count > 0)
                    {
                        return TableState.black_move;
                    }

                }
                return TableState.black_home;
            }
        }
        else
        {
            if (WaitRow[BeadCode.BeadColor.White].Beads.Count > 0) return TableState.white_out;
            else
            {
                for (int i = 1; i < 19; i++)
                {
                    if (_GameRow(i).Beads.FindAll(x => x.GetComponent<BeadCode>().Color == BeadCode.BeadColor.White).Count > 0)
                    {
                        return TableState.white_move;
                    }

                }
                return TableState.white_home;
            }
        
        }

    }
    public void LoadGame(string gameText)
    {
        Clean();
        for (int i = 1; i < 25; i++)
        {
            GameRows[i] = new BeadRow(i, 0, BeadCode.BeadColor.White, inp);
        }



        var Gstring = gameText;
        Gstring = Gstring.ToLower();
        var Beads_str = Gstring.Split('-');

        if (Beads_str.Length == 24)
        {
        
            int current_row = 0;
            foreach (var item in Beads_str)
            {
                current_row++;
                var lc = item[item.Length - 1];

                if (lc == 'e' || lc == 'b' || lc == 'w')
                {
                    int num = Convert.ToInt16(item.Substring(0, item.Length - 1));

                    if (lc == 'e')
                    {
                        GameRows[current_row] = new BeadRow(current_row, 0, BeadCode.BeadColor.Black, inp);
                    }
                    else
                    {

                        if (lc == 'b') GameRows[current_row] = new BeadRow(current_row, num, BeadCode.BeadColor.Black, inp);
                        else GameRows[current_row] = new BeadRow(current_row, num, BeadCode.BeadColor.White, inp);

                    }


                }
                else
                {

                    break;
                }
            }
        }
    }
	
	// Update is called once per frame
    public void DICE_random()
    {
        DICE[0] = UnityEngine.Random.Range(1, 6);
        DICE[1] = UnityEngine.Random.Range(1, 6);

        


        if (DICE[1]==DICE[0])
        {
            DICE[2] = DICE[1];
            DICE[3] = DICE[1];
        }
        else
        {
            DICE[2] = 0;
            DICE[3] = 0;

        }
    }
    public void DICE_remove(int _Dice)
    {
        var i=DICE.FindIndex(D => D == _Dice);
        DICE[i] = _Dice * 1;
    }



    public bool _CanMov(int Target)
    {
        if (Target == 0 || Target > 27 || Target < -3) return false;
            if (_GameRow(Target).BeadCount() < 2) return true;
            else if (_GameRow(Target).BeadColor() == Nobat) return true;
            else return false;
       
    }
    public void _Clone(GameObject _SeL, int targetrow, List<int> DiceNeed)
    {
        _highlight(targetrow,DiceNeed);
            //var SourceCode = _SeL.GetComponent<BeadCode>();
            //var x = Instantiate(_SeL, new Vector2(0, 0), Quaternion.identity);
            //var Code = x.GetComponent<BeadCode>();

        //Code.transform.position = RowPos(SourceCode.Row + 1, 1);
        //Code.isreal = false;
        //Code.DiceNeed =new List<int>(DiceNeed);
        //Code.Row = targetrow;
        //Code.Num = _GameRow(Code.Row).BeadCount();
        // Code.Target_pos = RowPos(item.num,i);
        //BeadAvList.Add(x);

    }

    public void _highlight(int targetrow, List<int> DiceNeed)
    {

        //var SourceCode = HighLight_obj.GetComponent<BeadCode>();
        var x = Instantiate(HighLight_obj, new Vector2(0, 0), Quaternion.identity);
        x.transform.rotation=Quaternion.Euler(0, 0, 180);
        var Code = x.GetComponent<highlight>();

        Code.DiceNeed = new List<int>(DiceNeed);
        Code.row = targetrow; 

        BeadAvList.Add(x);

    }
    public int Calc_Target(int pos,int distance)
    {
        int Targ = 0;
        var st = _TableState();
 
        switch(st)
        {
		case TableState.black_home:
			{
				Targ = pos - distance;
				Debug.Log ("Targ=" + Targ.ToString ());
				if (Targ == 0)
					return 26;
				else {
					if (Targ > 0)
						return Targ;
					else {
						var BeadsBefore = 0;
						for (int i = 6; i > pos; i--) {
							if (_GameRow (i).BeadColor () == BeadCode.BeadColor.Black)
								BeadsBefore += _GameRow (i).BeadCount ();
							
                             
						}
						Debug.Log ("BeadBefore=" + BeadsBefore.ToString ());
						if (BeadsBefore == 0)
							return 26;
						else
							return 0;
					}
				}
			}
		case TableState.white_home:
			{
				Targ = pos + distance;
				if (Targ == 25)
					return 25;
				else {
					if (Targ < 25)
						return Targ;
					else {
						var BeadsBefore = 0;
						for (int i = 19; i < pos; i++) {
							if (_GameRow (i).BeadColor () == BeadCode.BeadColor.White)
								BeadsBefore += _GameRow (i).BeadCount ();
						}					
						if (BeadsBefore == 0)
							return 25;
						else
							return 0;
					} 
				}
			}
              
            case TableState.black_out:
                Targ = 25 - distance;
                break;
            case TableState.white_out:
                Targ =distance;
                break;
            case TableState.black_move:
                Targ = pos - distance;
                if (Targ <1) return 0; //not possible
                break;
            case TableState.white_move:
                Targ = pos + distance;
                if (Targ >24) return 0;//not possible
                break;
        }
        return Targ;
    }
    public void BeadAvai(GameObject _SeL)
    {

   		
        var SourceCode = _SeL.GetComponent<BeadCode>();

		if (Selected_Row == SourceCode.Row) {
			Selected_Row = 0;
			BeadTemp.Clear();
			foreach (var item in BeadAvList) Destroy(item);
			return;
		} 

		if (DICE.Exists(item=>item==9)) {
			Selected_Row = 0;
			BeadTemp.Clear();
			foreach (var item in BeadAvList) Destroy(item);
			return;
		} 
	

        Selected_Row = SourceCode.Row;
        var targetrow = 0;
        BeadTemp.Clear();
        List<int> DNeed = new List<int>();
        foreach (var item in BeadAvList) Destroy(item);
        var _DiceAV = DICE.FindAll(item => item > 0);
        var st = _TableState();


        if (st == TableState.white_out)
        {
            Selected_Row = -1;
            foreach (var _d in _DiceAV)
            {
                DNeed.Clear();
                targetrow = _d;
                if (_CanMov(targetrow))
                {
                    DNeed.Add(_d);
                    _Clone(_SeL, targetrow, DNeed);
                }
            }
        }
        else if (st == TableState.black_out)
        {
            Selected_Row = -2;
            foreach (var _d in _DiceAV)
            {
                DNeed.Clear();
                targetrow = 25 - _d;
                if (_CanMov(targetrow))
                {
                    DNeed.Add(_d);
                    _Clone(_SeL, targetrow, DNeed);
                }
            }
        }else
        {
            switch (_DiceAV.Count)
            {
                case 4:
                    if (st == TableState.black_move || st == TableState.white_move)
                    {
                        for (int i = 0; i < 4; i++)
                        {

                            targetrow = Calc_Target(Selected_Row, _DiceAV[i] * (i + 1));
                            if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                            {
                                DNeed.Add(_DiceAV[i]);
                                _Clone(_SeL, targetrow, DNeed);
                            }

                            else break;
                        }
                    }else
                    {

                        targetrow = Calc_Target(Selected_Row, _DiceAV[0]);
                        if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                        {
                            DNeed.Add(_DiceAV[0]);
                            _Clone(_SeL, targetrow, DNeed);
                        }

                        else break;
                    }
                    break;
                case 3:
                    if (st == TableState.black_move || st == TableState.white_move)
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            targetrow = Calc_Target(Selected_Row, _DiceAV[i] * (i + 1));
                            if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                            {
                                DNeed.Add(_DiceAV[i]);
                                _Clone(_SeL, targetrow, DNeed);
                            }
                            else break;

                        }
                    }
                    else
                    {

                        targetrow = Calc_Target(Selected_Row, _DiceAV[0]);
                        if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                        {
                            DNeed.Add(_DiceAV[0]);
                            _Clone(_SeL, targetrow, DNeed);
                        }

                        else break;
                    }
                    
                    break;
                case 2:
        
                    targetrow = Calc_Target(Selected_Row, _DiceAV[0]);
                    if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                    {
                        _Clone(_SeL, targetrow, new List<int> { _DiceAV[0] });

                        if (st == TableState.black_move || st == TableState.white_move)
                        {
                            targetrow = Calc_Target(Selected_Row, _DiceAV[0] + _DiceAV[1]);
                            if (_CanMov(targetrow)) _Clone(_SeL, targetrow, new List<int> { _DiceAV[0], _DiceAV[1] });
                        }
                    }


                    targetrow = Calc_Target(Selected_Row, _DiceAV[1]);
                    if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                    {
                        _Clone(_SeL, targetrow, new List<int> { _DiceAV[1] });

                        if (st == TableState.black_move || st == TableState.white_move)
                        {
                            targetrow = Calc_Target(Selected_Row, _DiceAV[0] + _DiceAV[1]);
                            if (_CanMov(targetrow)) _Clone(_SeL, targetrow, new List<int> { _DiceAV[1], _DiceAV[0] });
                        }

                    }
                    break;
                case 1:

                    targetrow = Calc_Target(Selected_Row, _DiceAV[0]);
                    if (targetrow != 0)
                    {
                        Debug.Log("targetrow=" + targetrow.ToString());
                        if (_CanMov(targetrow) && FreezeCheck(Selected_Row, targetrow) == false)
                        {
                            _Clone(_SeL, targetrow, new List<int> { _DiceAV[0] });
                        }
                    }
                    break;

            }
        }


           





    }
	void Update () {
		if (DateTime.Now.Subtract (sync_lastsend).TotalSeconds > 3) {
			sync_lastsend = DateTime.Now;
			Socket.SOC.sndCM (new string[]{"sync",sync_string });
		}
		
	}
    public void Clean()
    {
     
        foreach (var item in GameRows)
        {
            foreach (var i in item.Value.Beads)
            {
                Destroy(i);
            }
          
        }
    }

    public Vector2 RowPos(int RowNum,int num)
    {
        int realrow = RowNum;
        if (GameColor==BeadCode.BeadColor.White)
        {
            if (RowNum == -1) RowNum = -2;
            else if (RowNum == -2) RowNum = -1;
            else if (RowNum == 25) RowNum = 26;
            else if (RowNum == 26) RowNum = 25;
            else RowNum = Math.Abs(25 - RowNum);
        }
        
        Vector2 ret = new Vector2(0, 0);
        var Scale = (R1.position.x - R6.position.x) / 5;
        float ScaleY = 0.78F;
        if (RowNum == 26) //white finish
        {
			var bo = OutRow[GameColor].BeadCount();
            if (bo > 4)
            {
                ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;
            }
            ret = Rend1.position;
			ret.y += num * Math.Abs(ScaleY);
        }
        else if (RowNum == 25) //black finish
        {
			var bo = OutRow[Opp()].BeadCount();
            if (bo > 4)
            {
                ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;
            }

            ret = Rend2.position;
			ret.y -= num * Math.Abs(ScaleY);
        }
        else if (RowNum == -1) //black wait
        {
			var bo = WaitRow[Opp()].BeadCount();
            if (bo > 4)
            {
                ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;
            }
            ret = Out2.position;
            ret.y -= num * ScaleY;
        }
        else if(RowNum == -2) //white wait
        {
			var bo = WaitRow[GameColor].BeadCount();
            if (bo > 4)
            {
                ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;
            }
            ret = Out1.position;
            ret.y += num * ScaleY;
        }
        else if (RowNum > 0 && RowNum <= 6)
        {
			var bo = GameRows[realrow].BeadCount();
			if (bo > 4)ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;
		
            ret = R1.position;
            ret.x -= Scale * (RowNum - 1);
            ret.y += num * ScaleY;
        }
        else if (RowNum > 6 && RowNum <= 12)
        {
			var bo = GameRows[realrow].BeadCount();
			if (bo > 4)ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;

            ret = R7.position;
            ret.x -= Scale * (RowNum - 7);
            ret.y += num * ScaleY;

        }
        else if (RowNum > 12 && RowNum <= 18)
        {
			var bo = GameRows[realrow].BeadCount();
			if (bo > 4)ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;

            ret = R13.position;
            ret.x += Scale * (RowNum - 13);
            ret.y -= num * ScaleY;
        }
        else if (RowNum >18 && RowNum <= 24)
        {
			var bo = GameRows[realrow].BeadCount();
			if (bo > 4)ScaleY = (Rend1t.position.y - Rend1.position.y) / bo;

            ret = R19.position;
            ret.x += Scale * (RowNum - 19);
            ret.y -= num * ScaleY;
        }
        else
        {

        }
        

       

        return ret;

    }
    public Vector2 RowPos_highlight(int RowNum)
    {
        if (GameColor == BeadCode.BeadColor.White)
        {
            if (RowNum == -1) RowNum = -2;
            else if (RowNum == -2) RowNum = -1;
            else if (RowNum == 25) RowNum = 26;
            else if (RowNum == 26) RowNum = 25;
            else RowNum = Math.Abs(25 - RowNum);
        }

        Vector2 ret = new Vector2(0, 0);
        var Scale = (R1.position.x - R6.position.x) / 5;
  
        if (RowNum == 26) //white finish
        {             
            ret = Rend1.position;
            ret.y = -3.184f;
        }
        else if (RowNum == 25) //black finish
        {
            ret = Rend2.position;
            ret.y = 3.15f;
        }
        else if (RowNum == -1) //black wait
        {
            ret = Out2.position;          
        }
        else if (RowNum == -2) //white wait
        {
            ret = Out1.position;           
        }
        else if (RowNum > 0 && RowNum <= 6)
        {
            ret = R1.position;
            ret.x -= Scale * (RowNum - 1);
            ret.y = -3.184f;


        }
        else if (RowNum > 6 && RowNum <= 12)
        {
            ret = R7.position;
            ret.x -= Scale * (RowNum - 7);
            ret.y = -3.184f;

        }
        else if (RowNum > 12 && RowNum <= 18)
        {
            ret = R13.position;
            ret.x += Scale * (RowNum - 13);
            ret.y = 3.15f;
        }
        else if (RowNum > 18 && RowNum <= 24)
        {
            ret = R19.position;
            ret.x += Scale * (RowNum - 19);
            ret.y = 3.15f;
        }
        else
        {

        }




        return ret;

    }
    public BeadCode.BeadColor Opp()
    {
        if (GameColor == BeadCode.BeadColor.White) return BeadCode.BeadColor.Black;
        else return BeadCode.BeadColor.White;
    }

    public void UndoMove()
    {
        if(Undo.Count()<=0)
        {
            Socket.SOC.PM("حرکتی برای برگشت وجود ندارد");
        }else
        {
            var fet = Undo.Fetch();
            transferBead(fet.to, fet.from);
            var ind = DICE.IndexOf(fet.Dice * -1);
            DICE[ind] = fet.Dice;

            if (fet.OutBead!=0)
            {              
                transferBead(fet.OutBead, fet.to);
            }
			Generate_hash ();
        }
    }
    public void transferBead(int from, int to)
    {
        Debug.Log(string.Format("transfer: {0} to {1}", from, to));
       
        var gr = _GameRow(from);
      
        var gr_count = _GameRow(from).Beads.Count - 1;
        var fromBead = gr.Beads[gr_count];
        var fromBeadCode = fromBead.GetComponent<BeadCode>();   
   
        fromBeadCode.Row = to;
        fromBeadCode.Num = _GameRow(to).Beads.Count;
        _GameRow(to).Beads.Add(fromBead);
        _GameRow(from).Beads.Remove(fromBead);
    }
    internal string GameString(string type)
    {
		switch (type) {  
		case "Beads":
			{
				string GS = "";
				for (int i = 1; i < GameRows.Count + 1; i++) {
					int bc = GameRows [i].Beads.Count;
					GS += bc;
					if (bc == 0)
						GS += "e";
					else if (GameRows [i].Beads [0].GetComponent<BeadCode> ().Color == BeadCode.BeadColor.Black)
						GS += "b";
					else
						GS += "w";

					GS += "-";

				}
				GS = GS.Substring (0, GS.Length - 1);

				return GS;
			}
		case "Dice":
			{
				string DC = "";
				if (DICE.Count > 0) {
					DC = DICE [0].ToString ();
					for (int i = 1; i < DICE.Count; i++) {
						if(DICE [i]!=0)DC += "*" + DICE [i].ToString ();
					}
				}
				return DC;
			}
		case "Out":
			return string.Format ("{0}b-{1}w", OutRow [BeadCode.BeadColor.Black].Beads.Count, OutRow [BeadCode.BeadColor.White].Beads.Count);

		case "Wait":
			return string.Format ("{0}b-{1}w", WaitRow [BeadCode.BeadColor.Black].Beads.Count, WaitRow [BeadCode.BeadColor.White].Beads.Count);
		case "Nobat":
			return (Nobat == BeadCode.BeadColor.White ? "w" : "b");
		case "GameColor":
			return (GameColor == BeadCode.BeadColor.White ? "w" : "b");

		default:
			return "";

		}



    }
    public bool FreezeCheck(int pos, int Targ)
    {
        if (_GameRow(Targ).BeadCount() >= 1 && _GameRow(Targ).BeadColor() == Nobat)
        {

            if (Nobat == BeadCode.BeadColor.Black && Targ >= 1 && Targ <= 6)
            {
                var res = Undo.CheckFreezeBead(pos);
                if (res >= 1 && res <= 6) return true;
            }
            if (Nobat == BeadCode.BeadColor.White && Targ >= 19 && Targ <= 24)
            {

                var res = Undo.CheckFreezeBead(pos);
                if (res >= 19 && res <= 24) return true;
            }

        }
        return false;
    }
    public void moveBead(int SelectedRow, List<int> _Dice, bool SendToServer = true)
    {

        audioSource.PlayOneShot(audio_move);
        var st = _TableState();
        var from = SelectedRow;
        var to = from;

        bool isFreeze = false;
     
  
        string moves = "";
        foreach (var Di in _Dice)
        {
            DICE_remove(Di);
            if (st == TableState.black_move || st == TableState.white_move)
            {
                from = SelectedRow;
                if (Nobat == BeadCode.BeadColor.Black) to = from - Di;
                else to = from + Di;
            }else if (st == TableState.black_out)
            {
                from = -2;
                to = 25 - Di;
            }
            else if (st == TableState.white_out)
            {
                from = -1;
                to = Di;
            }
            else if (st == TableState.white_home)
            {
                from = SelectedRow;
                to = from + Di;
                if (to > 24) to = 25;
            }
            else if (st == TableState.black_home)
            {
                from = SelectedRow;
                to = from- Di;
                if (to<=0) to = 26;
            }
          
         

            moves += string.Format("{0}*{1}#",from,Di);

            if (_CanMov(to))
            {
				
				
                if (_GameRow(to).BeadCount() == 1 && _GameRow(to).BeadColor() != Nobat)// zadan mohre
                {
                    if (Nobat == BeadCode.BeadColor.Black)
                    {
                        transferBead(to, -1);
                        transferBead(from, to);
                        new Undo(from, to, Di, -1);

                    }
                    else
                    {
                        transferBead(to, -2);
                        transferBead(from, to);
                        new Undo(from, to, Di, -2);
                    }
                  
                }
                else
                {
                    transferBead(from, to);
                    new Undo(from, to, Di);
                }
            }
            else
            {
                Socket.SOC.PM("حرکت غیر مجاز است");
            }


            SelectedRow = to;

        }
        foreach (var item in BeadAvList)
        {
            Destroy(item);
        }
        if (isFreeze == false)
        {
            Generate_hash();
            if (SendToServer) Socket.SOC.writeSocket(string.Format("Move&{0}&", moves));
        }


    }
    public class Undo
    {
        public int from;
        public int to;
        public int Dice;
        public int OutBead;
        public static int CheckFreezeBead(int row, int cycle = 0)
        {
            if (cycle > 4) return -1;

            if (!UndoList.Exists(item => item.OutBead != 0)) return -1;
            var FromItem = UndoList.Find(item => item.to == row);
            if (FromItem == null) return -1;
            else if (FromItem.OutBead != 0) return FromItem.to;
            else
                return CheckFreezeBead(FromItem.from, cycle + 1);


        }
        public static List<Undo> UndoList = new List<Undo>();
        public static int Count()
        {
            return UndoList.Count;
        }
        public static void clear()
        {
            UndoList.Clear();
        }
        public static Undo Fetch()
        {
            if (UndoList.Count > 0)
            {
                var Retundo = UndoList[UndoList.Count - 1];
                UndoList.Remove(Retundo);
                return Retundo;
            }
            return null;
        }
        public Undo(int from, int to, int Dice,int OutBead=0)
        {
            this.from = from;
            this.to = to;
            this.Dice = Dice;
            this.OutBead = OutBead;
            Debug.Log(string.Format("undo:{0}-{1}-{2}-{3}",from,to,Dice,OutBead));
            UndoList.Add(this);

        }
    }
    public class BeadRow
    {
       // public int rowNumber = -1;
        public List<GameObject> Beads = new List<GameObject>();
        public BeadRow(int RowNum,int Count, BeadCode.BeadColor Color,GameObject inp)
        {
          
            for (int i = 0; i < Count; i++)
            {
              
				var x = Instantiate(inp, Socket.SOC.GameBG.RowPos(RowNum, i), Quaternion.identity);

				x.tag="Bead";
                var Code = x.GetComponent<BeadCode>();
                Code.Row = RowNum;
                Code.Num = i;
                Code.Color = Color;
                // Code.Target_pos = RowPos(item.num,i);
                Beads.Add(x);
                
            }
        }
        public BeadCode.BeadColor BeadColor()
        {
            if (Beads.Count == 0) return BeadCode.BeadColor.Black;
            else
            {
                var c = Beads[0].GetComponent<BeadCode>();
                return c.Color;
            }

        }
        public int BeadCount()
        {
            if (Beads != null) return Beads.Count;
            else return 0;
        }
        public void CleanRow()
        {
            foreach (var item in Beads)
            {                
                    Destroy(item);
            }
        }
     
    }
    public BeadRow _GameRow(int RowNumber)
    {
        if (RowNumber == -1) return WaitRow[BeadCode.BeadColor.White];
        else if (RowNumber == -2) return WaitRow[BeadCode.BeadColor.Black];
        else if (RowNumber == 25) return OutRow[BeadCode.BeadColor.White];
        else if (RowNumber == 26) return OutRow[BeadCode.BeadColor.Black];
        else return GameRows[RowNumber];
    }



}
