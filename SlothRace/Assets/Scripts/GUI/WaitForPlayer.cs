using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitForPlayer : MonoBehaviour
{
    [SerializeField] private WaitForPlayerVisual[] playerVisuals;
    [SerializeField] private int testNum;
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(testNum);
    }

    public void Join(int playerID)
    {
        if (playerID >= playerVisuals.Length) return;
        //Debug.Log("Player " + playerID + " is in.");
        playerVisuals[playerID].Join();
    }

    public void Ready(int playerID)
    {
        if (playerID >= playerVisuals.Length) return;
        playerVisuals[playerID].Ready();
    }
}
