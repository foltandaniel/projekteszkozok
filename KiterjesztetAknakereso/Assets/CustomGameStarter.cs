using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class CustomGameStarter : MonoBehaviour {

	[SerializeField]
	private int minimumMapSize=4;
	[SerializeField]
	private int maximumMapSize=50;
	[SerializeField]
	private int minimumNumberOfMines=0;

	public InputField map,mine;
	private int mapSize, numberOfMines, maxNumberOfMines;

	void Start() {//alapértékek beolvasása a változókba
		MapFieldLooseFocus ();
		MineFieldLooseFocus ();
	}

	private void ChangeMapSize(int f)//ez a függvény biztosítja, hogy a kijelzőn lévő szám és a memóriában lévő változó együtt módosuljon, és nem engedi hogy invalid adatunk legyen
	{
		/*
		if (f < minimumMapSize) {
			f=minimumMapSize;
		}
		if (f > maximumMapSize) {
			f=maximumMapSize;
		}*/

		MapValudation (ref f);

		mapSize = f;
		map.text=mapSize.ToString();
		maxNumberOfMines = (mapSize - 1) * (mapSize - 1);
	}

	private void ChangeNumberOfMines(int f){//ugyanaz mint ChangeMapSize csak aknában
		
		MineValudation (ref f);

		numberOfMines=f;
		mine.text=numberOfMines.ToString();
	}

	public void MapFieldLooseFocus(){
		try{
			ChangeMapSize(int.Parse(map.text));
			//mapSize = int.Parse(map.text);
		}catch(Exception e) {
			Console.Log ("Exception in CustomGameStarter: " + e);
			Toast.Show ("Invalid parameter!");
			ChangeMapSize (GameManager.regular.n);
			return;
		}
		FixPlayerMistakes ();
	}

	public void MineFieldLooseFocus(){
		try{
			ChangeNumberOfMines(int.Parse(mine.text));
		}catch(Exception e) {
			Console.Log ("Exception in CustomGameStarter: " + e);
			Toast.Show ("Invalid parameter!");
			mine.text = GameManager.regular.mines.ToString();
			return;
		}
		FixPlayerMistakes ();
	}

	private void FixPlayerMistakes(){
		/*
		MineValudation ();
		MapValudation ();*/
		if(numberOfMines>maxNumberOfMines){
			ChangeNumberOfMines (maxNumberOfMines);
		}
	}


	//melyik legyen?
	private void MapValudation (){//alap változatlan
		if (mapSize < minimumMapSize) {
			ChangeMapSize (minimumMapSize);
			/*mapSize = 0;
			map.text=mapSize.ToString();*/
			return;
		}
		if (mapSize > maximumMapSize) {
			ChangeMapSize (maximumMapSize);
			/*mapSize = 50;
			map.text=mapSize.ToString();
			maxNumberOfMines = (mapSize - 1) * (mapSize - 1);*/
			return;
		}

	}

	private void MapValudation (ref int f){//referenciás (.netes)
		if (f < minimumMapSize) {
			f=minimumMapSize;
			return;
		}
		if (f > maximumMapSize) {
			f=maximumMapSize;
			return;
		}
		return;
	}

	private int MapValudation (int f){//sima értékvisszaadós
		if (f < minimumMapSize) {
			f=minimumMapSize;
			return f;
		}
		if (f > maximumMapSize) {
			f=maximumMapSize;
			return f;
		}
		return f;
	}

	//melyik legyen?
	private void MineValudation(){//alap változatlan
		if (numberOfMines < minimumNumberOfMines) {
			ChangeNumberOfMines (minimumNumberOfMines);
			return;
		}
	}

	private void MineValudation(ref int f){//referenciás (.net)
		if (f < minimumNumberOfMines) {
			f=minimumNumberOfMines;
		}
	}

	private int MineValudation( int f){//értékvisszaadós
		if (f < minimumNumberOfMines) {
			f=minimumNumberOfMines;
		}
		return f;
	}

	public void Play_Button() {
		FixPlayerMistakes ();
		Game game = new Game ("Custom Game", mapSize, numberOfMines, GameMode.CUSTOM, false);
		GameManager.StartCustom (game);
	}

}
