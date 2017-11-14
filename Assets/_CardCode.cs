using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class _CardCode : MonoBehaviour,IEndDragHandler,IDragHandler,IBeginDragHandler,IPointerDownHandler,IPointerUpHandler
{

	public enum CardMode {P1hand,P2hand,Deck,Stack,PS}
	public CardMode Mode;
    public Vector2 Rotatepoint;
    public float angel;
    public float distance = 0;
    public string _Code = "BB";
    private Vector2 _old_pos;
    private Quaternion _old_rot;
    private int _old_sibling;
    public bool IsAv = false;
	public GameObject DeckBack;
 

    public string Code
    {
        set
        {

            _Code = value;
            this.gameObject.name = _Code;
            if(Mode!=CardMode.P1hand)
                GetComponent<Image>().sprite = Resources.Load<Sprite>("CardsLow/" + _Code);
            else GetComponent<Image>().sprite = Resources.Load<Sprite>("Cards/"+_Code);
            if(HokmScript.AVCard.Contains(_Code))
            {
                IsAv = true;
                GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }
            else
            {
                IsAv = false;
                if (_Code != "BB")
                    GetComponent<Image>().color = new Color32(212, 212, 212, 255);
                else
                    GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            }

        }
        get
        {
            return _Code;
        }
    }
    
    


	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
        if (IsAv && Mode != CardMode.PS)
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pz.z = 0;
            transform.position = pz;
          
            
        }
    }

	#endregion

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
        if (IsAv && Mode != CardMode.PS)
        {
			_DeckBackGorund.State = true;
            _old_pos = transform.position;
            _old_rot = transform.rotation;
//            _old_sibling = this.gameObject.GetComponent<RectTransform>().GetSiblingIndex();
            this.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
    }

	#endregion

	#region IEndDragHandler implementation
	public void OnEndDrag (PointerEventData eventData)
	{
        if (IsAv && Mode!=CardMode.PS)
        {
			_DeckBackGorund.State = false;
			if(_DeckBackGorund.IsOver)
			{
                FindObjectOfType<HokmScript>().PlayCardSound();
                Socket.SOC.sndCM(new string[] { "Sel",Code });
           

            }
			transform.position = _old_pos;
			transform.rotation = _old_rot;
			this.gameObject.GetComponent<RectTransform>().SetSiblingIndex(_old_sibling);
         
           
        }
        //CalcPos();
    }
	#endregion

	#region IPointerUpHandler implementation

	public void OnPointerUp (PointerEventData eventData)
	{
        //		transform.position = _old_pos;
        //		transform.rotation = _old_rot;
   
        if (Code != "BB" && Mode == CardMode.PS && IsAv)
        {
            Socket.SOC.sndCM(new string[] { "Sel", Code });
            FindObjectOfType<HokmScript>().PlayCardSound();
        }
        else
        {
            this.gameObject.GetComponent<RectTransform>().SetSiblingIndex(_old_sibling);
        }
    }

    #endregion

    public void OnPointerDown(PointerEventData eventData)
    {
        //		_old_pos = transform.position;
        //		_old_rot = transform.rotation;
        _old_sibling = this.gameObject.GetComponent<RectTransform>().GetSiblingIndex();
        this.gameObject.GetComponent<RectTransform>().SetAsLastSibling();
        //		transform.rotation = new Quaternion(0, 0, 0, 0);
    }

    void Start()
    {

        HokmScript.GameUpdate += HokmScript_GameUpdate;


    }

    private void HokmScript_GameUpdate()
    {
        if (HokmScript.AVCard.Contains(Code))
        {
            IsAv = true;

            GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
        else
        {
            IsAv = false;
            if (Code != "BB")
                GetComponent<Image>().color = new Color32(212, 212, 212, 255);
            else
                GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        }
    }



	public void Log(string inp)
	{
		Debug.Log (inp);
	}

    // Update is called once per frame
    void Update()
    {        

    }
    public Vector2 CalcCircle(Vector3 center, float radius ,float ang)
    {
        //float ang = Random.value * 360;
        Vector2 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);        
        return pos;
    }
    public void CalcPos()
    {

        // Vector2 center = PosStart;
        if (distance == 0)
        {

        }
        else
        {
            Vector2 pos = CalcCircle(new Vector2(0, 0), distance, angel);
			switch (Mode)
			{
			case CardMode.Stack:
				{
					Quaternion rot = Quaternion.FromToRotation (Vector2.left, pos);
					transform.rotation = rot;
				}
				break;
			default:
				{
					Quaternion rot = Quaternion.FromToRotation (Vector2.up, pos);
					transform.rotation = rot;
				}
				break;

			}
         
            transform.position = new Vector2(pos.x + Rotatepoint.x, pos.y + Rotatepoint.y - distance);
           
        }



    }
    ~_CardCode()
    {
        HokmScript.GameUpdate -= HokmScript_GameUpdate;
    }
    private void OnDestroy()
    {
        HokmScript.GameUpdate -= HokmScript_GameUpdate;
    }


}
