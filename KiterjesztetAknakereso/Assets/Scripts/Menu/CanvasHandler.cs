using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasHandler : MonoBehaviour {
    //Animáció miatt kell//
    private CanvasGroup canvas;
    void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    public void DisableCanvas()
    {
        //Ne legyen interaktív a menü, és ne blokkolja a kurzor raycastot
        canvas.interactable = false;
		canvas.blocksRaycasts = false;
    }
    public void EnableCanvas()
    {
        canvas.interactable = true;
		canvas.blocksRaycasts = true;
    }
}
