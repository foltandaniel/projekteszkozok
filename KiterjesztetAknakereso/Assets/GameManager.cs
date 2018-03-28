using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameMode
{
    CUSTOM,REGULAR
}
public struct Game {
    string name;
    int n, m;
    int mines;
    GameMode mode;
    bool multiplayer;

    public Game(string name, int n, int m, int mines, GameMode mode, bool multiplayer)
    {
        this.name = name;
        this.n = n;
        this.m = m;
        this.mines = mines;
        this.mode = mode;
        this.multiplayer = multiplayer;
    }
}
public class GameManager : MonoBehaviour {
    public static GameManager singleton;
    public static Game regular = new Game("Regular", 10, 10, 10, GameMode.REGULAR, false);
    public Game actualGame;


    public Text timeText;
    Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    int time;
    void Awake()
    {
        singleton = this;
        SceneManager.activeSceneChanged += SceneChanged;
    }
    private void SceneChanged(Scene from, Scene to)
    {
        Console.Log("SCENE CHANGE: "+ from.name + "->" + to.name);
        if(to.name == "Game")
        {
            timeText = References.singleton.timeText;
        }
    }










void StartGame() //játék indítása
    {
        counter = StartCoroutine(Counter());
    }
	IEnumerator Counter() //számláló
    {
      //  Console.Log("Timer started");
        time = 0;
     
        while(true)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
            time++;
            yield return new WaitForSeconds(1f); //másodpercenként menjen a ciklus
        }
    }
    
    
}
