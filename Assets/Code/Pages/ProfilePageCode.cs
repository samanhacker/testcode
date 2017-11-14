using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProfilePageCode : MonoBehaviour {

    public InputField username;
    public InputField Name;
    public InputField Email;
    public InputField Mobile;
    public InputField shabaID;
    public InputField BankCardID;
    public InputField BankName;
    public InputField oldpass;
    public InputField newpass1;
    public InputField newpass2;

	void OnEnable()  { 
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
		Socket.SOC.sndCM(new string[] { "Profile","-1","-1" });
	}
	void OnDisable() { 

		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}

    // Use this for initialization
 
    private void SOC_PackeTRecived(string Value)
    {
        var com = Value.Split('&');
        if(com[0]=="Profile")
        {
            if (com[1] == "saved") Socket.SOC.PM("ذخیره شد");
            else if(com[1]== "Invalid password") Socket.SOC.PM("رمز عبور معتبر نیست");
            else
            {
                username.text = com[2];
                Name.text = com[3];
                Email.text = com[4];
                Mobile.text = com[5];
                shabaID.text = com[8];
                BankCardID.text = com[9];
                BankName.text = com[10];

                username.readOnly = (com[2] != "" ? true : false);
                Name.readOnly = (com[3] != "" ? true : false);
                Email.readOnly = (com[4] != "" ? true : false);
                Mobile.readOnly = (com[5] != "" ? true : false);
                shabaID.readOnly = (com[8] != "" ? true : false);
                BankCardID.readOnly = (com[9] != "" ? true : false);
                BankName.readOnly = (com[10] != "" ? true : false);



            }

        }
    }

    // Update is called once per frame
    public void ChangePage(string PageName)
    {
        SceneManager.LoadScene(PageName);

    }
    public void saveinfo()
    {
        Socket.SOC.sndCM(new string[] { "Profile", "1", "1", "", "", Name.text, shabaID.text, BankCardID.text, BankName.text });
    }
    public void changepassword()
    {
        if(newpass1.text != newpass2.text)
        {
            Socket.SOC.PM("رمز عبور یکسان نیست");
            return;
        }
        if(newpass1.text=="" )
        {
            Socket.SOC.PM("رمز جدید را وارد نمایید.");
            return;
        }
        if( newpass1.text.Length<8)
        {
            Socket.SOC.PM("رمز عبور جدید حداقل باید ۸ کاراکتر باشد.");
            return;
        }
        
            Socket.SOC.sndCM(new string[] { "Profile", "1", "1",oldpass.text,newpass1.text });            
        
    }
    void Update () {
		
	}
}
