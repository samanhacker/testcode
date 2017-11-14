using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AvatarPic : MonoBehaviour {
    public bool IsMyAvatar = true;
    private int oldnum = 0;
    // Use this for initialization
    void Start () {
      
        GetComponent<Image>().sprite = Resources.Load<Sprite>("avatar_sp_0");
    }
	
	// Update is called once per frame
	void Update () {
		var num = (IsMyAvatar == true ? Socket.SOC.MyAvatar : GameBase.P2Avatar);
        if(oldnum!=num)
        {
            if (num >= 0 && num <= 63)
            {
                oldnum = num;
                GetComponent<Image>().sprite = Resources.Load<Sprite>("avatar_sp_" + num.ToString());
            }else
            {
                if(IsMyAvatar == true ) Socket.SOC.MyAvatar = oldnum;
				else GameBase.P2Avatar = oldnum;
            }
        }
    }
}
