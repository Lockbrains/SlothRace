using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIManager : MonoBehaviour
{

    #region Variables

    public static GUIManager S;
    private GameManager.State _state;

    [Header("State UI")] [SerializeField] private GameObject titlePage;
    [SerializeField] private GameObject levelSelectionPage;
    [SerializeField] private GameObject mappingPage;
    [SerializeField] private GameObject waitForPlayerPage;
    [SerializeField] private GameObject countdownPage;
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject leaderboardPage;

    [Header("UI GameObjects")] public GameObject img_titleScreen;
    public GameObject img_waitingForPlayer2;

    [Header("Player HUD")] 
    [SerializeField] private PlayerHUDAssignment _assignment;
    private PlayerHUD[] _playerHUDs;

    [Header("UI Setting")] [SerializeField]
    private Color enableColor;

    [SerializeField] private Color disableColor;
    [SerializeField] private Color limbDisableColor;
    [SerializeField] private Color activeColor;
    [HideInInspector] public Animator player1Anim;
    [HideInInspector] public Animator player2Anim;
    [HideInInspector] public Animator player3Anim;
    [HideInInspector] public Animator player4Anim;

    [Header("Wait For Players")] [SerializeField]
    private GameObject twoPlayerMode;

    [SerializeField] private GameObject threePlayerMode;
    [SerializeField] private GameObject fourPlayerMode;
    [SerializeField] private GameObject allSetIcon;
    [HideInInspector] public bool allSet;
    private WaitForPlayer _waitModule;

    [Header("Countdown")]
    [SerializeField] private CountdownAssignment _cdAssignment;
    private GameObject three_label;
    private GameObject two_label;
    private GameObject one_label;
    private GameObject go_label;
    [SerializeField] private float shortTime;
    [SerializeField] private float longTime;
    private bool hasStartedCountdown;

    [Header("Game Over")] 
    [SerializeField] private Leaderboard _leaderboard;

    #endregion

    #region Unity Basics

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
        hasStartedCountdown = false;
    }

    private void Update()
    {
        UpdateHUD();
        UpdateListenToInput();
        UpdatePlayerScrollBar();
    }

    #region Update Helpers

    private void UpdateHUD()
    {
        _state = GameManager.S.gameState;
        switch (_state)
        {
            case GameManager.State.TitleScreen:
                titlePage.SetActive(true);
                levelSelectionPage.SetActive(false);
                mappingPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.Mapping:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
                mappingPage.SetActive(true);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.LevelSelection:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(true);
                mappingPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.WaitForPlayers:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
                mappingPage.SetActive(false);
                waitForPlayerPage.SetActive(true);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                allSetIcon.SetActive(allSet);
                CheckPlayerNum();
                break;
            case GameManager.State.Countdown:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
                mappingPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(true);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.GameStart:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
                mappingPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(true);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.GameEnd:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
                mappingPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void UpdateListenToInput()
    {
        switch (_state)
        {
            case GameManager.State.TitleScreen:
                if (Input.anyKey)
                {
                    GameManager.S.gameState = GameManager.State.LevelSelection;
                }

                break;
            case GameManager.State.Mapping:
                if (Input.GetKeyDown(KeyCode.Space) || Input.GetButtonDown("Cancel"))
                {
                    GameManager.S.gameState = GameManager.State.WaitForPlayers;
                    OnMappingToWaitForPlayer();
                }

                break;
            default:
                break;
        }
    }

    private void UpdatePlayerScrollBar()
    {
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            int len = _playerHUDs.Length;
            Debug.Assert(len >= 2);
            _playerHUDs[0].UpdateProgress(player1Anim);
            _playerHUDs[1].UpdateProgress(player2Anim);
            if (len >= 3) _playerHUDs[2].UpdateProgress(player3Anim);
            if (len >= 4) _playerHUDs[3].UpdateProgress(player4Anim);
        }
    }

    #endregion

    #endregion

    #region LevelSelection, Codes for Ad Board

    public void OnLevelSelectButtonClick(int playerNum)
    {
        if (GameManager.S.gameState == GameManager.State.LevelSelection)
        {
            GameManager.S.maxPlayerCount = playerNum;
            GameManager.S.gameState = GameManager.State.Mapping;
            _playerHUDs = _assignment.AssignPlayerHUD(playerNum);
            _assignment.TurnOnHUD(playerNum);
            GameObject[] labels = _cdAssignment.AssignmentCDHUD(playerNum);
            _cdAssignment.ActivateCDHUD(playerNum);
            three_label = labels[0];
            two_label = labels[1];
            one_label = labels[2];
            go_label = labels[3];
            InitialColorAdjustment();
        }
    }

    private void OnMappingToWaitForPlayer()
    {
        switch (GameManager.S.maxPlayerCount)
        {
            case 2:
                twoPlayerMode.SetActive(true);
                _waitModule = twoPlayerMode.GetComponent<WaitForPlayer>();
                threePlayerMode.SetActive(false);
                fourPlayerMode.SetActive(false);
                break;
            case 3:
                twoPlayerMode.SetActive(false);
                threePlayerMode.SetActive(true);
                _waitModule = threePlayerMode.GetComponent<WaitForPlayer>();
                fourPlayerMode.SetActive(false);
                break;
            case 4:
                twoPlayerMode.SetActive(false);
                threePlayerMode.SetActive(false);
                fourPlayerMode.SetActive(true);
                _waitModule = fourPlayerMode.GetComponent<WaitForPlayer>();
                break;
            default:
                break;
        }
    }


    #endregion

    #region WaitForPlayer, Codes for Join UI

    private void CheckPlayerNum()
    {
        if (GameManager.S.readyPlayer == GameManager.S.maxPlayerCount)
        {
            if (!hasStartedCountdown)
            {
                GameManager.S.gameState = GameManager.State.Countdown;
                hasStartedCountdown = true;
                StartCoroutine(Countdown());
            }
        }
    }

    public void PlayerJoin(int playerID)
    {
        _waitModule.Join(playerID);
    }

    public void PlayerReady(int playerID)
    {
        _waitModule.Ready(playerID);
    }

    public void ChangePlayerRank(Sprite rank, int playerID)
    {
        _playerHUDs[playerID].ChangePlayerRank(rank);
    }

    #endregion

    #region Countdown, Codes for countdown
    
    private IEnumerator Countdown()
    {
        GameManager.S.gameState = GameManager.State.Countdown;
        countdownPage.SetActive(true);
        three_label.SetActive(true);
        two_label.SetActive(false);
        one_label.SetActive(false);
        go_label.SetActive(false);
        yield return new WaitForSeconds(shortTime);
        three_label.SetActive(false);
        two_label.SetActive(true);
        yield return new WaitForSeconds(shortTime);
        two_label.SetActive(false);
        one_label.SetActive(true);
        yield return new WaitForSeconds(longTime);
        one_label.SetActive(false);
        go_label.SetActive(true);
        yield return new WaitForSeconds(shortTime);
        GameManager.S.gameState = GameManager.State.GameStart;
        GameManager.S.gameStartTime = Time.time;
        Debug.Log("Start time is " + GameManager.S.gameStartTime);
        foreach (PlayerHUD playerHUD in _playerHUDs)
        {
            playerHUD.gameObject.SetActive(true);
        }
    }

    #endregion

    #region GameStart, Codes for Player HUD

    private void DisableHUD()
    {
        foreach (PlayerHUD playerHUD in _playerHUDs)
        {
            playerHUD.gameObject.SetActive(false);
        }
    }

    void InitialColorAdjustment()
    {
        foreach (var t in _playerHUDs)
        {
            t.InitialColorAdjustment(true);
        }
    }

    public void SetItemAvailability(int playerID, bool available)
    {
        _playerHUDs[playerID].isItemAvailable = available;
    }

    public void UpdateCount(int playerID, int count, bool farting)
    {
        _playerHUDs[playerID].UpdateLettuceCounter(count, farting);
    }

    public void RefreshHUDColor(int playerID, bool isLeft)
    {
        _playerHUDs[playerID].InitialColorAdjustment(isLeft);
    }

    public void MoveLeft(int playerID, bool isMoving)
    {
        _playerHUDs[playerID].MoveLeft(isMoving);
    }

    public void ChangeLeftArmColor(bool enabled, bool active, int playerID)
    {
        _playerHUDs[playerID].ChangeLeftArmColor(enabled, active);
    }

    public void ChangeRightArmColor(bool enabled, bool active, int playerID)
    {
        _playerHUDs[playerID].ChangeRightArmColor(enabled, active);
    }

    public void ChangeLeftLegColor(bool enabled, bool active, int playerID)
    {
        _playerHUDs[playerID].ChangeLeftLegColor(enabled, active);
    }

    public void ChangeRightLegColor(bool enabled, bool active, int playerID)
    {
        _playerHUDs[playerID].ChangeRightLegColor(enabled, active);
    }

    #endregion

    #region GameEnd, Codes for Leaderboard UI

    public void PlayerReachTheEnd(int playerID) 
    {
        _playerHUDs[playerID].ShowEOG(GameManager.S.finishedPlayer - 1);
    }

    public void GameEnds()
    {
        // todo 
        _leaderboard.UpdateLeaderboard();
        return;
    }
    #endregion
    
}
