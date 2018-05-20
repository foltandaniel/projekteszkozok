using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class GameDiscovery : NetworkDiscovery {
    public static GameDiscovery singleton;

    public MultiplayerUI ui;

    bool initialized = false;
    bool found = false;
	// Use this for initialization
	void Awake()
    {
        singleton = this;
        //Console.Log("Initialized network");

      
    }
    void Start()
    {
        broadcastData = Login.playerName;

        ui = GameObject.FindObjectOfType<MultiplayerUI>();
        NetworkTransport.Init();
        SceneManager.activeSceneChanged += SceneManager_activeSceneChanged;
       
    }

    private void SceneManager_activeSceneChanged(Scene arg0, Scene arg1)
    {
        if(arg1.name == "Menu")
        {
            broadcastData = Login.playerName;

            ui = GameObject.FindObjectOfType<MultiplayerUI>();
            NetworkTransport.Init();
        }
    }

    public void StartBroadcasting()
    {
        broadcastData = Login.playerName;
        Console.Log("Network: Started broadcasting");
        StopBroadcast();
        Initialize();
        StartAsServer();
    }
    public void Stop()
    {
        found = false;
        Console.Log("Network: Shutting down");
        initialized = false;
        StopBroadcast();
        NetworkTransport.Shutdown();
        
    }
    public void StartListening()
    {
       if(!initialized)
        {
            Console.Log("Network: Initialized");
            initialized = true;
            NetworkTransport.Init();
            Initialize();
        }
        Console.Log("Network: Started listening");
        StartAsClient();
    }

    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        if(!found)
        {
            found = true;
            Toast.Show("Found games on network!");
        }
        ui.AddToScrollList(fromAddress, data);

    }

}
