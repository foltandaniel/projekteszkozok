using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class TouchAlpha : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(EventSystem.current.currentSelectedGameObject == this.gameObject)
        {
            GetComponent<CanvasGroup>().alpha = 1.0f;
        } else
        {
            GetComponent<CanvasGroup>().alpha = 0.2f;
        }
	}
}
