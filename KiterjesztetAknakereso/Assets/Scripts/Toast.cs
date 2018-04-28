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
        animator = GetComponent<Animator>();
    }
	public static void Show(string msg)
    {
        singleton.toastmsg.text = msg;
        singleton.animator.Play("Toast");
    }
}
