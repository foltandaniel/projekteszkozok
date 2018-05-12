using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;


public class MultiPlayerGameManager : SinglePlayerGameManager
{
    private int readyCount = 0;
    private bool serverReady;

    [SyncVar]
    public int whosTurn;
    
    public List<Player> players;

    protected override void SetupGameMap()
    {
       
        if (isServer)
        {

            NetworkServer.RegisterHandler(100, ReadyMessageFromClient); //ready message
            //ez alapján látjuk hány client áll készen
            GenerateMines(actualGame.mines);
            FillMatrix();
            SetupGrid();
            AddToReady();
            whosTurn = 0;
        }
        else
        {
                  }
    }
    void ReadyMessageFromClient(NetworkMessage msg)
    {
        AddToReady();
    }


    public void AddToReady()
    {
        readyCount++;
        Console.Log("SERVER: ready: " + readyCount);
        if(readyCount == 3)
        {
            Vector2[] array = minePositions.ToArray();
           RpcSendMap(array);
            Dialog.Hide();

            players[whosTurn].RpcMyTurn();

        }
    }
    [ClientRpc]
   void RpcSendMap(Vector2[] mines)
    {
        if (NetworkServer.active) return;
        //kliensek vagyunk, de van aktív szerver. ez azt jelenti, hogy host vagyunk
        Console.Log("Map received!");
        minePositions = new List<Vector2>( mines);
        FillMatrix();
        SetupGrid();
        Dialog.Hide();

    }

    public override void Clicked(int x, int y) //SZERVER
    {
        //jött egy klikk a jelenlegi playertől.
        RpcClicked(x, y); //elküldjük a klienseknek

        SwitchPlayers();
    }

    private void SwitchPlayers() //SZERVER
    {
        if (whosTurn == 0)
        {
            players[1].RpcMyTurn();
            whosTurn = 1;
        }
        else
        {
            players[0].RpcMyTurn();
            whosTurn = 0;
        }
    }

    IEnumerator CountDown()
    {
        int time = 30;
        while(time >0)
        {
            timeText.text = "00:" + time.ToString("00");
            time--;
            yield return new WaitForSeconds(1f);
        }
    }
    private void RestartCounter()
    {
        if(counter != null) StopCoroutine(counter);
        counter = StartCoroutine(CountDown());
    }
    [ClientRpc]
    private void RpcClicked(int x, int y)
    {
        RestartCounter();
        Field clickedField = field[x, y].fieldClass;


        clickedField.TurnMe();

        int whatIsIt = field[x, y].value;


        if (whatIsIt == 0)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    CheckIfZero(x + i, y + j);
                }
            }
        }

        IsEnd(x, y, whatIsIt);
    }
}


