using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Network : MonoBehaviour {


	public string LOGIN_URL;
	// Use this for initialization
	void Start () {
		StartCoroutine (SendPost());
	}
	IEnumerator SendPost() {
		
		WWWForm form = new WWWForm ();

		form.AddField ("username","Jóska");
		form.AddField ("password","pwJóska");
		WWW www = new WWW (LOGIN_URL,form);
		yield return www;
	

		Debug.Log ("Response:" + www.text);
	}
}
