using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Net.Sockets;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Net;

public class Socket : MonoBehaviour
{
    public BackGammon GameBG;
    public GameObject PMPanel;
    public GameObject AdminMsgPanel;
    public GameObject YesNoPanel;
    public GameObject GameReadyPanel;
    public bool IsLocal = true;
    public string host = "185.8.172.142";
    public Int32 port = 85;
    public string Game_Version = "1.3.0";
    public string Game_Type = "desktop";
    public bool IsWaiting = false;
    public List<WaitNard> WaitList = new List<WaitNard>();
    internal Boolean socket_ready = false;
    internal String input_buffer = "";
    TcpClient tcp_socket;
    NetworkStream net_stream;
    StreamWriter socket_writer;
    StreamReader socket_reader;

    public string MyID = "";
    public string MyUsername = "";
    public string MyBalance = "";
    public int MyAvatar = 0;
 
    private string update_link = ""; 


    public delegate void CustomeEvent(string Value);
    public event CustomeEvent PackeTRecived;
    

    public static Socket SOC;
    public static void ChangePage(string PageName)
    {
        SceneManager.LoadScene(PageName);

    }
    private void lunch_update()
    {
        Application.OpenURL(update_link);
    }
    public void DoWork(string Command)
    {

        var com = Command.Split('&');
        Scene cur = SceneManager.GetActiveScene();
        switch (com[0])
        {
            case "dc":
                if (com[1] == "y")
                {
                    var val = Convert.ToInt64(com[2]);
                    GameBase.P2_TimeRed = val;
                    GameBase.P2_TimeRedMax = val;

                }
                if (com[1] == "n")
                {
                    var val = Convert.ToInt64(com[2]);
                    GameBase.P2_TimeRed = 0;
                    GameBase.P2_TimeRedMax = val;

                }
                break;
            case "isready":
                {
                    var x = Instantiate(GameReadyPanel);
                    var code = x.GetComponentInChildren<WaitPanelCode>();
                    code.RoundID = com[1];
                    GameBase.P2Avatar = Convert.ToInt16(com[3]);
                    code.Show(MyUsername, com[2], com[7], com[4], com[5], true);

                }
                break;
            case "waitready":
                {
                    var x = Instantiate(GameReadyPanel);
                    var code = x.GetComponentInChildren<WaitPanelCode>();
                    code.RoundID = com[1];
                    GameBase.P2Avatar = Convert.ToInt16(com[3]);
                    code.Show(MyUsername, com[2], com[7], com[4], com[5], false);

                }
                break;
            case "update":
                {
                    var x = Instantiate(Socket.SOC.YesNoPanel);
                    var code = x.GetComponent<MSGBOX_CODE>();
                    code.Show(com[1], "آپدیت", "خروج", "دانلود");
                    code.Yes_butt.onClick.AddListener(Application.Quit);
                    update_link = com[2];
                    code.No_butt.onClick.AddListener(lunch_update);
                }
                break;

            case "undo":
                GameBG.UndoMove();
                break;

            case "LoadGame_backgammon_preview":
                GameBase.IsGamePreview = true;
                GameBase.GameID = com[1];
                GameBase.GameType = com[2];
                GameBase.GameCount = com[3];
                GameBase.GamePrice = com[4];
                GameBase.GameTime = com[5];
                GameBase.p1total = Convert.ToInt16(com[6]);
                GameBase.p2total = Convert.ToInt16(com[7]);
                GameBase.P1Username = com[8];
                GameBase.P1Avatar = Convert.ToInt16(com[9]);
                GameBase.P2Username = com[10];
                GameBase.P2Avatar = Convert.ToInt16(com[11]);
                if (cur.name != "Table")
                {
                    SceneManager.LoadScene("Table");
                }
                break;
            case "LoadGame_hokm_preview":
                GameBase.IsGamePreview = true;
                GameBase.GameID = com[1];
                GameBase.GameType = com[2];
                GameBase.GameCount = com[3];
                GameBase.GamePrice = com[4];
                GameBase.GameTime = com[5];
                GameBase.p1total = Convert.ToInt16(com[6]);
                GameBase.p2total = Convert.ToInt16(com[7]);
                GameBase.P1Username = com[8];
                GameBase.P1Avatar = Convert.ToInt16(com[9]);
                GameBase.P2Username = com[10];
                GameBase.P2Avatar = Convert.ToInt16(com[11]);
                if (cur.name != "HokmPreview")
                {
                    SceneManager.LoadScene("HokmPreview");
                }
                break;
            case "LoadGame_backgammon":
                GameBase.IsGamePreview = false;
                GameBase.GameID = com[1];
                GameBase.GameType = com[2];
                GameBase.GameCount = com[3];
                GameBase.GamePrice = com[4];
                GameBase.GameTime = com[5];
                GameBase.p1total = Convert.ToInt16(com[6]);
                GameBase.p2total = Convert.ToInt16(com[7]);
                GameBase.P1Username = com[8];
                GameBase.P1Avatar = Convert.ToInt16(com[9]);
                GameBase.P2Username = com[10];
                GameBase.P2Avatar = Convert.ToInt16(com[11]);
                if (cur.name != "Table")
                {
                    SceneManager.LoadScene("Table");
                }
                break;
            case "LoadGame_hokm":
                GameBase.IsGamePreview = false;
                GameBase.GameID = com[1];
                GameBase.GameType = com[2];
                GameBase.GameCount = com[3];
                GameBase.GamePrice = com[4];
                GameBase.GameTime = com[5];
                GameBase.p1total = Convert.ToInt16(com[6]);
                GameBase.p2total = Convert.ToInt16(com[7]);
                GameBase.P1Username = com[8];
                GameBase.P1Avatar = Convert.ToInt16(com[9]);
                GameBase.P2Username = com[10];
                GameBase.P2Avatar = Convert.ToInt16(com[11]);
                if (cur.name != "Hokm")
                {
                    SceneManager.LoadScene("Hokm");
                }
                break;

            case "mainmenu":
                MyBalance = com[2];
                if (com[3] == "1")
                    IsWaiting = true;
                else
                    IsWaiting = false;

                if (cur.name != "MainMenu")
                    SceneManager.LoadScene("MainMenu");
                SOC.MyAvatar = Convert.ToInt16(com[6]);


                break;
            case "login":
                if (com[1] != "-1")
                {
                    MyUsername = com[2];
                    writeSocket("Status&");
                    SceneManager.LoadScene("MainMenu");
                }
                break;
            case "room":
                {
                    switch (com[1])
                    {
                        case "main":

                            if (cur.name != "MainMenu")
                            {
                                writeSocket("Status&");
                                SceneManager.LoadScene("MainMenu");
                            }
                            break;
                    }

                }
                break;
            case "PM":
                if (com.Length > 1 && com[2] != "")
                    PM(com[2]);
                else
                    PM(com[1]);
                break;
            case "PM2":
                if (com.Length > 1 && com[2] != "")
                    PM(com[2]);
                else
                    PM(com[1]);
                break;
        }
    }
    
