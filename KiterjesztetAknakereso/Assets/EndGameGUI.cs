using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class EndGameGUI : MonoBehaviour {


    private Animator anim;
    public Text result;

	// Use this for initialization
	void Start () {
        anim = GetComponent<Animator>();
	}
	
	public void Lost()
    {
        result.text = "<color=red>Lost</color>";
        anim.Play("End");
    }

    internal void Won()
    {
        result.text = "<color=green>Won</color>";
        anim.Play("End");
    }


    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    
}
