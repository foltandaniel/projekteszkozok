using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class References : MonoBehaviour {
    public Text timeText;

    public Text mines;
    public static References singleton;
    void Awake()
    {
        singleton = this;
    }
}
