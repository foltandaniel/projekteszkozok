using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameNetworkManager : NetworkManager {

    public static GameNetworkManager singleton;

   void Awake()
    {
        singleton = this;
    }
    	public static void CreateGame()
    {
        singleton.StartHost();
    }


    public void TryToConnect(string ip)
    {
        networkAddress = ip;
        StartClient();
    }

    public override void OnServerConnect(NetworkConnection connection)
    {
        GameManager.singleton.RegisterHandlers();
        if (NetworkServer.connections.Count == 2)
        {
          //  Dialog.Hide();
           // Backend.ShowHideLoad(true);
            Console.Log("Client connected: " + connection.address);
            GameManager.singleton.actualGame = new Game("Multiplayer", 15, 45, GameMode.CUSTOM, true);
            SceneManager.LoadScene("MultiPlayerScene");
        }
    }

    //Detect when a client connects to the Server
    public override void OnClientConnect(NetworkConnection connection)
    {
        GameManager.singleton.RegisterHandlers();
        Backend.ShowHideLoad(true);
        Console.Log("Connected to " + connection.address);
        GameManager.singleton.actualGame = new Game("Multiplayer", 15, 45, GameMode.CUSTOM, true);

        Console.Log("Waiting for server to load..");

    }

}
