using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
 
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
        broadcastData =Login.playerName;
       
        ui = GameObject.FindObjectOfType<MultiplayerUI>();
        NetworkTransport.Init();
    }
	public void StartBroadcasting()
    {
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
            Toast.ShowToast("Found games on network!");
        }
        ui.AddToScrollList(fromAddress, data);

    }

}
