using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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
    
    [Header("Player 1 HUD")] 
    public GameObject player1HUD;
    public Image titlePlayer1Left;
    public Image titlePlayer1Right;
    public Image leftArm1, rightLeg1, rightArm1, leftLeg1;
    public Scrollbar leftScrollbar1, rightScrollbar1;

    [Header("Player 2 HUD")] 
    public GameObject player2HUD;
    public Image titlePlayer2Left;
    public Image titlePlayer2Right;
    public Image leftArm2, rightLeg2, rightArm2, leftLeg2;
    public Scrollbar leftScrollbar2, rightScrollbar2;
    
    [Header("UI Setting")] 
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;
    [SerializeField] private Color limbDisableColor;
    [SerializeField] private Color activeColor;
    [HideInInspector] public Animator player1Anim;
    [HideInInspector] public Animator player2Anim;
    [HideInInspector] public bool isMovingLeft1, isMovingLeft2;

    [Header("Wait For Players")] 
    [SerializeField] private GameObject twoPlayerMode;
    [SerializeField] private GameObject threePlayerMode;
    [SerializeField] private GameObject fourPlayerMode;
    [SerializeField] private GameObject allSetIcon;
    [HideInInspector] public bool allSet;
    
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

    private void ShowHUD(GameObject HUD)
    {
        titlePage.SetActive(false);
        levelSelectionPage.SetActive(false);
        waitForPlayerPage.SetActive(false);
        countdownPage.SetActive(false);
        inGameHUD.SetActive(false);
        leaderboardPage.SetActive(false);
        
        HUD.SetActive(true);
    }
    
    private void UpdateHUD()
    {
        _state = GameManager.S.gameState;
        switch (_state)
        {   
            case GameManager.State.TitleScreen:
                ShowHUD(titlePage);
                break;
            case GameManager.State.LevelSelection:
                ShowHUD(levelSelectionPage);
                break;
            case GameManager.State.WaitForPlayers:
                ShowHUD(waitForPlayerPage);
                allSetIcon.SetActive(allSet);
                CheckPlayerNum();
                break;
            case GameManager.State.Countdown:
                ShowHUD(countdownPage);
                break;
            case GameManager.State.GameStart:
                ShowHUD(inGameHUD);
                break;
            case GameManager.State.GameEnd:
                ShowHUD(leaderboardPage);
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
            float TOLERANCE = 0.0001f;
            if (player1Anim != null)
            {
                if (isMovingLeft1)
                {
                    float size = player1Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    leftScrollbar1.size = size;
                    rightScrollbar1.size = 0;
                }
                else
                {
                    float size = player1Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    rightScrollbar1.size = size;
                    leftScrollbar1.size = 0;
                }
            }

            if (player2Anim != null)
            {
                if (isMovingLeft2)
                {
                    float size = player2Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    leftScrollbar2.size = size;
                    rightScrollbar2.size = 0;
                }
                else
                {
                    float size = player2Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    rightScrollbar2.size = size;
                    leftScrollbar2.size = 0;
                }
            }

            if (Mathf.Abs(leftScrollbar1.size - 1) < TOLERANCE) leftScrollbar1.size = 0;
            if (Mathf.Abs(leftScrollbar2.size - 1) < TOLERANCE) leftScrollbar2.size = 0;
            if (Mathf.Abs(rightScrollbar1.size - 1) < TOLERANCE) rightScrollbar1.size = 0;
            if (Mathf.Abs(rightScrollbar2.size - 1) < TOLERANCE) rightScrollbar2.size = 0;
        }
    }
        
    private void CheckPlayerNum()
    {
        if (GameManager.S.joinedPlayer == GameManager.S.maxPlayerCount)
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
        player1HUD.SetActive(false);
        player2HUD.SetActive(false);
    }

    private void DisableCountdown()
    {
        reverseCount.SetActive(false);
    }
    
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
        player1HUD.SetActive(true);
        player2HUD.SetActive(true);
        DisableCountdown();
    }
    void InitialColorAdjustment()
    {
        leftArm1.color = limbDisableColor;
        rightArm1.color = enableColor;
        leftLeg1.color = enableColor;
        rightLeg1.color = limbDisableColor;
        leftArm2.color = limbDisableColor;
        rightArm2.color = enableColor;
        leftLeg2.color = enableColor;
        rightLeg2.color = limbDisableColor;
    }
    public void EnableLeft(int playerIndex)
    {
        if (playerIndex == 0)
        {
            titlePlayer1Left.color = enableColor;
            titlePlayer1Right.color = disableColor;
            rightScrollbar1.gameObject.SetActive(false);
            leftScrollbar1.gameObject.SetActive(true);
            leftArm1.gameObject.SetActive(true);
            rightLeg1.gameObject.SetActive(true);
            leftLeg1.gameObject.SetActive(false);
            rightArm1.gameObject.SetActive(false);
            leftScrollbar1.size = 0;
        }
        else
        {
            titlePlayer2Left.color = enableColor;
            titlePlayer2Right.color = disableColor;
            rightScrollbar2.gameObject.SetActive(false);
            leftScrollbar2.gameObject.SetActive(true);
            leftArm2.gameObject.SetActive(true);
            rightLeg2.gameObject.SetActive(true);
            leftLeg2.gameObject.SetActive(false);
            rightArm2.gameObject.SetActive(false);
            leftScrollbar2.size = 0;
        }
    }
    public void DisableLeft(int playerIndex)
    {
        if (playerIndex == 0)
        {
            titlePlayer1Left.color = disableColor;
            titlePlayer1Right.color = enableColor;
            rightScrollbar1.gameObject.SetActive(true);
            leftScrollbar1.gameObject.SetActive(false);
            leftArm1.gameObject.SetActive(false);
            rightLeg1.gameObject.SetActive(false);
            leftLeg1.gameObject.SetActive(true);
            rightArm1.gameObject.SetActive(true);
            rightScrollbar1.size = 0;
        }
        else
        {
            titlePlayer2Left.color = disableColor;
            titlePlayer2Right.color = enableColor;
            rightScrollbar2.gameObject.SetActive(true);
            leftScrollbar2.gameObject.SetActive(false);
            leftArm2.gameObject.SetActive(false);
            rightLeg2.gameObject.SetActive(false);
            leftLeg2.gameObject.SetActive(true);
            rightArm2.gameObject.SetActive(true);
            rightScrollbar2.size = 0;
        }
    }

    public void ChangeLeftArmColor(bool enabled, bool active, int playerID)
    {
        if (enabled)
        {
            if (playerID == 0)
            {
                leftArm1.color = active ? activeColor : enableColor;
            }
            else
            {
                leftArm2.color = active ? activeColor : enableColor;
            }
        }
        else
        {
            if (playerID == 0)
            {
                leftArm1.color = limbDisableColor;
            }
            else
            {
                leftArm2.color = limbDisableColor;
            }
        }
    }

    public void ChangeRightArmColor(bool enabled, bool active, int playerID)
    {
        if (enabled)
        {
            if (playerID == 0)
            {
                rightArm1.color = active ? activeColor : enableColor;
            }
            else
            {
                rightArm2.color = active ? activeColor : enableColor;
            }
        }
        else
        {
            if (playerID == 0)
            {
                rightArm1.color = limbDisableColor;
            }
            else
            {
                rightArm2.color = limbDisableColor;
            }
        }
    }
    
    public void ChangeLeftLegColor(bool enabled, bool active, int playerID)
    {
        if (enabled)
        {
            if (playerID == 0)
            {
                leftLeg1.color = active ? activeColor : enableColor;
            }
            else
            {
                leftLeg2.color = active ? activeColor : enableColor;
            }
        }else
        {
            if (playerID == 0)
            {
                leftLeg1.color = limbDisableColor;
            }
            else
            {
                leftLeg2.color = limbDisableColor;
            }
        }
    }
    
    public void ChangeRightLegColor(bool enabled, bool active, int playerID)
    {
        if (enabled)
        {
            if (playerID == 0)
            {
                rightLeg1.color = active ? activeColor : enableColor;
            }
            else
            {
                rightLeg2.color = active ? activeColor : enableColor;
            }
        }else
        {
            if (playerID == 0)
            {
                rightLeg1.color = limbDisableColor;
            }
            else
            {
                rightLeg2.color = limbDisableColor;
            }
        }
    }

    public void OnLevelSelectButtonClick(int playerNum)
    {
        GameManager.S.maxPlayerCount = playerNum;
        GameManager.S.gameState = GameManager.State.WaitForPlayers;
        switch (playerNum)
        {
            case 2:
                twoPlayerMode.SetActive(true);
                threePlayerMode.SetActive(false);
                fourPlayerMode.SetActive(false);
                break;
            case 3:
                twoPlayerMode.SetActive(false);
                threePlayerMode.SetActive(true);
                fourPlayerMode.SetActive(false);
                break;
            case 4:
                twoPlayerMode.SetActive(false);
                threePlayerMode.SetActive(false);
                fourPlayerMode.SetActive(true);
                break;
            default:
                break;
        }
    }
    
}
