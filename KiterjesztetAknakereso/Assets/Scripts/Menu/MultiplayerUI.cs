using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class MultiplayerUI : NetworkDiscovery {
	[SerializeField]
	public Text matchesFound;


	#region list
	public GameObject listMenu;
	public Transform contentParent; //hova kerüljenek a gombok
	public GameObject joinButtonPrefab;
	#endregion


	private List<string> lobbys = new List<string>(); //tároljuk a szobákat
	public void Init() {
		Console.Log ("Started listening");
		lobbys.Clear ();
		Initialize ();
		StartAsClient ();


		/*OnReceivedBroadcast ("asd2", "asd");
		OnReceivedBroadcast ("asd3", "asd");
		OnReceivedBroadcast ("asd4", "asd");
		OnReceivedBroadcast ("asd5", "asd");
		OnReceivedBroadcast ("asd6", "asd");
		OnReceivedBroadcast ("asd7", "asd");
		OnReceivedBroadcast ("asd8", "asd");*/
	}
	public void StartListening() {
		Console.Log ("Started broadcasting");
		StopBroadcast ();
		StartAsServer ();
	}
	private void UpdateFoundMatchesText() {
		matchesFound.text = "Matches found: " + lobbys.Count;
	}

	private void AddToScrollList(string fromAdress,string data) {
		GameObject instantiated = Instantiate (joinButtonPrefab, contentParent);
		//TODO - funkció hozzáadása a létrehozott gombhoz..

		instantiated.GetComponentInChildren<Text> ().text = data;
	}
	public override void OnReceivedBroadcast(string fromAddress, string data)
	{
		
		if (lobbys.Contains (fromAddress)) //ha már tudunk a szobáról, figyelmen kvül hagyjuk
			return;
		Console.Log ("Recieved broadcast from " + fromAddress);
		//új szoba..
		lobbys.Add (fromAddress);
		UpdateFoundMatchesText ();

		AddToScrollList (fromAddress, data);

	}
	void Update() {
		if (Input.GetKeyDown (KeyCode.Escape))
			StopEverything ();
	}
	public void StopEverything() {
		Console.Log ("Stopped network");
		StopBroadcast ();
	}

	public void ShowList() {
		listMenu.SetActive (true);
	}
	public void HideList() {
		listMenu.SetActive (false);
	}
}
