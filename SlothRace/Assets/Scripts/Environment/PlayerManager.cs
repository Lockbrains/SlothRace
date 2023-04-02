using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager S;
    
    [Header("Players")]
    private List<PlayerInput> players = new List<PlayerInput>();
    private List<Player> playerControllers = new List<Player>();
    public GameObject[] playerGOs = new GameObject[4];
    public bool[] playerHasStart = new bool[4];

    [SerializeField] private Vector3[] respawningPoints = new Vector3[4];
    [SerializeField] private List<LayerMask> playerLayers;

    private PlayerInputManager _playerInputManager;

    #region Unity Basics
    private void Awake()
    {
        S = this;
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ResetPlayer();
        UpdateRank();
    }

    private void UpdateRank()
    {
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            GetPlayersRank();
        }
    }
    
    #endregion
    
    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        playerGOs[player.playerIndex] = player.transform.parent.gameObject;
        playerControllers.Add(player.transform.parent.GetComponent<Player>());
        
        Transform playerTransform = player.transform.parent;
        
        // Convert layer mask to an integer
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

        // set the layer
        playerTransform.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        
        // add the layer
        player.camera.cullingMask |= 1 << layerToAdd;
        
        // set the action in the custom cinemachine input handler
        playerTransform.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("RightStickMove");
    }
    
    private void ResetPlayer()
    {
        for (int i = 0; i < 4; i++)
        {
            if (playerGOs[i] != null)
            {
                if (!playerHasStart[i]) playerGOs[i].transform.position = respawningPoints[i];
            }
        }
    }
    
    public void RespawnPlayer(int playerID)
    {
        playerGOs[playerID].transform.position = respawningPoints[playerID];
    }
    
    public void GetPlayersRank()
    {
        GameObject player1, player2;
        player1 = playerGOs[0];
        player2 = playerGOs[1];
        Transform player1model = player1.transform.GetChild(0).transform.GetChild(3);
        Transform player2model = player2.transform.GetChild(0).transform.GetChild(3);

        Vector3 player1Pos = player1model.position;
        Vector3 player2Pos = player2model.position;

        // compare positions to final destinations
        float p1Distance = Vector3.Distance(player1Pos, GameManager.S.finishPosition);
        float p2Distance = Vector3.Distance(player2Pos, GameManager.S.finishPosition);

        if (p1Distance > p2Distance)
        {
            // player 2 is in the lead 
            GUIManager.S.ChangePlayerRank(GameManager.S.rankNumbers[0], 1);
            player2.GetComponent<Player>().rank = 1;

            // player 1 is second
            GUIManager.S.ChangePlayerRank(GameManager.S.rankNumbers[1], 0);
            player1.GetComponent<Player>().rank = 2;
        } else
        {
            // player 1 is in the lead
            GUIManager.S.ChangePlayerRank(GameManager.S.rankNumbers[0], 0);
            player1.GetComponent<Player>().rank = 1;

            // player 2 is second
            GUIManager.S.ChangePlayerRank(GameManager.S.rankNumbers[1], 1);
            player2.GetComponent<Player>().rank = 2;


        }
    }
}
