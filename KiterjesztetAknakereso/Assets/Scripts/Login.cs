using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System.Security.Cryptography;

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
        SceneManager.activeSceneChanged += SceneChanged;
    }
    private void SceneChanged(Scene from, Scene to)
    {
        
        if(to.name == "Menu")
        {
            if (LOGGED_IN)
            {
                GameObject.Find("LoginButton").GetComponentInChildren<Text>().text = "Logout";
            }
        }
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
       
            if (!string.IsNullOrEmpty(token))
            {
                //TODO - lecsekkolni, hogy jó-e a token..
                LoggedIn();
                return;
            }

		StartCoroutine(Backend.singleton.SendLoginRequest(inputName.text,  MD5Hash(pw.text), LoginResult));
            //backend -nek elküldjük az adatokat, illetve megmondjuk, hogy a LoginResult(ResultMSG msg) -t hívja meg utána

      
    }
    public void Register()
    {
		Application.OpenURL ("http://194.182.67.11/asp/Register.aspx");
    }
    public void Offline()
    {
        form.SetActive(false);
    }
    public void LoggedIn()
    {
        LOGGED_IN = true;
        GameObject.Find("LoginButton").GetComponentInChildren<Text>().text = "Logout";
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
	public static void Logout() {
		singleton.StartCoroutine(singleton.LogoutCoroutine());
		Toast.Show ("Logged out!");
	}
	IEnumerator LogoutCoroutine() {
		WWWForm form = new WWWForm ();
		form.AddField ("token", token);
		WWW www = new WWW ("http://194.182.67.11/asp/Logout.aspx", form);
		yield return www;


		SaveTokenAndName ("", "");
		LOGGED_IN = false;
		GameObject.Find("LoginButton").GetComponentInChildren<Text>().text = "Login";

	}


	public static string MD5Hash(string input)
	{
		StringBuilder hash = new StringBuilder();
		MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
		byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

		for (int i = 0; i < bytes.Length; i++)
		{
			hash.Append(bytes[i].ToString("x2"));
		}
		return hash.ToString();
	}
}
