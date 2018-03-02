using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    public Text timeText;
    Coroutine counter;
    int time;
    void Awake()
    {
        singleton = this;   
    }
    // Use this for initialization
    void Start () {
        StartGame();
	}
    void StartGame()
    {
        counter = StartCoroutine(Counter());
    }
	IEnumerator Counter()
    {
      //  Console.Log("Timer started");
        time = 0;
     
        while(true)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
            time++;
            yield return new WaitForSeconds(1f);
        }
    }
}
