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

public struct Game {
    public string name;
    public int n;
    public int mines;
    public GameMode mode;
    public bool multiplayer;
    //konstruktor
    public Game(string name, int n, int mines, GameMode mode, bool multiplayer)
    {
        this.name = name;
        this.n = n;
        this.mines = mines;
        this.mode = mode;
        this.multiplayer = multiplayer;


    }  
	public override string ToString() {
		return mode.ToString();
	}
}

public struct FieldStruct
{
    public int value;
    public Field fieldClass;
    public bool flooded; //volt-e már rajta a Flood? (endgame)
}


public static class MsgTypes
{
    public const int SERVER_LOADED = 0;
}
public class GameManager : MonoBehaviour {
	
	public static bool PLAYING;
	private bool firstClick;
    public static GameManager singleton;
    public static Game regular = new Game("Regular", 15, 45, GameMode.REGULAR, false);
    public Game actualGame;

    public GameObject fieldPrefab;
    public Text timeText;
	public Text pointText;
	private int point;
	private int multiplier=0;
		

    public Texture[] textures;
    /* 0 - akna
     * 1 - 0
     * 2 - 1
     * stb...
    */


    private FieldStruct[,] field;
    //map
    //akna -1
    //számok 0-8
    private int flaggedCount; //mennyi mező van flagelve?
    //private Dictionary<Vector2, Field> fieldMap = new Dictionary<Vector2, Field>();
	private List<Vector2> minePositions;

    private int remainingNotMineFields;
   
    

   

   
    Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    private WaitForSeconds floodWait = new WaitForSeconds(0.17f);
    int time;
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
		if (to.name == "Game") {
            PLAYING = false;
            StopAllCoroutines ();
            Dialog.Hide();
			timeText = References.singleton.timeText;
			pointText = References.singleton.pointText;
            if (actualGame.multiplayer) {

                if (NetworkServer.active){ // mi vagyunk a szerver..
                   
                    StartMultiplayerGameAsHost();
                }
            }
            else
            {
                StartLocalGame();
            }
			
		}
    }
    public static void StartRegular()
    {
		Backend.ShowHideLoad(true);
        singleton.actualGame = regular;
		Debug.Log ("actual game set");
        SceneManager.LoadScene("Game");
    }

	public static void StartCustom(Game game) {
		Backend.ShowHideLoad(true);
		singleton.actualGame = game;
		SceneManager.LoadScene ("Game");

	}



