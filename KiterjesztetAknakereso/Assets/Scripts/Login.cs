using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public static bool LOGGED_IN = false;
    public static string playerName = "Player";
    public static string token;


    public static Login singleton;
    public GameObject form;
    public InputField inputName, pw;
    void Awake()
    {
        singleton = this;
       
    }
    private void Start()
    {
        playerName = SystemInfo.deviceModel;
        token = PlayerPrefs.GetString("token");
        playerName = PlayerPrefs.GetString("name");
        CheckToken();
    }
    public void TryLogin()
    {
      if(!string.IsNullOrEmpty(token))
        {
            //TODO - lecsekkolni, hogy jó-e a token..
            LoggedIn();
            return;
        }
        StartCoroutine(Backend.singleton.SendLoginRequest(inputName.text,pw.text,LoginResult));
        //backend -nek elküldjük az adatokat, illetve megmondjuk, hogy a LoginResult(ResultMSG msg) -t hívja meg utána
        

    }
    public void Register()
    {
        Toast.Show("Not yet!");
    }
    public void Offline()
    {
        form.SetActive(false);
    }
    public void LoggedIn()
    {
        LOGGED_IN = true;
        GameObject.Find("LoginButton").SetActive(false);
        Toast.Show("Logged in!");
  
        Console.Log("playername: " + playerName);
        
        Hide();
    }
    public static void Show()
    {
        singleton.form.SetActive(true);
    }
    public static void Hide()
    {
        singleton.form.SetActive(false);
    }

    private void SaveTokenAndName(string _token,string _name)
    {
        token = _token;
        PlayerPrefs.SetString("token", token);
        PlayerPrefs.SetString("name", _name);
        playerName = _name;
        PlayerPrefs.Save();
    }

    public void CheckToken()
    {
        if(string.IsNullOrEmpty(token))
        {
            Show();
            return;
        } else
        {
           
            LoggedIn();
            //Van Tokenünk, csekkolni kéne, hogy jó-e..
            //ha nem, akkor megjeleníteni a formot, ha igen, akkor loggedin = true;
            //szerver adja vissza a nevünket, és állítsuk be.
           // Backend.CheckToken();
        }
    }


    private void LoginResult(ResultMSG result)
    {
       if(result.ok)
        {
            Console.Log("Logged in! token:" + result.msg);
            SaveTokenAndName(result.msg,inputName.text);
            LoggedIn();
            
        } else
        {
            Toast.Show(result.msg);
        }
    }
    
}
