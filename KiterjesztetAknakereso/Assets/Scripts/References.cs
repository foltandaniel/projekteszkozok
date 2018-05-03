using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class References : MonoBehaviour {
    public Text timeText,pointText;

    public Text mines;
    public EndGameGUI endGUI;
	public GameObject StartTip;
    public static References singleton;
    void Awake()
    {
        singleton = this;
    }
}
