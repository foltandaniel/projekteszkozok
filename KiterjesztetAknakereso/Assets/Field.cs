using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
	Material front;
public TextMesh text;//debug
	public void Setup(int number){
		text.text = number.ToString();
	}
}
