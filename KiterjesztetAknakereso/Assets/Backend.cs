using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Backend : MonoBehaviour {

    #region debug
    public InputField dest;
    public static string HOST_URL = "localhost";
    public GameObject hostDest;
    #endregion


    private void Start()
    {
        dest.text = PlayerPrefs.GetString("HostDest", "localhost");
    }
    public void SetHostDestination()
    {
        Console.Log("DESTINATION SET TO " + dest.text);
        PlayerPrefs.SetString("HostDest", dest.text);
        PlayerPrefs.Save();
        HOST_URL = dest.text;
        Destroy(hostDest);
    }
}
