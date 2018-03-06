using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class Backend : MonoBehaviour {
    public static Backend singleton;
    #region debug
    public InputField dest;
    public static string HOST_URL = "localhost";
    private static string SCOREBOARD_URL = HOST_URL + "scoreboard";
    public GameObject hostDest;
    #endregion


    [SerializeField]
    private GameObject waiting;
    void Awake()
    {
       singleton = this;    
    }
    void Start()
    {
        dest.text = PlayerPrefs.GetString("HostDest", "localhost");
    }
    public static void ShowHideLoad(bool show)
    {
        singleton.waiting.SetActive(show);
    }
    public void SetHostDestination()
    {
        Console.Log("DESTINATION SET TO " + dest.text);
        PlayerPrefs.SetString("HostDest", dest.text);
        PlayerPrefs.Save();
        HOST_URL = dest.text;
        Destroy(hostDest);
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
            Console.LogError("Error downloading scoreboard:" + www.error);
            toReturn.Invoke(new string[] {"Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6",
            "Zetya|6"});
        } else
        {
            string[] scores = www.text.Split('/');
            toReturn.Invoke(scores);
        }
    }

}
