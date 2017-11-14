using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour {

    public int num =1;
    private SpriteRenderer sr;
    public Sprite num1;
    public Sprite num2;
    public Sprite num3;
    public Sprite num4;
    public Sprite num5;
    public Sprite num6;
    public bool isMy;
    private Renderer rend;
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        sr = GetComponent<SpriteRenderer>();
        if (isMy== (Socket.SOC.GameBG.GameColor == Socket.SOC.GameBG.Nobat))
        {
            var Code = Socket.SOC.GameBG;
            var Dice = Code.DICE[num];
            bool Isreal = true;
            if (Dice < 0)
            {
                Isreal = false;
                Dice = Dice * -1;
            }

          
            sr.sortingOrder = 1;
            switch (Dice)
            {
                case 1:
                    sr.sprite = num1;
                    break;
                case 2:
                    sr.sprite = num2;
                    break;
                case 3:
                    sr.sprite = num3;
                    break;
                case 4:
                    sr.sprite = num4;
                    break;
                case 5:
                    sr.sprite = num5;
                    break;
                case 6:
                    sr.sprite = num6;
                    break;
                default:
                    sr.sortingOrder = -1;
                    break;
            }
            if (Isreal == false)
            {
                sr.material.color = Color.white;
                Color c = new Color();
                c = sr.material.color;
                c.a = 0.7f;
                sr.color = Color.yellow;
                sr.material.color = c;
            }
            else
            {
                sr.material.color = Color.white;
                sr.color = Color.white;
            }
        }
        else
        {
            sr.sortingOrder = -1;
        }
        
     
    }
}
