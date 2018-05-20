using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;
public class Player : NetworkBehaviour {
    private MultiPlayerGameManager manager;
    public static Player myPlayer;
    [SyncVar]
    public int PLAYER_ID;

    private void Start()
    {
        if (isLocalPlayer) //ez a MI playerünk
        {
            myPlayer = this;
        }
        if (isServer) StartCoroutine(WaitForLoad());
        //ha szerver vagyunk, várakozunk, hogy betöltődjön a scene, és létrejöjjön
        //a manager
    }
    IEnumerator WaitForLoad()
    {
        bool ok = false;
        while(!ok)
        {
            if(GlobalGameManager.singleton.actualGameManager) //van manager?
            {
                Initialize();
                ok = true;
            } else
            {
                yield return new WaitForSeconds(0.5f); //még várunk..
            }
        }
        
    }
    private MultiPlayerGameManager GetManager()
    {
        if (manager == null)
        {
            manager = (MultiPlayerGameManager)GlobalGameManager.singleton.actualGameManager;
            return (MultiPlayerGameManager)GlobalGameManager.singleton.actualGameManager;
        }else
        {
            return manager;
        }
    }
    public void Initialize() //SZERVEREN
    {
        /* 
         van manager..
         beállítjuk a playerid, hozzárakjuk magunkat a players listához
         küldünk egy OK -t a kliensnek (MINDEGYIK KLIENS MEGKAPJA)

         */
        PLAYER_ID = GetManager().players.Count;
        GetManager().players.Add(this);
        Console.Log("SERVER: Player"+PLAYER_ID+" initialized! sending trigger to client!");
        RpcReadyOnServer();
    }
    [ClientRpc]
    private void RpcReadyOnServer()
    {
        /* a szerveren ez a player már betöltődött.
        */
        if(isLocalPlayer) //ez a MI playerünk
        {
            Console.Log("Local player ready, sending message to server!");
            GameNetworkManager.singleton.client.Send(100, new EmptyMessage());
            //küldünk egy ok-t a szervernek
        }
        

    }
    [ClientRpc]
    public void RpcMyTurn()//e player köre
    {
        if(isLocalPlayer)
        {
            //ez a mi playerünk, tehát MI jövünk
            GetManager().pointText.text = "<color=green>Your turn!</color>";
        } else
        {
            //ez nem a mi playerünk, szóval NEM MI jövünk
            GetManager().pointText.text = "<color=red>Opponent's turn!</color>";
        }
       
    }

    public void Click(int x,int y) //local playerre hívódik meg(kliens)
    {
        Field clickedField = GetManager().field[x, y].fieldClass;
        if (clickedField.IsFlagged())
        {
            clickedField.FlagMe(); //ha flagelve van, akkor csak unflag, semmi más
            return;
        }
        if (GetManager().whosTurn == PLAYER_ID)
        {
            CmdClicked(x, y);
        }
    }
    [Command]
    private void CmdClicked(int x, int y) //SZERVEREN FUT LE
    {
        if(GetManager().whosTurn == PLAYER_ID) GetManager().Clicked(x, y);
    }
    [ClientRpc]
    public void RpcWin(int x, int y) {
        if(isLocalPlayer) GetManager().Victory(x,y);
    }
    [ClientRpc]
    public void RpcLost(int x, int y) {
        if (isLocalPlayer) {
            GetManager().Victory(x, y);
        } else {
            GetManager().Loose(x, y);
        }
    }


}
