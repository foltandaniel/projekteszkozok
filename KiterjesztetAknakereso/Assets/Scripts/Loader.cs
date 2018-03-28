using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
    public GameObject staticObjPrefab;
    // Use this for initialization
    private static bool alreadyLoaded;
	void Awake () {
        /* static objekt inicializálása, ha még nincs 
         * Ezen lesz: Backend, Console, stb
         * Ezek nem "pusztulnak" el Scene váltáskor sem (amikor játékmódba váltunk) */
		if(!alreadyLoaded)
        {
            
            DontDestroyOnLoad(Instantiate(staticObjPrefab)); //Ne pusztuljon el scene váltáskor.
            Console.Log("LOADER: LOADING STATIC OBJECTS");
            Destroy(this.gameObject); //Erre a Loaderre már nincs szükségünk
            alreadyLoaded = true;
        }
	}
}
