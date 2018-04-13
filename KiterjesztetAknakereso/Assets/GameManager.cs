using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
}
public class GameManager : MonoBehaviour {

	public static bool PLAYING;
    public static GameManager singleton;
    public static Game regular = new Game("Regular", 15, 10, GameMode.REGULAR, false);
    public Game actualGame;

    public GameObject fieldPrefab;
    public Text timeText;

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

    //private Dictionary<Vector2, Field> fieldMap = new Dictionary<Vector2, Field>();
	private List<Vector2> minePositions;


   





   

   
    Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    int time;
    void Awake()
    {
		
			singleton = this;
        SceneManager.activeSceneChanged += SceneChanged;

       
    }

   
    private void Start()
    {
		//actualGame = regular;
       //StartGame(); //DEBUG
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





void StartGame() //játék indítása
    {
		Console.Log("game mode: " + actualGame);


        References.singleton.mines.text = actualGame.mines.ToString();

		field = new FieldStruct[actualGame.n, actualGame.n];
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
		PLAYING = true;
    }


    private void CheckIfZero(int x,int y)
    {
        int n = actualGame.n;
        if (x < 0 || x >= n || y < 0 || y >= n)
        {
            return;
        }


        if(field[x,y].value == 0 )
        {
            field[x,y].fieldClass.ClickedMe();
        }

    }
    public void Clicked(int x,int y)
    {

        //Console.Log("Clicked on " + x + "," + y);


        int whatIsIt = field[x,y].value; //(int) mert vector2 floatot tárol..

        if(whatIsIt == 0)
        {
            CheckIfZero(x - 1, y);
            CheckIfZero(x+1,y);
            CheckIfZero(x,y-1);
            CheckIfZero(x,y+1);
        }




        if (whatIsIt == -1) // :( (akna)
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        throw new NotImplementedException();
    }
}
