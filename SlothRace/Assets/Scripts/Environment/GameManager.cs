using System;
using System.Collections;
using System.Collections.Generic;
using DualSenseSample.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Singleton pattern
    public static GameManager S;
    public GameObject playerInputManager;
    private PlayerInputManager _inputManager;
    
    public enum State
    {
        TitleScreen, LevelSelection, Mapping, WaitForPlayers, Countdown, GameStart, GameEnd
    }
    

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
    // readyPlayer: the number of players that are prepared
    public int readyPlayer;
    public State gameState;
    public float gameStartTime;

    [Header("Player Rank")]
    private float[] distance2p = new float[2];
    private float[] distance3p = new float[3];
    private float[] distance4p = new float[4];
    public float[] distances;
    
    [Header("Ending & Rank Check")]
    public Sprite[] rankNumbers = new Sprite[4];
    public GameObject finishLine;
    public Vector3 finishPosition;
    public int finishedPlayer;

    [Header("Player Data")] 
    public int[] playerFartTimes = new int[4];
    public int[] playerPoopTimes = new int[4];
    public float[] playerEndTimes = new float[4];
    public int[] playerEndOrder = new int[4];
    
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
        finishPosition = finishLine.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        EnableAndDisableJoin();
        CheckPlayerNum();
        
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
