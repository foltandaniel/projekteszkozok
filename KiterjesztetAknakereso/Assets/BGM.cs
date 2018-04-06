using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {


    public AudioClip intro;
    public AudioSource intros,loop;
    IEnumerator Start()
    {

        intros.Play();
        yield return new WaitForSeconds(intro.length);
        loop.Play();
    }
	
	
	
	
}
