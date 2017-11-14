using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeadCode : MonoBehaviour {
    public BeadColor Color;
    public Sprite SPBlack;
    public Sprite SPWhite;
    public int Row; 
    public int Num;
    private Renderer rend;
    public Vector2 Target_pos=new Vector2(0,0);
    public SpriteRenderer sr;
    public bool isreal = true;
    public List<int> DiceNeed = new List<int>();
    public enum BeadColor { White,Black };
    public enum BeadState { Real,Fade};

	public static List<BeadCode> _list = new List<BeadCode> ();
	void Awake()
	{
		_list.Add (this);
	}

	void Start () {
        rend = GetComponent<Renderer>();
        sr = GetComponent<SpriteRenderer>();        
        if (Color == BeadColor.Black) sr.sprite = SPBlack;
        else sr.sprite = SPWhite; 
    }
    // Update is called once per frame
    void OnMouseEnter()
    {
        //sr.color = Color.red;    
    }
    void OnMouseDown()
    {
        //GameObject Camera = GameObject.Find("GameCamera");
        // BackGammon BGCode = Camera.GetComponent<BackGammon>();
		if (isreal == true && GameBase.IsGamePreview==false)
        {
//			if (Socket.SOC.GameBG.Selected_Row == Row) {
//				Socket.SOC.GameBG.Selected_Row = 0;
//			}else
				if (
				Socket.SOC.GameBG.Nobat == Socket.SOC.GameBG.GameColor &&
                Socket.SOC.GameBG.GameColor == Color &&
                Row != 25 && Row != 26)
                Socket.SOC.GameBG.BeadAvai(this.gameObject);
            else Socket.SOC.GameBG.Selected_Row = 0;

        }
//        else
//        {
//            if (Socket.SOC.GameBG.Selected_Row != 0)
//                Socket.SOC.GameBG.moveBead(Socket.SOC.GameBG.Selected_Row, DiceNeed, true);
//
//            Socket.SOC.GameBG.Selected_Row = 0;
//
//        }
    }
    
    
    void OnMouseExit()
    {
        // rend.material.color = Color.white;
        //sr.color = Color.white;
        // rend.material.color = Color.white;
    }

    
    // Update is called once per frame
    void Update () {
		sr.sortingOrder = Num+1;
        //GameObject Camera = GameObject.Find("GameCamera");
       // BackGammon BGCode = Camera.GetComponent<BackGammon>();
        if (!isreal)
        {
            rend.material.color = UnityEngine.Color.white;
            Color c = new Color();
            c = rend.material.color;
            c.a = 0.7f;
            sr.color = UnityEngine.Color.white;
	
            rend.material.color= c;
        }
        else
        {

            if (Socket.SOC.GameBG.Selected_Row == Row) rend.material.color = new Color32(0, 255, 0, 255);
            else rend.material.color = UnityEngine.Color.white;
        }

     
        Target_pos = Socket.SOC.GameBG.RowPos(Row, Num);
        transform.position = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), Target_pos, 10 * Time.deltaTime);
    }
    public void DestroyMe()
    {        

       Destroy(gameObject);
    }
	void OnDestroy()
	{
		_list.Remove (this);
	}

}
