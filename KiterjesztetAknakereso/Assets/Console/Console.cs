using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Console : MonoBehaviour {
    private static Console singleton;
    private Transform parent;
    public GameObject consoleElementPrefab;
    public GameObject console;
    public ScrollRect scroll;
    void Awake()
    {
        parent = GameObject.Find("ConsoleContent").transform;
        singleton = this;
        singleton.MSG("CONSOLE: singleton set");
        console.SetActive(false);
    }
    public static void Log(string msg)
    {
        singleton.MSG(msg);
    }
    public static void LogError(string msg)
    {
        singleton.MSG( "<color=red>"+msg+"</color>");
    }
    private void MSG(string msg)
    {
        Instantiate(consoleElementPrefab, parent).GetComponent<Text>().text = msg;
        Debug.Log(msg);

        Canvas.ForceUpdateCanvases();
        scroll.verticalScrollbar.value = 0.001f;
        Canvas.ForceUpdateCanvases();
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F1))
        {
            console.SetActive(!console.activeSelf);
            Canvas.ForceUpdateCanvases();
            scroll.verticalScrollbar.value = 0.001f;
            Canvas.ForceUpdateCanvases();
        }
    }
}