void StartLocalGame() //játék indítása
    {
        flaggedCount = 0;
		firstClick = true;
		Console.Log("game mode: " + actualGame);


        References.singleton.mines.text = actualGame.mines.ToString();

		field = new FieldStruct[actualGame.n, actualGame.n];
		GenerateMines (actualGame.mines);
		FillMatrix ();

       
       // actualGame = regular;
        SetupGrid();

		References.singleton.StartTip.SetActive (true);



        remainingNotMineFields = actualGame.n * actualGame.n - actualGame.mines;
		PLAYING = true;
        Backend.ShowHideLoad(false);

    }
   
    IEnumerator Counter() //számláló
    {
      Console.Log("Timer started");
        time = 0;
     
        while(true)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
			multiplier = 6-(Math.Min((time / 60),5));
            time++;
            yield return new WaitForSeconds(1f); //másodpercenként menjen a ciklus
        }
    }
    
    

    void SetupGrid()
    {
        
        Transform parent = GameObject.Find("GRID").transform;
        for(int i = 0; i < actualGame.n;i++)
        {
            for(int j = 0; j < actualGame.n;j++)
            {
                GameObject go = Instantiate(fieldPrefab, new Vector3(i+0.5f, j+0.5f, 0), Quaternion.identity, parent);
				Field currentfield = go.GetComponent<Field> ();

                field[i, j].fieldClass = currentfield;


                currentfield.Setup(field[i, j].value,
                    i, j,
                    (field[i, j].value == -1), //akna -e?
                    textures[field[i, j].value + 1] //textúra.
                );
            }
        }

        parent.position = new Vector3(-actualGame.n / 2, -actualGame.n / 2, 0);
		Debug.Log ("grid setup size: " + actualGame.n);
        CameraControl.singleton.AlignCamera(actualGame.n);

    }


    private void CheckIfZero(int x,int y)
    {
        int n = actualGame.n;
        if (x < 0 || x >= n || y < 0 || y >= n)
        {
            return;
        }


     //   if(field[x,y].value == 0 )
     //   {
            field[x,y].fieldClass.ClickedMe(true);
      //  }


    }




    public void Clicked(int x,int y)
    {
		if(firstClick) {
			counter = StartCoroutine(Counter());
			Destroy(References.singleton.StartTip);
			point = 0;
			firstClick = false;
		}
        //Console.Log("Clicked on " + x + "," + y);


        int whatIsIt = field[x,y].value; //(int) mert vector2 floatot tárol..

        
		CalculatePoint (whatIsIt);

        if (whatIsIt == 0)
        {
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					CheckIfZero(x + i, y + j);
				}
			}
        }

		IsEnd(x,y,whatIsIt);

    }

	private void CalculatePoint(int actualNumber){
		if(actualNumber<1){
			return;
		}
		point = point + actualNumber * multiplier;
		pointText.text = point.ToString();
		//Debug.Log ("Added point: ["+ (point - actualNumber * multiplier) +" + "+ (actualNumber * multiplier) +"] (actual number: "+actualNumber+" * multiplier: "+multiplier+")");

	}

	private void IsEnd(int x, int y, int actualNumber)
    {
		if (actualNumber == -1) // :( (akna)
		{
			Loose(x, y);
			return;
		}
        remainingNotMineFields--;
        if (remainingNotMineFields <= 0)
        {
            Victory(x, y);
        }
    }

    private void Victory(int x, int y)
    {
        PLAYING = false;
        StartCoroutine(CameraControl.singleton.ResetCamera());
        StopCoroutine(counter);
        References.singleton.endGUI.Won();

		if(actualGame.mode == GameMode.REGULAR) StartCoroutine (Backend.singleton.SendScore(point));
    }

    private void Loose(int x, int y)
    {
        PLAYING = false;
        StartCoroutine(CameraControl.singleton.ResetCamera());
        StopCoroutine(counter);
        References.singleton.endGUI.Lost();

        StartCoroutine(FloodAlgorithm(x, y));
		StartCoroutine (Backend.singleton.SendScore(point));



    }

    void GenerateMines(int minecount){
		minePositions = new List<Vector2> ();



		int size = actualGame.n;

		while (minePositions.Count < minecount) {
			Vector2 mine = new Vector2 (
				UnityEngine.Random.Range (0, size),
				UnityEngine.Random.Range (0, size));
			if(!minePositions.Contains(mine)){
				minePositions.Add (mine);
				//Debug.Log(mine.x +" " + mine.y);// pozíciók kiírása
			}
		}
	}

	private void TryToAddOne(int x, int y){
		int n = actualGame.n;
		if (x < 0 || x >= n || y < 0 || y >= n) {
			return;
		}
		if(field [x, y].value == -1){
			return;
		}
		field [x, y].value++;
		//Debug.Log ("Adding mine to " + x + " " + y + "!");
	}

	private void FillMatrix(){
		foreach(Vector2 mine in minePositions){
			field[(int)mine.x, (int)mine.y].value = -1;
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					TryToAddOne ((int)mine.x + i, (int)mine.y + j);
				}
			}

			//TryToAddOne ((int)mine.x-1,(int)mine.y-1);

		}
	}


    


    IEnumerator FloodAlgorithm(int x, int y)
    {
        yield return floodWait;
        try
        {
            if (field[x, y].flooded) { yield break; }

            field[x, y].fieldClass.TurnMe();
            field[x, y].flooded = true;
        }
        catch (IndexOutOfRangeException)
        {
            yield break;
        }

        StartCoroutine(FloodAlgorithm(x - 1, y));
        
        StartCoroutine(FloodAlgorithm(x, y - 1));


        StartCoroutine(FloodAlgorithm(x - 1, y - 1));
     
        StartCoroutine(FloodAlgorithm(x + 1, y));

        StartCoroutine(FloodAlgorithm(x, y + 1));
     
        StartCoroutine(FloodAlgorithm(x + 1, y + 1));

        StartCoroutine(FloodAlgorithm(x - 1, y + 1));
      
        StartCoroutine(FloodAlgorithm(x + 1, y - 1));


    }

    public void FlagCount(int x)
    {
        flaggedCount += x;
        References.singleton.mines.text = (actualGame.mines - flaggedCount).ToString();


    }

    void StartMultiplayerGameAsHost() //játék indítása
    {


        flaggedCount = 0;
        firstClick = true;
        Console.Log("game mode: " + actualGame);


        References.singleton.mines.text = actualGame.mines.ToString();

        field = new FieldStruct[actualGame.n, actualGame.n];
        GenerateMines(actualGame.mines);
        FillMatrix();


        // actualGame = regular;
        SetupGrid();

        References.singleton.StartTip.SetActive(true);


        NetworkServer.SendToAll(100, new IntegerMessage(MsgTypes.SERVER_LOADED));

        remainingNotMineFields = actualGame.n * actualGame.n - actualGame.mines; 




    }
    public void RegisterHandlers()
    {
        if (NetworkServer.active)
        {
            NetworkServer.RegisterHandler(100, MessageFromClient);
        }
        else
        {
            NetworkManager.singleton.client.RegisterHandler(100, MessageFromServer);
        }

        Console.Log("Registering message handlers");
    }
    void MessageFromServer(NetworkMessage msg )
    {
        int msgInt = msg.ReadMessage<IntegerMessage>().value;
        Console.Log("Message from server:" + msgInt);
        switch(msgInt)
        {
            case MsgTypes.SERVER_LOADED:
                Console.Log("Message from server: READY");
                SceneManager.LoadScene("Game");
                break;
        }
    }
 
    void MessageFromClient(NetworkMessage msg)
    {

    }
}
