using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager S;
    
    
    // public variables
    public GameObject player1;
    public GameObject player2;

    public bool player1Started;
    public bool player2Started;

    [Header("Spawn Positions")] 
    [SerializeField] private Vector3 player1SpawnPoint;
    [SerializeField] private Vector3 player2SpawnPoint;

    private void Awake()
    {
        if (S)
        {
            Destroy(S.gameObject);
        }

        S = this;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (player1 != null)
        {
            if (!player1Started)
            {
                player1.transform.position = player1SpawnPoint;
            }
        }
        
        if (player2 != null)
        {
            if (!player2Started)
            {
                player2.transform.position = player2SpawnPoint;
            }
        }
    }
    
}
