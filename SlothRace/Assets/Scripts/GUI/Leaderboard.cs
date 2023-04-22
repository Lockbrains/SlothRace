using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private Text[] PlayerNames = new Text[4];
    [SerializeField] private Text[] fartTimes = new Text[4];
    [SerializeField] private Text[] poopTimes = new Text[4];
    [SerializeField] private Text[] endTimes = new Text[4];

    public void UpdateLeaderboard()
    {
        string[] names = new[] { "Thunder", "Lightning", "Hurricane", "Tornado" };
        
        // GameManager.S.playerEndOrder[n]: the nth player that finishes the race
        for (int i = 0; i < GameManager.S.maxPlayerCount; i++)
        {
            int ithPlayer = GameManager.S.playerEndOrder[i];
            PlayerNames[i].text = names[ithPlayer];
            fartTimes[i].text = GameManager.S.playerFartTimes[ithPlayer].ToString();
            poopTimes[i].text = GameManager.S.playerPoopTimes[ithPlayer].ToString();
            
            // display Time
            float timeSpent = GameManager.S.playerEndTimes[ithPlayer] - GameManager.S.gameStartTime;
            
            int timeRound = Mathf.RoundToInt(timeSpent);
            if (timeRound > timeSpent) timeRound -= 1;
            float fraction = timeSpent - timeRound;
            int fractionInt = (int) fraction * 100;
            
            int min = Mathf.RoundToInt(timeRound / 60);
            if (min * 60 > timeRound) min -= 1;
            
            int sec = timeRound % 60;
            endTimes[i].text = min.ToString() + ":" + sec.ToString() + ":" + fractionInt.ToString();
        }

        if (GameManager.S.maxPlayerCount < 4)
        {
            for (int j = GameManager.S.maxPlayerCount; j < 4; j++)
            {
                PlayerNames[j].text = "Invisible Sloth";
                fartTimes[j].text = "60419";
                poopTimes[j].text = "15213";
                endTimes[j].text = "IM:NOT:HERE";
            }
        }
    }

}
