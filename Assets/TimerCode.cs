using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerCode : MonoBehaviour {
    public Image RedC;
    public Image GreenC;
    public Text Num;
    private DateTime Set=new DateTime();
    public bool IsMy=true;
	// Use this for initialization
	void Start () {
        Set = DateTime.Now;
	}

    //public void set(int _Rval,int _Gval,int _Rmax,int _Gmax)
    //   {
    //       Rvalue = _Rval;
    //       Gvalue = _Gval;
    //       RMax = _Rmax;
    //       GMax = _Gmax;            
    //   }
    // Update is called once per frame
    private void Calc(ref float Gvalue, ref float GMax, ref float Rvalue, ref float RMax)
    {
        if(Gvalue>0 || Rvalue>0)
        {
            this.gameObject.GetComponent<Canvas>().enabled = true;
        }
        var def = (float)DateTime.Now.Subtract(Set).TotalMilliseconds;

        if (Gvalue > 0)
        {
            RedC.GetComponent<Image>().fillAmount = 1;
            GreenC.GetComponent<Image>().fillAmount = (Gvalue / GMax) - (def / (1000 * GMax));
        }
        else if (Rvalue > 0)
        {
            GreenC.GetComponent<Image>().fillAmount = 0;
            RedC.GetComponent<Image>().fillAmount = (Rvalue / RMax) - (def / (1000 * RMax)); ;
        }
        if (def > 1000)
        {
            if (Gvalue > 0)
            {
                Gvalue--;
                Num.text = Gvalue.ToString();
                Num.color = new Color32(1,71,0,255);
            }
            else if (Rvalue > 0)
            {
                Rvalue--;
                Num.text = Rvalue.ToString();
                Num.color = Color.red;
            }
            else
            {
                //DestroyObject(this.gameObject);
                //this.gameObject.SetActive(false);
                this.gameObject.GetComponent<Canvas>().enabled= false;
            }
            Set = DateTime.Now;
        }
    }
    void Update()
    {

        if (IsMy)
        {
            Calc(ref GameBase.P1_TimeGreen, ref GameBase.P1_TimeGreenMax, ref GameBase.P1_TimeRed, ref GameBase.P1_TimeRedMax);
        }else
        {
            Calc(ref GameBase.P2_TimeGreen, ref GameBase.P2_TimeGreenMax, ref GameBase.P2_TimeRed, ref GameBase.P2_TimeRedMax);
        }


        
    }
}
