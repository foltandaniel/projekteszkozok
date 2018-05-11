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
    public Collider2D myCollider;
    void Start()
    {
        anim = GetComponent<Animator>();
    
    }

    public TextMesh text;//debug

    public int X
    {
        get
        {
            return x;
        }
    }

    public int Y
    {
        get
        {
            return y;
        }
    
    }

    public bool IsFlagged()
    {
        return flagged;
    }
    public void Setup(int number,int x,int y,bool mine,Texture texture){
		text.text = number.ToString();
        this.x = x;
        this.y = y;
        amIMine = mine;


        front.material.mainTexture = texture;

	}

	public void FlagMe() {
		if(!flagged) {
			DoFlag ();
            /*GameManager.singleton.FlagCount(+1);
            back.material.color = Color.red;
			flagged = true;*/
		} else {
			Unflag ();
			/*back.material.color = Color.white;
            GameManager.singleton.FlagCount(-1);
            flagged = false;*/
		}
	}


    public void TurnMe() {
		if (alreadyClicked)	return;
		
		anim.Play("Turn");
        Destroy(myCollider);
		if(flagged){
			FlagMe ();
		}
        alreadyClicked = true;
    }

	private void Unflag(){
		GameManager.singleton.FlagCount(-1);
		back.material.color = Color.white;
		flagged = false;
	}

	private void DoFlag(){
		GameManager.singleton.FlagCount(+1);
		back.material.color = Color.red;
		flagged = true;
	}
}
