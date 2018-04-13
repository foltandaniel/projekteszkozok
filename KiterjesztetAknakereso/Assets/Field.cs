using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Animator))]
public class Field : MonoBehaviour {

    private int x,y;
    bool alreadyClicked,amIMine,flagged;


    //refs
    public Renderer front;
	public Renderer back;
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

	public void FlagMe() {
		if(!flagged) {
			back.material.color = Color.red;
			flagged = true;
		} else {
			back.material.color = Color.white;
			flagged = false;
		}
	}
	public void ClickedMe(bool logic)
	/* logic -- az algoritmus fordtotta-e át a fieldet (pl 0 szomszédja..) */
    {
        if (alreadyClicked) return;
		if(flagged) { 
			back.material.color = Color.white;
			flagged = false;

			if (!logic) return; //ha kézzel kattintottunk rá, és flaggelve van, akkor ne forduljon át 
		}
        anim.Play("Turn");
        alreadyClicked = true;
        GameManager.singleton.Clicked(x,y);
    


        
    }

}
