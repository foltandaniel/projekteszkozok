using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SinglePlayerGameManager : NetworkBehaviour {

    protected Game actualGame;
    private int point;
    private int multiplier = 0;
    int time;
    bool firstClick;

    public Text timeText;
    public Text pointText;


    public FieldStruct[,] field;
    protected int flaggedCount; //mennyi mező van flagelve?
    protected List<Vector2> minePositions;

    protected int remainingNotMineFields;

    protected Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    protected WaitForSeconds floodWait = new WaitForSeconds(0.17f);
   


    public virtual void Start()
    {
        GlobalGameManager.singleton.actualGameManager = this;
        actualGame = GlobalGameManager.singleton.actualGame;
        timeText = References.singleton.timeText;
        pointText = References.singleton.pointOrWhoText;      
        flaggedCount = 0;
       
        Console.Log("game mode: " +  actualGame);


        References.singleton.mines.text = actualGame.mines.ToString();

        field = new FieldStruct[actualGame.n, actualGame.n];

        remainingNotMineFields = actualGame.n * actualGame.n - actualGame.mines;
        GlobalGameManager.canMove = true;
        Backend.ShowHideLoad(false);
        SetupGameMap();
       
    }
    protected virtual void SetupGameMap()
    {
        GenerateMines(actualGame.mines);

        FillMatrix();
        SetupGrid();
        firstClick = true;
        References.singleton.StartTip.SetActive(true);
    }


    IEnumerator SinglePlayerCounter() //számláló
    {

        time = 0;

        while (true)
        {
            timeText.text = (time / 60).ToString("00") + ":" + (time % 60).ToString("00");
            multiplier = 6 - (Math.Min((time / 60), 5));
            time++;
            yield return new WaitForSeconds(1f); //másodpercenként menjen a ciklus
        }
    }

    private void CalculatePoint(int actualNumber)
    {
        if (actualNumber < 1)
        {
            return;
        }
        point = point + actualNumber * multiplier;
        pointText.text = point.ToString();
        //Debug.Log ("Added point: ["+ (point - actualNumber * multiplier) +" + "+ (actualNumber * multiplier) +"] (actual number: "+actualNumber+" * multiplier: "+multiplier+")");

    }


    public virtual void Clicked(int x, int y)
    {
        Field clickedField = field[x, y].fieldClass;

        if (clickedField.IsFlagged())
        {
            clickedField.FlagMe(); //ha flagelve van, akkor csak unflag, semmi más
            return;
        }
        //NINCS FLAGELVE
        ActualClick(x, y);
        
    }

    protected virtual void ActualClick(int x, int y)
    {
        if (field[x, y].fieldClass.alreadyClicked) return;



        if (firstClick)
        {
            counter = StartCoroutine(SinglePlayerCounter());
            point = 0;
            firstClick = false;

            Destroy(References.singleton.StartTip);
        }

        field[x,y].fieldClass.TurnMe();

        int whatIsIt = field[x, y].value;
        CalculatePoint(whatIsIt);

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

    public virtual void Victory(int x, int y)
    {
      
        StartCoroutine(CameraControl.singleton.ResetCamera());
        StopCoroutine(counter);
        References.singleton.endGUI.Won();

        if (actualGame.mode == GameMode.REGULAR && !actualGame.multiplayer) StartCoroutine(Backend.singleton.SendScore(point));
    }
    public virtual void Loose(int x, int y)
    {

        StartCoroutine(CameraControl.singleton.ResetCamera());
        StopCoroutine(counter);
        References.singleton.endGUI.Lost();

        StartCoroutine(FloodAlgorithm(x, y));
        StartCoroutine(Backend.singleton.SendScore(point));



    }


    protected void SetupGrid()
    {

        Transform parent = GameObject.Find("GRID").transform;
        for (int i = 0; i < actualGame.n; i++)
        {
            for (int j = 0; j < actualGame.n; j++)
            {
                if(GlobalGameManager.singleton.fieldPrefab == null) {
                    Console.LogError("FieldPrefab is null!!!");
                }
                GameObject go = Instantiate(GlobalGameManager.singleton.fieldPrefab, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity, parent);
                Field currentfield = go.GetComponent<Field>();

                field[i, j].fieldClass = currentfield;


                currentfield.Setup(field[i, j].value,
                    i, j,
                    (field[i, j].value == -1), //akna -e?
                    GlobalGameManager.singleton.textures[field[i, j].value + 1] //textúra.
                );
            }
        }

        parent.position = new Vector3(-actualGame.n / 2, -actualGame.n / 2, 0);
        Debug.Log("grid setup size: " + actualGame.n);
        CameraControl.singleton.AlignCamera(actualGame.n);

    }


    protected void CheckIfZero(int x, int y)
    {
        int n = actualGame.n;
        if (x < 0 || x >= n || y < 0 || y >= n)
        {
            return;
        }


        //   if(field[x,y].value == 0 )
        //   {
        ActualClick(x, y);
        //  }


    }

    protected void GenerateMines(int minecount)
    {
        minePositions = new List<Vector2>();



        int size = actualGame.n;

        while (minePositions.Count < minecount)
        {
            Vector2 mine = new Vector2(
                UnityEngine.Random.Range(0, size),
                UnityEngine.Random.Range(0, size));
            if (!minePositions.Contains(mine))
            {
                minePositions.Add(mine);
                //Debug.Log(mine.x +" " + mine.y);// pozíciók kiírása
            }
        }
    }

    protected void TryToAddOne(int x, int y)
    {
        int n = actualGame.n;
        if (x < 0 || x >= n || y < 0 || y >= n)
        {
            return;
        }
        if (field[x, y].value == -1)
        {
            return;
        }
        field[x, y].value++;
        //Debug.Log ("Adding mine to " + x + " " + y + "!");
    }

    protected void FillMatrix()
    {
        foreach (Vector2 mine in minePositions)
        {
            field[(int)mine.x, (int)mine.y].value = -1;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    TryToAddOne((int)mine.x + i, (int)mine.y + j);
                }
            }

            //TryToAddOne ((int)mine.x-1,(int)mine.y-1);

        }
    }





    protected IEnumerator FloodAlgorithm(int x, int y)
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


    protected virtual void IsEnd(int x, int y, int actualNumber)
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

    public void FlagCount(int x)
    {
        flaggedCount += x;
        References.singleton.mines.text = (actualGame.mines - flaggedCount).ToString();


    }




}
