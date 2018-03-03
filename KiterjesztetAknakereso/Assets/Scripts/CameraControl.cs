using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	private Vector3 startMousePos;
	public float distanceThreshold = 1f; // legalább ennyit kell elmozdulnia a kurzornak, hogy ne számítson kattintásnak

	private float dragDistance; //mennyit mozgott a kurzor?
	// Update is called once per frame
	void Update () {

		//DRAG
		if (Input.GetMouseButtonDown (0)) { //ha nyomjuk a bal egér gombot (vagy touch0)
			dragDistance = 0f;
			startMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition); //hol nyomtuk le az egeret?
			startMousePos.z = 0.0f;
		}

		if (Input.GetMouseButton (0)) { //ha nyomva TARTJUK az egeret
			Vector3 nowMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition); //hol van most a kurzor? (World pozíció)
			nowMousePos.z = 0.0f;

			dragDistance += (startMousePos - nowMousePos).magnitude; //mennyit ment eddig a kurzor?
			transform.position += startMousePos - nowMousePos; //a kamerát mozgatjuk (azon van ez a script)
		}


		if (Input.GetMouseButtonUp (0)) { //felengedtük a gombot

			if (dragDistance < distanceThreshold) { //ha kevesebbet ment a kurzor, mint a threshold.
				Debug.Log ("CLICK"); //klikk
			}
			
		}
	}
}
