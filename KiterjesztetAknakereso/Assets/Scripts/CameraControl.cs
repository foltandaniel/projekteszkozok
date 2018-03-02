using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

	private Vector3 startMousePos;
	public float distanceThreshold = 1f;

	private float dragDistance;
	// Update is called once per frame
	void Update () {

		//DRAG
		if (Input.GetMouseButtonDown (0)) {
			dragDistance = 0f;
			startMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			startMousePos.z = 0.0f;
		}

		if (Input.GetMouseButton (0)) {
			Vector3 nowMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			nowMousePos.z = 0.0f;

			dragDistance += (startMousePos - nowMousePos).magnitude;
			transform.position += startMousePos - nowMousePos;
		}


		if (Input.GetMouseButtonUp (0)) {

			if (dragDistance < distanceThreshold) {
				Debug.Log ("CLICK");
			}
			
		}
	}
}
