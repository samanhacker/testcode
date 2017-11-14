using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadLogin : MonoBehaviour {

    public GameObject _user;
    public GameObject _pass;
	public GameObject _email;
	public GameObject _forgotPanel;
    // Use this for initialization

	void OnEnable()  { 

		string username = PlayerPrefs.GetString("username", "");
		string password = PlayerPrefs.GetString("password", "");
        if(username.Length>1)
        {
            _user.GetComponent<InputField>().readOnly = true;
        }
		_user.GetComponent<InputField>().text = username;
		_pass.GetComponent<InputField>().text = password;
	}
	void OnDisable() { 
		
		Socket.SOC.PackeTRecived -= SOC_PackeTRecived;
	}

    void Start()
    {
      
		Socket.SOC.PackeTRecived += SOC_PackeTRecived;
		Application.targetFrameRate = 60;
		Socket.SOC.writeSocket("LoadLogin&");
        // Socket.SOC.PM(Socket.SOC.host + ":" + Socket.SOC.port);

    }

    private void SOC_PackeTRecived(string Value)
    {
        var com = Value.Split('&');
        if(com[0]=="sysmsg" && com[1]!="-1")
        {
            Socket.SOC.AdminMsg(com[1]);
        }
       
	}
	public void forgot()
	{
		
		Socket.SOC.writeSocket(                   
			string.Format("forgot&{0}&", _email.GetComponent<InputField>().text)
		);
		show_hide_forgot ();
	}
	public void show_hide_forgot()
	{
		_forgotPanel.SetActive (!_forgotPanel.active);
	}


    public void RegisterPage()
    {
        SceneManager.LoadScene("Register");
    }

   
	public void login()
    {
      
       
        Socket.SOC.writeSocket(                   
            string.Format("login&{0}&{1}&{2}&{3}&", _user.GetComponent<InputField>().text, _pass.GetComponent<InputField>().text, Socket.SOC.Game_Version, Socket.SOC.Game_Type)
            );
        PlayerPrefs.SetString("username", _user.GetComponent<InputField>().text);
        PlayerPrefs.SetString("password", _pass.GetComponent<InputField>().text);
        // GameObject LoginPanel = GameObject.Find("LoginPanel");
        //LoginPanel.SetActive(false);
    }
    // Update is called once per frame
    public void CloseApp()
    {
		Application.Quit ();
      //  var x = Instantiate(Socket.SOC.YesNoPanel);
        //var code = x.GetComponent<MSGBOX_CODE>();
       // code.Show("آیا مطمئن هستید؟", "خروج");
        //code.Yes_butt.onClick.AddListener(Application.Quit);    
    }
  
    void Update () {
        //if ((Input.deviceOrientation == DeviceOrientation.LandscapeLeft) && (Screen.orientation != ScreenOrientation.LandscapeLeft))
        //{
        //    Screen.orientation = ScreenOrientation.LandscapeLeft;
        //}

        //if ((Input.deviceOrientation == DeviceOrientation.LandscapeRight) && (Screen.orientation != ScreenOrientation.LandscapeRight))
        //{
        //    Screen.orientation = ScreenOrientation.LandscapeRight;
        //}
    }
}
