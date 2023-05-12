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
    private List<GameObject> playerCameras = new List<GameObject>();
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
        Player thisPlayer = player.transform.parent.GetComponent<Player>();
        playerControllers.Add(thisPlayer);
        playerCameras.Add(thisPlayer.slothCamera);
        
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
        // get players
        GameObject player1, player2, player3, player4;
        player1 = playerGOs[0];
        player2 = playerGOs[1];
        player3 = playerGOs[2];
        player4 = playerGOs[3];

        Transform player1model = player1.transform.GetChild(0).transform.GetChild(0);
        Transform player2model = player2.transform.GetChild(0).transform.GetChild(0);
        Transform player3model = null;
        Transform player4model = null;

        // get player physical models
        if (player3 != null)
        {
            player3model = player3.transform.GetChild(0).transform.GetChild(0);
        }
        if (player4 != null)
        {
            player4model = player4.transform.GetChild(0).transform.GetChild(0);
        }

        // get positions
        Vector3 player1Pos = player1model.position;
        Vector3 player2Pos = player2model.position;
        Vector3 player3Pos = new Vector3(0, 0, 0);
        Vector3 player4Pos = new Vector3(0, 0, 0);

        if (player3 != null)
        {
            player3Pos = player3model.position;
        }
        if (player4 != null)
        {
            player4Pos = player4model.position;
        }

        // compare positions to final destinations
        float p1Distance = Vector3.Distance(player1Pos, GameManager.S.finishPosition);
        float p2Distance = Vector3.Distance(player2Pos, GameManager.S.finishPosition);
        float p3Distance = 0;
        float p4Distance = 0;

        if (player3 != null)
        {
            p3Distance = Vector3.Distance(player3Pos, GameManager.S.finishPosition);
        }
        if (player4 != null)
        {
            p4Distance = Vector3.Distance(player4Pos, GameManager.S.finishPosition);
        }

        List<Tuple<float, int>> playerRankList = new List<Tuple<float, int>>();

        // add to tuple list with respective player index and their distance
        playerRankList.Add(new Tuple<float, int>(p1Distance, 0));
        playerRankList.Add(new Tuple<float, int>(p2Distance, 1));

        if (player3 != null)
        {
            playerRankList.Add(new Tuple<float, int>(p3Distance, 2));
        }
        if (player4 != null)
        {
            playerRankList.Add(new Tuple<float, int>(p4Distance, 3));
        }

        SortPlayerRanks(playerRankList);

        // update ranks
        for (int i = 0; i < GameManager.S.maxPlayerCount; i++)
        {
            int playerID = playerRankList[i].Item2;

            GUIManager.S.ChangePlayerRank(GameManager.S.rankNumbers[i], playerID);
            if (playerID == 0)
            {
                player1.GetComponent<Player>().rank = i + 1;
            } else if (playerID == 1)
            {
                player2.GetComponent<Player>().rank = i + 1;
            }
            else if (playerID == 2)
            {
                player3.GetComponent<Player>().rank = i + 1;
            }
            else if (playerID == 3)
            {
                player4.GetComponent<Player>().rank = i + 1;
            }

        }

        playerRankList.Clear();
        //int id = GC.GetGeneration(playerRankList);
        //GC.Collect(id, GCCollectionMode.Forced);
  
    }

    private void SortPlayerRanks(List<Tuple<float, int>> playerRankList)
    {
        playerRankList.Sort((x, y) => x.Item1.CompareTo(y.Item1));
    }

    public void TurnOffCameras()
    {
        foreach (var playerCamera in playerCameras)
        {
            playerCamera.SetActive(false);
        }
    }

    public void TurnOnCameras()
    {
        foreach (var playerCamera in playerCameras)
        {
            playerCamera.SetActive(true);
        }
    }
}
