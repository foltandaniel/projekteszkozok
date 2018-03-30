using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour {

	
	IEnumerator Start()
    {
        Destroy(this,15f);
        AudioSource source = GetComponent<AudioSource>();

        source.volume = 0f;
        while(source.volume < 1f)
        {
            source.volume += 0.08f * Time.deltaTime;
            yield return null;
        }
        
    }
}
