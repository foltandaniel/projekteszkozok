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
    public string name;
    public int n, m;
    public int mines;
    public GameMode mode;
    public bool multiplayer;
    //konstruktor
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


    public GameObject fieldPrefab;





    public static GameManager singleton;
    public static Game regular = new Game("Regular", 15, 15, 10, GameMode.REGULAR, false);
    public Game actualGame;


    public Text timeText;
    Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    int time;
    void Awake()
    {
        singleton = this;
        SceneManager.activeSceneChanged += SceneChanged;

       
    }

    private void Start()
    {
        StartGame(); //DEBUG
    }
    private void SceneChanged(Scene from, Scene to)
    {
        Console.Log("SCENE CHANGE: "+ from.name + "->" + to.name);
        if(to.name == "Game")
        {
            timeText = References.singleton.timeText;
            StartGame();
        }
    }
    public static void StartRegular()
    {
        singleton.actualGame = regular;

        SceneManager.LoadScene("Game");
    }









void StartGame() //játék indítása
    {
        counter = StartCoroutine(Counter());
        actualGame = regular;
        SetupGrid();
    }
	IEnumerator Counter() //számláló
    {
      Console.Log("Timer started");
        time = 0;
     
        while(true)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
            time++;
            yield return new WaitForSeconds(1f); //másodpercenként menjen a ciklus
        }
    }
    
    

    void SetupGrid()
    {
        Transform parent = GameObject.Find("GRID").transform;
        for(int i = 0; i < actualGame.m;i++)
        {
            for(int j = 0; j < actualGame.n;j++)
            {
                Instantiate(fieldPrefab, new Vector3(i+0.5f, j+0.5f, 0), Quaternion.identity, parent);
            }
        }

        parent.position = new Vector3(-actualGame.n / 2, -actualGame.n / 2, 0);
        CameraControl.singleton.AlignCamera(actualGame.n);
    }

   
}
