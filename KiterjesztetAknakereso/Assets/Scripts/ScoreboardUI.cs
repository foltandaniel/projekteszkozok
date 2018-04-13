using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour {
    [SerializeField]
    private GameObject scoreElementPrefab;
    [SerializeField]
    private Transform contentList;
	public void Refresh()
    {
        for(int i = 0; i < contentList.childCount;i++)
        {
            Destroy(contentList.GetChild(i).gameObject);
        }
        Backend.ShowHideLoad(true);
        Console.Log("Start GetScoreboard");
        StartCoroutine(Backend.singleton.GetScoreboard(new UnityEngine.Events.UnityAction<string[]>(ScoreboardDownloaded)));
    }

    void ScoreboardDownloaded(string[] scores)
    {
        Backend.ShowHideLoad(false);
        Console.Log("Scoreboard refresh");
        int index = 1;
        foreach(string score in scores)
        {
            ScoreElementReference scoreRef =  GameObject.Instantiate(scoreElementPrefab, contentList).GetComponent<ScoreElementReference>();
            string[] splittedScore = score.Split('|');

            scoreRef.Setup(index.ToString(), splittedScore[0], splittedScore[1]);
            index++;
        }
    }
}
