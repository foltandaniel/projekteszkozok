using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    public Animator mainCanvas, playCanvas, credits; //menük animator
    private Stack<Animator> animatorStack = new Stack<Animator>();
    /* Veremben tároljuk a navigációt, így egyszerűen vissza lehet jönni 
     * PL: MainMenu -> Play -> Custom */
    Animator GetCurrent() //jelenlegi menü lekérése
    {
        return animatorStack.Peek();
    }
    //jelenlegi menü kiszedése veremből
    Animator GetCurrentPop()
    {
        return animatorStack.Pop();
    }

    // Use this for initialization
    void Start () {
#if UNITY_STANDALONE_WIN
        Screen.SetResolution(360, 640, false);
#endif
        Console.Log("MainMenu Start");
        animatorStack.Push(mainCanvas); //mainmenuvel kezdünk
    }

    public void Exit()
    {
        Dialog.Question("Exit game?", new UnityEngine.Events.UnityAction(delegate { Application.Quit(); }),new UnityEngine.Events.UnityAction(delegate { }));
    }

    public void ChangeTo(Animator to)
    {
        //melyik menübe megyünk?
        Console.Log("Change to: " + to.name);
        GetCurrent().Play("out");
        to.Play("in");
        animatorStack.Push(to);
    }

    public void Back()
    {
        Console.Log("Back");
        if (GetCurrent() == mainCanvas)
        {
            Application.Quit();
        }
        /* vissza gomb, ha mainmenüben vagyunk, akkor kilépés */
        GetCurrentPop().Play("out");
        GetCurrent().Play("in");
        /* menü animációk
         * ki -> ami menüből jövünk
         * be -> amibe megyünk */
    }


    public void ShowLogin()
    {
        Login.Show();
    }


    public void StartRegular()
    {
        Backend.ShowHideLoad(true);
        GameManager.StartRegular();
    }
}
