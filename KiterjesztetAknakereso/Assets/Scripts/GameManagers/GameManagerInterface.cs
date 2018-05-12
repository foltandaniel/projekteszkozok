using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManagerInterface : MonoBehaviour {

    protected abstract void Clicked(int x, int y);
    protected FieldStruct[,] field;
    protected int flaggedCount; //mennyi mező van flagelve?
    protected List<Vector2> minePositions;

    protected int remainingNotMineFields;

    protected Coroutine counter; //referencia a számláló funkcióra, hogy megtudjuk állítani
    protected WaitForSeconds floodWait = new WaitForSeconds(0.17f);
    protected Game actualGame;
    protected GameObject fieldPrefab;


    protected void SetMeUp()
    {
        actualGame = GlobalGameManager.singleton.actualGame;
        fieldPrefab = GlobalGameManager.singleton.fieldPrefab;
    }

    protected void SetupGrid()
    {

        Transform parent = GameObject.Find("GRID").transform;
        for (int i = 0; i < actualGame.n; i++)
        {
            for (int j = 0; j < actualGame.n; j++)
            {
                GameObject go = Instantiate(fieldPrefab, new Vector3(i + 0.5f, j + 0.5f, 0), Quaternion.identity, parent);
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
        field[x, y].fieldClass.TurnMe();
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


    protected void IsEnd(int x, int y, int actualNumber)
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
    protected abstract void Victory(int x, int y);

    protected abstract void Loose(int x, int y);

}
