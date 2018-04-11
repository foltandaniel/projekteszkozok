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
public class GameManager : MonoBehaviour {


	public static bool PLAYING;
	private int[,] field; 
	//map
	//akna -1
	//számok 0-8

	private List<Vector2> minePositions;


    public GameObject fieldPrefab;





    public static GameManager singleton;
    public static Game regular = new Game("Regular", 15, 10, GameMode.REGULAR, false);
    public Game actualGame;


    public Text timeText;
    Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    int time;
    void Awake()
    {
		if (!singleton != null) {
			Console.Log ("Game Manager already exists!!");
		} else {
			singleton = this;
		}
        SceneManager.activeSceneChanged += SceneChanged;

       
    }

    private void Start()
    {
		//actualGame = regular;
       // StartGame(); //DEBUG
    }
    private void SceneChanged(Scene from, Scene to)
    {
        Console.Log("SCENE CHANGE: "+ from.name + "->" + to.name);
		if (to.name == "Game") {
			timeText = References.singleton.timeText;
			StartGame ();
		} else {
			PLAYING = false;
		}
    }
    public static void StartRegular()
    {
        singleton.actualGame = regular;
		Debug.Log ("actual game set");
        SceneManager.LoadScene("Game");
    }



	void GenerateMines(int minecount){
		minePositions = new List<Vector2> ();
		int size = actualGame.n;

		while (minePositions.Count < minecount) {
			Vector2 mine = new Vector2 (
		    Random.Range (0, size),
			Random.Range (0, size));
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
		if(field [x, y] == -1){
			return;
		}
		field [x, y]++;
		//Debug.Log ("Adding mine to " + x + " " + y + "!");
	}



	private void FillMatrix(){
		foreach(Vector2 mine in minePositions){
			field[(int)mine.x, (int)mine.y] = -1;
			for (int i = -1; i <= 1; i++) {
				for (int j = -1; j <= 1; j++) {
					TryToAddOne ((int)mine.x + i, (int)mine.y + j);
				}
			}

			//TryToAddOne ((int)mine.x-1,(int)mine.y-1);

		}
	}





void StartGame() //játék indítása
    {
		Console.Log("game mode: " + actualGame);
		field = new int[actualGame.n, actualGame.n];
		GenerateMines (actualGame.mines);
		FillMatrix ();

        counter = StartCoroutine(Counter());
       // actualGame = regular;
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
        for(int i = 0; i < actualGame.n;i++)
        {
            for(int j = 0; j < actualGame.n;j++)
            {
                GameObject go = Instantiate(fieldPrefab, new Vector3(i+0.5f, j+0.5f, 0), Quaternion.identity, parent);
				Field currentfield = go.GetComponent<Field> ();
				currentfield.Setup (field [i, j]);
            }
        }

        parent.position = new Vector3(-actualGame.n / 2, -actualGame.n / 2, 0);
		Debug.Log ("grid setup size: " + actualGame.n);
        CameraControl.singleton.AlignCamera(actualGame.n);
		PLAYING = true;
    }

   
}