    void Awake()
    {         
        MakeThisTheOnlyGameManager();
    }
    void MakeThisTheOnlyGameManager()
    {
        if (SOC == null)
        {
            DontDestroyOnLoad(gameObject);
            SOC = this;
            setupSocket();
		
            PackeTRecived += DoWork;
        }
        else
        {
            if (SOC != this)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
   
        string received_data = readSocket();
       
        string key_stroke = Input.inputString;

        // Collects keystrokes into a buffer
        if (key_stroke != "")
        {
            input_buffer += key_stroke;

            if (key_stroke == "\n")
            {
                // Send the buffer, clean it
                Debug.Log("Sending: " + input_buffer);
                writeSocket(input_buffer);
                input_buffer = "";
            }

        }


        if (received_data != "")
        {
            // Do something with the received data,
            //DoWork(received_data);
            Debug.Log(received_data);
            PackeTRecived(received_data);
            // print it in the log for now
 
        }
    }

	public static string GetHash(string input)
	{

		// step 1, calculate MD5 hash from input

		MD5 md5 = System.Security.Cryptography.MD5.Create();

		byte[] inputBytes = System.Text.Encoding.Unicode.GetBytes(input);

		byte[] hash = md5.ComputeHash(inputBytes);

		// step 2, convert byte array to hex string

		StringBuilder sb = new StringBuilder();

		for (int i = 0; i < hash.Length; i++)

		{

			sb.Append(hash[i].ToString("X2"));

		}

		return sb.ToString();

	}


    void OnApplicationQuit()
    {
        closeSocket();
    }



    public void setupSocket()
    {
        try
        {           
            if(IsLocal)tcp_socket = new TcpClient("127.0.0.1", port);
            else tcp_socket = new TcpClient(host, port);
            net_stream = tcp_socket.GetStream();
            socket_writer = new StreamWriter(net_stream, System.Text.Encoding.UTF8);
            socket_reader = new StreamReader(net_stream, System.Text.Encoding.UTF8);
            socket_ready = true;
        }
        catch (Exception e)
        {
    
            DC();
            Debug.Log("Socket error: " + e);
        }
    }

    public void DC()
    {
		socket_ready = false;
        Scene scene = SceneManager.GetActiveScene();
        if (scene.name != "LoginScene") SceneManager.LoadScene("LoginScene");
        var x = Instantiate(YesNoPanel);        
        var code = x.GetComponent<MSGBOX_CODE>();
  
        code.Show("اتصال به سرور ممکن نیست.", "خطا در اتصال","خروج","تلاش مجدد");
        code.Yes_butt.onClick.AddListener(Application.Quit);
        code.No_butt.onClick.AddListener(setupSocket);

       
    }
    public void AdminMsg(string input,string backlink="")
    {
        var x = Instantiate(AdminMsgPanel);
        var Xcode = x.GetComponentInChildren<AdminMsg>();
        Xcode.BackLink = backlink;
        Xcode.Show(input, "پیام سیستمی");
    }
   public void PM(string input)
    {
        var x=Instantiate(PMPanel);
        var Xcode = x.GetComponent<MSGBOX>();
        Xcode.Show(input, "پیام سیستمی");
    }

    public void writeSocket(string line)
    {  
		if (!socket_ready) {
			DC ();
			return;
		}
        try
        {
            line = line + "\r\n";
            socket_writer.Write(line);
            socket_writer.Flush();
        }
        catch
        {
            DC();
        }
    
    }
    public String readSocket()
    {
        if (!socket_ready)
            return "";

        if ((net_stream != null) && net_stream.DataAvailable)
            return socket_reader.ReadLine();

        return "";
    }
    public void closeSocket()
    {
        if (!socket_ready)
            return;

        socket_writer.Close();
        socket_reader.Close();
        tcp_socket.Close();
        socket_ready = false;
      
    }
    public void sndCM(string[] list)
    {
        string ret = "";
        foreach (var item in list)
        {
            ret += item + "&";
        }
        writeSocket(ret);
    }
   
    public class WaitNard
    {
        public long P1ID; 
        public DateTime CreateTime;
        public string roundID;
        public string p1_username; 
        public long p1_avatar;
        public string WinPrice;
        public string NardType;
        public string Time;
        public string NardCount;
        public WaitNard()
        {
            CreateTime = DateTime.Now;
        }
    }
}