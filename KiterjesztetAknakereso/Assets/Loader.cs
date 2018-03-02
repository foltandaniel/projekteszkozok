using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader : MonoBehaviour {
    public GameObject staticObjPrefab;
    // Use this for initialization
    private static bool alreadyLoaded;
	void Awake () {
		if(!alreadyLoaded)
        {
            DontDestroyOnLoad(Instantiate(staticObjPrefab));

            Destroy(gameObject);
            alreadyLoaded = true;
        }
	}
}
