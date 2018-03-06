using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour {

	public void Refresh()
    {
        Console.Log("Start GetScoreboard");
        StartCoroutine(Backend.singleton.GetScoreboard(new UnityEngine.Events.UnityAction<string[]>(ScoreboardDownloaded)));
    }

    void ScoreboardDownloaded(string[] scores)
    {
        Console.Log("Scoreboard refresh");
    }
}
