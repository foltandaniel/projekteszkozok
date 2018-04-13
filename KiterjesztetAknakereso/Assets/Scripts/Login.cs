using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Login : MonoBehaviour {
    public static bool LOGGED_IN = false;
    public static string playerName = "Player";
    public static Login singleton;
    public GameObject form;
    public InputField inputName, pw;
    void Awake()
    {
        singleton = this;
        playerName = SystemInfo.deviceModel;
    }

    public void TryLogin()
    {
        Toast.ShowToast("Not yet!");
    }
    public void Register()
    {
        Toast.ShowToast("Not yet!");
    }
    public void Offline()
    {
        form.SetActive(false);
    }
    public void LoggedIn()
    {
        GameObject.Find("LoginButton").SetActive(false);
    }
    public static void Show()
    {
        singleton.form.SetActive(true);
    }
    
}
