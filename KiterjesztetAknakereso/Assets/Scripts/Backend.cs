using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;

public class ResultMSG
{
    public bool ok;
    public string msg;

    public ResultMSG(string rawResultFromServer)
    {
        try
        {
            string[] splitHalf = rawResultFromServer.Split('|');
            switch (splitHalf[0])
            {
                case "HIBA":
                    ok = false;
                    break;
                case "OK":
                    ok = true;
                    break;
            }

            msg = splitHalf[1];
        } catch(Exception)
        {
            ok = false;
            msg = "Unknown Error!";
        }
    }



}
public class Backend : MonoBehaviour {
    public static Backend singleton;
   
    public static string HOST_URL = "http://194.182.67.11/asp/";
    private static string SCOREBOARD_URL = HOST_URL + "GetScoretable.aspx";
    private static string LOGIN_URL = HOST_URL + "Login.aspx";




    public GameObject waiting;
    void Awake()
    {
       singleton = this;    
    }
   
    public static void ShowHideLoad(bool show)
    {
        singleton.waiting.SetActive(show);
    }
   
    public IEnumerator GetScoreboard(UnityAction<string[]> toReturn)
        /* paraméterben kapunk egy függvényt, amit megehívunk, ha letöltődött a scoreboard
         * ez azért kell, mert IEnumerator "aszinkron"-ban fut, ahol meghívjuk a függvény a következő sorban még NINCS meg
         * az eredmény */
    {
        yield return new WaitForSeconds(3f);
        Console.Log("Downloading scoreboard"); 
        WWW www = new WWW(SCOREBOARD_URL);
        yield return www;
        if(!string.IsNullOrEmpty(www.error))
        {
          //ERROR
        } else
        {
            string[] scores = www.text.Split('/');
            toReturn.Invoke(scores);
        }
    }

    public IEnumerator SendLoginRequest(string name,string pw,UnityAction<ResultMSG> toDoAfterResponse)
    {
        WWWForm form = new WWWForm();
        form.AddField("username", name);
        form.AddField("password", pw);

        WWW www = new WWW(LOGIN_URL, form);
        ShowHideLoad(true);
        yield return www;
        ShowHideLoad(false);

        ResultMSG msg;
        if (!string.IsNullOrEmpty( www.error))
        {
            msg = new ResultMSG("HIBA|"+www.error);
        }
        else
        {
            msg = new ResultMSG(www.text);
        }

        toDoAfterResponse.Invoke(msg);


    }





}
