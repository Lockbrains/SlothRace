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

    public static GUIManager S;
    private GameManager.State _state;
    
    [Header("State UI")] 
    [SerializeField] private GameObject titlePage;
    [SerializeField] private GameObject levelSelectionPage;
    [SerializeField] private GameObject waitForPlayerPage;
    [SerializeField] private GameObject countdownPage;
    [SerializeField] private GameObject inGameHUD;
    [SerializeField] private GameObject leaderboardPage;

    [Header("UI GameObjects")]
    public GameObject img_titleScreen;
    public GameObject img_waitingForPlayer2;
    
    [Header("Player HUD")]
    [SerializeField] private PlayerHUD[] _playerHUDs;
    
    [Header("UI Setting")] 
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;
    [SerializeField] private Color limbDisableColor;
    [SerializeField] private Color activeColor;
    [HideInInspector] public Animator player1Anim;
    [HideInInspector] public Animator player2Anim;
    [HideInInspector] public Animator player3Anim;
    [HideInInspector] public Animator player4Anim;

    [Header("Wait For Players")] 
    [SerializeField] private GameObject twoPlayerMode;
    [SerializeField] private GameObject threePlayerMode;
    [SerializeField] private GameObject fourPlayerMode;
    [SerializeField] private GameObject allSetIcon;
    [HideInInspector] public bool allSet;
    private WaitForPlayer _waitModule;
    
    [Header("Countdown")] 
    [SerializeField] private GameObject reverseCount;
    [SerializeField] private GameObject three_label;
    [SerializeField] private GameObject two_label;
    [SerializeField] private GameObject one_label;
    [SerializeField] private GameObject go_label;
    [SerializeField] private float shortTime;
    [SerializeField] private float longTime;
    private bool hasStartedCountdown;

    [Header("Game Over")]
    [SerializeField] private GameObject player1Wins;
    [SerializeField] private GameObject player2Wins;
    
    
    private void Awake()
    {
        if (S)
        {
            Destroy(S.gameObject);
        }

        S = this;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        hasStartedCountdown = false;
        DisableCountdown();
        DisableLeft(0);
        DisableLeft(1);
        InitialColorAdjustment();
    }
    
    // Update is called once per frame
    void Update()
    {
        UpdateHUD();
        UpdateListenToInput();
        UpdatePlayerScrollBar();
    }
    
    private void UpdateHUD()
    {
        _state = GameManager.S.gameState;
        switch (_state)
        {   
            case GameManager.State.TitleScreen:
                titlePage.SetActive(true);
                levelSelectionPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.LevelSelection:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(true);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.WaitForPlayers:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
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
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(true);
                inGameHUD.SetActive(false);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.GameStart:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
                waitForPlayerPage.SetActive(false);
                countdownPage.SetActive(false);
                inGameHUD.SetActive(true);
                leaderboardPage.SetActive(false);
                break;
            case GameManager.State.GameEnd:
                titlePage.SetActive(false);
                levelSelectionPage.SetActive(false);
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
            if(len >=3) _playerHUDs[2].UpdateProgress(player3Anim);
            if(len >=4) _playerHUDs[3].UpdateProgress(player4Anim);
        }
    }
        
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
    
    private void DisableHUD()
    {
        foreach (PlayerHUD playerHUD in _playerHUDs)
        {
            playerHUD.gameObject.SetActive(false);
        }
    }

    private void DisableCountdown()
    {
        reverseCount.SetActive(false);
    }
    
    //todo: to be updated into PlayerHUD
    public void PlayerWins(int playerID)
    {
        if (playerID == 1)
        {
            DisableHUD();
            player1Wins.SetActive(true);
            player2Wins.SetActive(false);
        }
        else
        {
            DisableHUD();
            player1Wins.SetActive(false);
            player2Wins.SetActive(true);
        }
    } 

    private IEnumerator Countdown()
    {
        GameManager.S.gameState = GameManager.State.Countdown;
        countdownPage.SetActive(true);
        reverseCount.SetActive(true);
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
        foreach (PlayerHUD playerHUD in _playerHUDs)
        {
            playerHUD.gameObject.SetActive(true);
        }
        DisableCountdown();
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
    
    public void RefreshHUDColor(int playerID, bool isLeft)
    {
        _playerHUDs[playerID].InitialColorAdjustment(isLeft);
    }
    public void EnableLeft(int playerID)
    {
        _playerHUDs[playerID].Left();
    }
    public void DisableLeft(int playerID)
    {
        _playerHUDs[playerID].Right();
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

    public void OnLevelSelectButtonClick(int playerNum)
    {
        if (GameManager.S.gameState == GameManager.State.LevelSelection)
        {
            GameManager.S.maxPlayerCount = playerNum;
            GameManager.S.gameState = GameManager.State.WaitForPlayers;
            switch (playerNum)
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
    }

    public void PlayerJoin(int playerID)
    {
        _waitModule.Join(playerID);
    }

    public void PlayerReady(int playerID)
    {
        _waitModule.Ready(playerID);
    }
    
}
