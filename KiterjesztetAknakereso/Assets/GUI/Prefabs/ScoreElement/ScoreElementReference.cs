using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ScoreElementReference : MonoBehaviour {
    public Text place, nameText, score;

    public void Setup(string place,string name,string score)
    {
        this.place.text = place;
        this.nameText.text = name;
        this.score.text = score;
    }
}
