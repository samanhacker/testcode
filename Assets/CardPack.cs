using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CardPack : MonoBehaviour
{
	public _CardCode.CardMode Mode;
    private string handstr_old = "";
    public string handstr = "";
    public GameObject card_sample;
    public float Rposition;
    public float EachCardDegree;
	public bool Invers=true;

    // Use this for initialization
    void Start()
    {
		this.gameObject.GetComponent<Image>().enabled = false;
     
    }

    // Update is called once per frame
    void Update()
    {
        if(handstr!=handstr_old)
        {
            handstr_old = handstr;
            hand_draw();
        }

    }

    public void hand_draw()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if (handstr.Length > 0)
        {
			var _cards =handstr.Split('-');
        
            int i = 1;
           
			var startAngel = 0 - (_cards.Length / 2) * EachCardDegree;
		
			if (Invers)Array.Reverse (_cards);

			foreach (string _card in _cards) {         
                RectTransform rt = (RectTransform)this.transform;
                var x = Instantiate(card_sample, this.gameObject.transform);
                x.transform.localScale = new Vector3(1, 1,1);
                var rect = this.GetComponent<RectTransform>().rect;
                x.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);

                var x_code = x.GetComponent<_CardCode>();
                x_code.Mode = Mode;
                x_code.Code = _card;  
               
                var r = this.gameObject.transform.position;
                x_code.Rotatepoint = new Vector2(r.x, r.y);
                
                x_code.distance = Rposition;
		
				switch (Mode) {
				case _CardCode.CardMode.Stack:
					{
						x_code.angel = 0 + i * (EachCardDegree);
					}
					break;
				default:
					{
						x_code.angel = startAngel + i * (EachCardDegree);
					}
					break;

				}


				x_code.Mode = this.Mode;
                               
               
                x_code.CalcPos();
                i++;
            }
        }
    }


    
}
