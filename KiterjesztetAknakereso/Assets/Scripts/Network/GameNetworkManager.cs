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
        base.OnServerConnect(connection);
        // GlobalGameManager.singleton.RegisterHandlers();
      
        if (NetworkServer.connections.Count == 2)
        {
            //  Dialog.Hide();
            // Backend.ShowHideLoad(true);
            GameDiscovery.singleton.StopBroadcast();
            Console.Log("Client connected: " + connection.address);
            GlobalGameManager.singleton.actualGame = new Game("Multiplayer", 15, 45, GameMode.CUSTOM, true);
            Invoke("LoadGameScene", 3f);
        }
    }
    void LoadGameScene()
    {
        ServerChangeScene("MultiPlayerScene");
    }


    public override void OnClientDisconnect(NetworkConnection conn)
        //ha leléptünk a szervertől, akkor resetelünk mindent.
        //menüre váltunk, töröljük a game managert
    {
        base.OnClientDisconnect(conn);
        Toast.Show("Lost connection to the server!");
        GlobalGameManager.singleton.Reset();

    }

    public override void OnServerDisconnect(NetworkConnection conn)
    {
        //ha lecsatlakozik 1 kliens, akkor leállítjuk a szervert.
        base.OnServerDisconnect(conn);
        StopServer();
        StopClient();
        GlobalGameManager.singleton.Reset();
    }
    //Detect when a client connects to the Server
    public override void OnClientConnect(NetworkConnection connection)
    {
        base.OnClientConnect(connection);
        if (!NetworkServer.active)
        {
            // GlobalGameManager.singleton.RegisterHandlers();
            Backend.ShowHideLoad(true);
            Console.Log("Connected to " + connection.address);
            GlobalGameManager.singleton.actualGame = new Game("Multiplayer", 15, 45, GameMode.CUSTOM, true);

            Console.Log("Waiting for server ...");
           // SceneManager.LoadScene("MultiPlayerScene");
        }

    }

}
