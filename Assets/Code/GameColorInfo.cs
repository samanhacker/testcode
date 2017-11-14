using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameColorInfo : MonoBehaviour {
    private Image sr;
    public bool IsMy;
    public bool IsNobat = false;
    public Sprite Color_Black;
    public Sprite Color_White;
    // Use this for initialization
    void Start () {
        sr = GetComponent<Image>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Socket.SOC.GameBG!=null)
        {
       
            if (IsNobat)
                sr.sprite = (Socket.SOC.GameBG.Nobat == BeadCode.BeadColor.White ? Color_White : Color_Black);            
            else
            {         
                if (Socket.SOC.GameBG.GameColor == BeadCode.BeadColor.Black)
                    sr.sprite = (IsMy == false ? Color_White : Color_Black);                
                else sr.sprite = (IsMy == true ? Color_White : Color_Black);
            } 
        }
    }
}
