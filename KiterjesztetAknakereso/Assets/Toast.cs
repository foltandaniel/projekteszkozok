using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Toast : MonoBehaviour {
    private static Toast singleton;
    public Text toastmsg;
    Animator animator;

    void Awake()
    {
        singleton = this;
    }
    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
      //  ShowToast("Test toast");
	}
	public static void ShowToast(string msg)
    {
        singleton.toastmsg.text = msg;
        singleton.animator.Play("Toast");
    }
}
