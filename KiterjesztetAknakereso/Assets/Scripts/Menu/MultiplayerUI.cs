using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;


public class MultiplayerUI : MonoBehaviour {
	public Text matchesFoundText;


	#region list
	public GameObject listMenu;
	public Transform contentParent; //hova kerüljenek a gombok
	public GameObject joinButtonPrefab;
	#endregion


	private List<string> lobbys = new List<string>(); //tároljuk a szobákat
	public void Init() {
      
		
        GameDiscovery.singleton.StartListening();
		lobbys.Clear ();
        ClearList();
		

	}
    private void ClearList()
    {
        matchesFoundText.text = "Matches found on your network: 0";
        foreach(Transform go in contentParent)
        {
            Destroy(go.gameObject);
        }
    }
	public void StartGame() {
		
        GameDiscovery.singleton.StartBroadcasting();
        GameNetworkManager.CreateGame();
        Dialog.Info("Waiting for opponent..", new UnityEngine.Events.UnityAction(delegate
         {
             GameDiscovery.singleton.Stop();
             Init();
         }));
	}
	private void UpdateFoundMatchesText() {
        Console.Log("Update matches found");
	matchesFoundText.text = "Matches found on your network: " + lobbys.Count;
	}

	public void AddToScrollList(string fromAddress,string data) {
        if (lobbys.Contains(fromAddress)) //ha már tudunk a szobáról, figyelmen kvül hagyjuk
            return;
        lobbys.Add(fromAddress);
        UpdateFoundMatchesText();
        GameObject instantiated = Instantiate (joinButtonPrefab, contentParent);

        instantiated.GetComponentInChildren<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(delegate
        {
            //kattintás
            Dialog.Info("Connecting..", new UnityEngine.Events.UnityAction(delegate
            {
                //TODO -- CANCEL
            }));


            StopEverything();
            GameNetworkManager.singleton.TryToConnect(fromAddress);


        }));

		instantiated.GetComponentInChildren<Text> ().text = data;
	}
	

	public void StopEverything() {
		
        GameDiscovery.singleton.Stop();
	}

	public void ShowList() {
		listMenu.SetActive (true);
	}
	public void HideList() {
		listMenu.SetActive (false);
	}
}
