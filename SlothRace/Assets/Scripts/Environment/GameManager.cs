using System;
using System.Collections;
using System.Collections.Generic;
using DualSenseSample.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager S;
    public GameObject playerInputManager;
    public DualSenseMonitor dualSenseMonitor;
    private PlayerInputManager _inputManager;
    
    public enum State
    {
        TitleScreen, LevelSelection, WaitForPlayers, Countdown, GameStart, GameEnd
    }
    
    // public variables
    public GameObject player1;
    public GameObject player2;
    public GameObject player3;
    public GameObject player4;

    public bool player1Started;
    public bool player2Started;

    [Header("Spawn Positions")] 
    [SerializeField] private Vector3 player1SpawnPoint;
    [SerializeField] private Vector3 player2SpawnPoint;
    [SerializeField] private Vector3 player3SpawnPoint;
    [SerializeField] private Vector3 player4SpawnPoint;

    [Header("Player Join Status")]
    // maxPlayerCount: how many players can we have for the current level
    public int maxPlayerCount;
    // joinedPlayer: the number of players that have joined the game
    public int joinedPlayer;
    public int readyPlayer;
    public State gameState;

    [Header("Player Rank")] 
    private float[] distance3p = new float[3];
    private float[] distance4p = new float[4];
    public float[] distances;
    
    private void Awake()
    {
        if (S)
        {
            Destroy(S.gameObject);
        }

        S = this;
    }

    private void Start()
    {
        _inputManager = playerInputManager.GetComponent<PlayerInputManager>();
    }

    // Update is called once per frame
    void Update()
    {
        EnableAndDisableJoin();
        ResetPlayer();
        CheckPlayerNum();
    }

    private void ResetPlayer()
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

    private void EnableAndDisableJoin()
    {
        if(gameState != State.WaitForPlayers) _inputManager.DisableJoining();
        else
        {
            if (_inputManager.playerCount >= maxPlayerCount)
            {
                _inputManager.DisableJoining();
            }
            else
            {
                _inputManager.EnableJoining();
            }
        }
    }

    private void CheckPlayerNum()
    {
        GUIManager.S.allSet = (joinedPlayer == maxPlayerCount);
    }

    public void SendPlayerToOrigin(int playerID)
    {
        if (playerID == 0)
        {
            player1.transform.position = player1SpawnPoint;
        }
        else
        {
            player2.transform.position = player2SpawnPoint;
        }
    }

    public void SetDistanceArray()
    {
        if (maxPlayerCount == 3)
        {
            distances = distance3p;
        } else if (maxPlayerCount == 4)
        {
            distances = distance4p;
        }
        else return;
    }

}
