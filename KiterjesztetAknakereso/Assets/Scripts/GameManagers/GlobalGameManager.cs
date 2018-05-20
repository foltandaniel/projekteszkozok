using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;

public enum GameMode
{
    CUSTOM,REGULAR
}
public enum GameStatus
{
    MY_TURN,OPPONENT_TURN,ENDED,NOT_STARTED
}

public class GlobalGameManager : MonoBehaviour {

    public GameObject multiplayerManagerPrefab;
    public GameObject singleplayerManagerPrefab;
  



    public static GlobalGameManager singleton;
    public static Game regular = new Game("Regular", 15, 45, GameMode.REGULAR, false);
    public Game actualGame;
    public SinglePlayerGameManager actualGameManager;
    public GameObject fieldPrefab;

    public Texture[] textures;
    public static bool canMove;
    /* 0 - akna
     * 1 - 0
     * 2 - 1
     * stb...
    */
    void Awake()
    {
    
		singleton = this;
        SceneManager.activeSceneChanged += SceneChanged;

       
    }

   
    void Start()
    {
        // actualGame = regular;
        // StartLocalGame(); //DEBUG
        // this should be somewhere else..
       
    }
    private void SceneChanged(Scene from, Scene to)
    {
        Console.Log("SCENE CHANGE: "+ from.name + "->" + to.name);
		if (to.name == "SinglePlayerScene") {
            GameObject.Instantiate(singleplayerManagerPrefab);

		} else if(to.name == "MultiPlayerScene")
        {
            if (actualGame.multiplayer)
            {

                if (NetworkServer.active)
                { // mi vagyunk a szerver..
                    GameObject toInstantiate = GameObject.Instantiate(multiplayerManagerPrefab);
                    NetworkServer.Spawn(toInstantiate);
                }
           
            }
        }


        else
        {
            Destroy(gameObject.GetComponent<SinglePlayerGameManager>());
        }
    }

   
    public static void StartRegular()
    {
		Backend.ShowHideLoad(true);
        singleton.actualGame = regular;
		
        SceneManager.LoadScene("SinglePlayerScene");
    }

	public static void StartCustom(Game game) {
		Backend.ShowHideLoad(true);
		singleton.actualGame = game;
		SceneManager.LoadScene ("SinglePlayerScene");

	}
    public void Reset()
    {
        Destroy(actualGameManager.gameObject);
        SceneManager.LoadScene("Menu");
    }


}
