using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CustomGameStarter : MonoBehaviour {

	public InputField map,mine;
	private int mapSize, numberOfMines, maxNumberOfMines;
	void Start() {
		MapFieldLooseFocus ();
		MineFieldLooseFocus ();
	}
	private void ChangeMapSize(int f)
	{
		mapSize = f;
		map.text=mapSize.ToString();
		if (mapSize > 0) {
			maxNumberOfMines = (mapSize - 1) * (mapSize - 1);
		}
	}

	public void MapFieldLooseFocus(){
		try{
			ChangeMapSize(int.Parse(map.text));
			//mapSize = int.Parse(map.text);
		}catch(Exception e) {
			Console.Log ("Exception in CustomGameStarter: " + e);
			Toast.Show ("Invalid parameter!");
			map.text = GameManager.regular.n.ToString();
			return;
		}
		maxNumberOfMines = (mapSize - 1) * (mapSize - 1);
		FixPlayerMistakes ();
	}

	public void MineFieldLooseFocus(){
		try{
			numberOfMines=int.Parse(mine.text);
		}catch(Exception e) {
			Console.Log ("Exception in CustomGameStarter: " + e);
			Toast.Show ("Invalid parameter!");
			mine.text = GameManager.regular.mines.ToString();
			return;
		}
		FixPlayerMistakes ();
	}

	private void FixPlayerMistakes(){
		MineValudation ();
		MapValudation ();
		if(numberOfMines>maxNumberOfMines){
			numberOfMines=maxNumberOfMines;
			mine.text=numberOfMines.ToString();
		}
	}

	private void MapValudation (){
		if (mapSize < 4) {
			ChangeMapSize (4);
			/*mapSize = 0;
			map.text=mapSize.ToString();*/
			return;
		}
		if (mapSize > 50) {
			ChangeMapSize (50);
			/*mapSize = 50;
			map.text=mapSize.ToString();
			maxNumberOfMines = (mapSize - 1) * (mapSize - 1);*/
			return;
		}

	}


	private void MineValudation(){
		if (numberOfMines < 0) {
			numberOfMines = 0;
			mine.text=numberOfMines.ToString();
			return;
		}
	}

	public void Play_Button() {
		FixPlayerMistakes ();
		Game game = new Game ("Custom Game", mapSize, numberOfMines, GameMode.CUSTOM, false);
		GameManager.StartCustom (game);
	}

}
