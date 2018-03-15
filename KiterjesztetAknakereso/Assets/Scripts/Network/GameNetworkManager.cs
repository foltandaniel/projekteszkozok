using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
public class GameNetworkManager : NetworkManager {

    public static GameNetworkManager G_singleton;

   void Awake()
    {
        G_singleton = this;
    }
	public static void CreateGame()
    {
        singleton.StartHost();
    }
}
