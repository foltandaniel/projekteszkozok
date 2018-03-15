using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Dialog : MonoBehaviour {
    public static Dialog singleton;
    [SerializeField]
    private Text question;
    [SerializeField]
    private GameObject dialog;
    [SerializeField]
    private Button yes;
    [SerializeField]
    private Button cancel;


    UnityAction closeDialog = new UnityAction(delegate {
        singleton.dialog.SetActive(false); //ablak bezárása minden gombnyomáskor
        singleton.cancel.gameObject.SetActive(false);
        singleton.yes.gameObject.SetActive(false);
    });
    void Awake()
    {
        singleton = this;
        Console.Log("Dialog instantiated");

    }

    public static void Question(string question,UnityAction yes,UnityAction no)
    {
        /* felugró ablak */
        singleton.dialog.SetActive(true); //ablak bekapcsolása
        Console.Log("Showing dialog: " + question);
        singleton.question.text = question;
        singleton.AddEventsToButtons(yes, no);
        singleton.yes.gameObject.SetActive(true);
        singleton.cancel.gameObject.SetActive(true);
    }
    public static void Info(string info,UnityAction cancel)
    {
        singleton.dialog.SetActive(true);
        Console.Log("Showing info: " + info);
        singleton.question.text = info;
        singleton.AddEventsToButtons(null, cancel);
        singleton.cancel.gameObject.SetActive(true);
    }
    private void AddEventsToButtons(UnityAction yes,UnityAction no)
    {
        /* eventek hozzáadása */
        this.yes.onClick.RemoveAllListeners();
        this.cancel.onClick.RemoveAllListeners();

        if(yes != null)this.yes.onClick.AddListener(yes);
        if(cancel != null) this.cancel.onClick.AddListener(no);
        this.yes.onClick.AddListener(closeDialog); //miután kattintunk, bezárjuk az ablakot
        this.cancel.onClick.AddListener(closeDialog);
    }
}
