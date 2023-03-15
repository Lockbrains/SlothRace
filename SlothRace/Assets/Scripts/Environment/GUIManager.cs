using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    public static GUIManager S;

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

    [Header("Reverse Count")] 
    [SerializeField] private GameObject reverseCount;
    [SerializeField] private GameObject three_label;
    [SerializeField] private GameObject two_label;
    [SerializeField] private GameObject one_label;
    [SerializeField] private GameObject go_label;
    private bool hasStartedReverseCount;
    
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
        hasStartedReverseCount = false;
        DisableHUD();
        DisableReverseCount();
        DisableLeft(0);
        DisableLeft(1);
        InitialColorAdjustment();
    }
    
    // Update is called once per frame
    void Update()
    {
        CheckPlayerNum();
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

        
    private void CheckPlayerNum()
    {
        if (GameManager.S.playerNum == 0)
        {
            GameManager.S.gameState = GameManager.State.TitleScreen;
            img_titleScreen.SetActive(true);
            img_waitingForPlayer2.SetActive(false);
            DisableHUD();
        } else if (GameManager.S.playerNum == 1)
        {
            GameManager.S.gameState = GameManager.State.PlayerJoin;
            img_titleScreen.SetActive(false);
            img_waitingForPlayer2.SetActive(true);
            DisableHUD();
        }
        else 
        {
            img_waitingForPlayer2.SetActive(false);
            if (!hasStartedReverseCount)
            {
                hasStartedReverseCount = true;
                StartCoroutine(ReverseCount());
            }
        }
    }
    private void DisableHUD()
    {
        player1HUD.SetActive(false);
        player2HUD.SetActive(false);
    }

    private void DisableReverseCount()
    {
        reverseCount.SetActive(false);
    }
    
    private IEnumerator ReverseCount()
    {
        GameManager.S.gameState = GameManager.State.ReverseCount;
        reverseCount.SetActive(true);
        three_label.SetActive(true);
        two_label.SetActive(false);
        one_label.SetActive(false);
        go_label.SetActive(false);
        yield return new WaitForSeconds(4.0f);
        three_label.SetActive(false);
        two_label.SetActive(true);
        yield return new WaitForSeconds(4.0f);
        two_label.SetActive(false);
        one_label.SetActive(true);
        yield return new WaitForSeconds(6.0f);
        one_label.SetActive(false);
        go_label.SetActive(true);
        yield return new WaitForSeconds(3.0f); 
        GameManager.S.gameState = GameManager.State.GameStart;
        player1HUD.SetActive(true);
        player2HUD.SetActive(true);
        DisableReverseCount();
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
    
    
}
