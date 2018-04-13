using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Animator))]
public class Field : MonoBehaviour {

    private int x,y;
    bool alreadyClicked,amIMine;


    //refs
    public Renderer front;
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    
    }

    public TextMesh text;//debug
	public void Setup(int number,int x,int y,bool mine,Texture texture){
		text.text = number.ToString();
        this.x = x;
        this.y = y;
        amIMine = mine;


        front.material.mainTexture = texture;

	}


    public void ClickedMe()
    {
        if (alreadyClicked) return;

        anim.Play("Turn");
        alreadyClicked = true;
        GameManager.singleton.Clicked(x,y);
    


        
    }

}
