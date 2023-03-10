using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{

    public static GUIManager S;
    
    [Header("Player 1 HUD")] 
    public Text titlePlayer1Left;
    public Text titlePlayer1Right;
    public Text leftArm1, rightLeg1, rightArm1, leftLeg1;
    public Scrollbar leftScrollbar1, rightScrollbar1;

    [Header("Player 2 HUD")] 
    public Text titlePlayer2Left;
    public Text titlePlayer2Right;
    public Text leftArm2, rightLeg2, rightArm2, leftLeg2;
    public Scrollbar leftScrollbar2, rightScrollbar2;
    
    [Header("UI Setting")] 
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;
    [SerializeField] private Color activeColor;

    [HideInInspector] public Animator player1Anim;
    [HideInInspector] public Animator player2Anim;

    [HideInInspector] public bool isMovingLeft1, isMovingLeft2;
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
        DisableLeft(0);
        DisableLeft(1);
        InitialColorAdjustment();
    }

    // Update is called once per frame
    void Update()
    {
        if (player1Anim != null)
        {
            if (isMovingLeft1)
            {
                leftScrollbar1.size = player1Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                rightScrollbar1.size = 0;
            }
            else
            {
                rightScrollbar1.size = player1Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                leftScrollbar1.size = 0;
            }
        }

        if (player2Anim != null)
        {
            if (isMovingLeft2)
            {
                leftScrollbar2.size = player1Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                rightScrollbar2.size = 0;
            }
            else
            {
                rightScrollbar2.size = player1Anim.GetCurrentAnimatorStateInfo(0).normalizedTime;
                leftScrollbar2.size = 0;
            }
        }
    }

    void InitialColorAdjustment()
    {
        leftArm1.color = disableColor;
        rightArm1.color = enableColor;
        leftLeg1.color = enableColor;
        rightLeg1.color = disableColor;
        leftArm2.color = disableColor;
        rightArm2.color = enableColor;
        leftLeg2.color = enableColor;
        rightLeg2.color = disableColor;
    }
    public void EnableLeft(int playerIndex)
    {
        if (playerIndex == 0)
        {
            titlePlayer1Left.color = enableColor;
            titlePlayer1Right.color = disableColor;
            rightScrollbar1.enabled = false;
            leftScrollbar1.enabled = true;
        }
        else
        {
            titlePlayer2Left.color = enableColor;
            titlePlayer2Right.color = disableColor;
            rightScrollbar2.enabled = false;
            leftScrollbar2.enabled = true;
        }
    }

    public void DisableLeft(int playerIndex)
    {
        if (playerIndex == 0)
        {
            titlePlayer1Left.color = disableColor;
            titlePlayer1Right.color = enableColor;
            rightScrollbar1.enabled = true;
            leftScrollbar1.enabled = false;
        }
        else
        {
            titlePlayer2Left.color = disableColor;
            titlePlayer2Right.color = enableColor;
            rightScrollbar2.enabled = true;
            leftScrollbar2.enabled = false;
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
                leftArm1.color = disableColor;
            }
            else
            {
                leftArm2.color = disableColor;
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
                rightArm1.color = disableColor;
            }
            else
            {
                rightArm2.color = disableColor;
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
                leftLeg1.color = disableColor;
            }
            else
            {
                leftLeg2.color = disableColor;
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
                rightLeg1.color = disableColor;
            }
            else
            {
                rightLeg2.color = disableColor;
            }
        }
    }
    
    
}
