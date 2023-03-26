using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitForPlayer : MonoBehaviour
{
    [SerializeField] private WaitForPlayerVisual[] playerVisuals;

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Join(int playerID)
    {
        if (playerID >= playerVisuals.Length) return;
        Debug.Log("Player " + playerID + " is in.");
        playerVisuals[playerID].Join();
    }
}
