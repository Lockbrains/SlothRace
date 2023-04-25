using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI Objects")] 
    [SerializeField] private Image rank;
    [SerializeField] private Image leftArm, rightLeg, rightArm, leftLeg;
    [SerializeField] private Text t_leftArm, t_rightLeg, t_rightArm, t_leftLeg;
    [SerializeField] private Scrollbar progress;
    
    [Header("Lettuce Count")]
    [SerializeField] private Image[] lettuces;
    [SerializeField] private Sprite availableLettuce;
    [SerializeField] private Sprite disableLettuce;
    [SerializeField] private Image fartIcon;
    [SerializeField] private Image poopIcon;
    [SerializeField] private Sprite availableFart;
    [SerializeField] private Sprite disableFart;
    [SerializeField] private Sprite availablePoop;
    [SerializeField] private Sprite disablePoop;
    [SerializeField] private Image fartText;
    [SerializeField] private Image poopText;
    [SerializeField] private Sprite availableFartText;
    [SerializeField] private Sprite disableFartText;
    [SerializeField] private Sprite availablePoopText;
    [SerializeField] private Sprite disablePoopText;
    [SerializeField] private Image fartButton;
    [SerializeField] private Sprite availableFartButton;
    [SerializeField] private Sprite disableFartButton;
    [SerializeField] private Image poopButton;
    [SerializeField] private Sprite availablePoopButton;
    [SerializeField] private Sprite disablePoopButton;
    [SerializeField] private Image poopFire;
    [SerializeField] private Sprite availablePoopFire;
    [SerializeField] private Sprite disablePoopFire;
    [SerializeField] private GameObject pressHolder;
    [SerializeField] private GameObject holdHolder;
    [SerializeField] private GameObject lettuceCounter;
    private bool isMovingLeft = false;
    
    [Header("UI Settings")]
    [SerializeField] private Color enableColor;
    [SerializeField] private Color itemDisableColor;
    [SerializeField] private Color limbDisableColor;
    [SerializeField] private Color activeColor;

    [Header("Font Colors")] 
    [SerializeField] private Color lightFontColor;
    [SerializeField] private Color darkFontColor;

    [Header("Limb Highlights")] 
    [SerializeField] private GameObject leftArmGO;
    [SerializeField] private GameObject rightArmGO;
    [SerializeField] private GameObject leftLegGO;
    [SerializeField] private GameObject rightLegGO;

    [Header("End Of Game")] 
    [SerializeField] private Image endRankSprite;
    [SerializeField] private Sprite[] rankSprites;
    [SerializeField] private GameObject endRank;

    public bool isItemAvailable;

    // Start is called before the first frame update
    void Start()
    {
        isItemAvailable = false;
        endRank.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (leftArm != null)
        {
            if (leftArm.color == limbDisableColor)
            {
                t_leftArm.color = Color.clear;
            }
            else
            {
                t_leftArm.color = (leftArm.color == enableColor) ? darkFontColor : lightFontColor;
            }
        }

        if (rightArm != null)
        {
            if (rightArm.color == limbDisableColor)
            {
                t_rightArm.color = Color.clear;
            }
            else
            {
                t_rightArm.color = (rightArm.color == enableColor) ? darkFontColor : lightFontColor;
            }
        }

        if (leftLeg != null)
        {
            if (leftLeg.color == limbDisableColor)
            {
                t_leftLeg.color = Color.clear;
            }
            else
            {
                t_leftLeg.color = (leftLeg.color == enableColor) ? darkFontColor : lightFontColor;
            }
        }

        if (rightLeg != null)
        {
            if (rightLeg.color == limbDisableColor)
            {
                t_rightLeg.color = Color.clear;
            }
            else
            {
                t_rightLeg.color = (rightLeg.color == enableColor) ? darkFontColor : lightFontColor;
            }
        }
    }
    

    public void MoveLeft(bool isMoving)
    {
        isMovingLeft = isMoving;
    }
    public void UpdateProgress(Animator animator)
    {
        float TOLERANCE = 0.0001f;
        if (animator!= null)
        {
            if (isMovingLeft)
            {
                float size = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                progress.size = size;
            }
            else
            {
                float size = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                progress.size = size;
            }
            
            if (Mathf.Abs(progress.size - 1) < TOLERANCE) 
                progress.size = 0;
        }
        
    }

    public void InitialColorAdjustment(bool left)
    {
        if (left)
        {
            leftArm.color = limbDisableColor;
            rightArm.color = enableColor;
            leftLeg.color = enableColor;
            rightLeg.color = limbDisableColor;
        }
        else
        {
            leftArm.color = enableColor;
            rightArm.color = limbDisableColor;
            leftLeg.color = limbDisableColor;
            rightLeg.color = enableColor;
        }
    }

    public void UpdateLettuceCounter(int count, bool farting)
    {
        for (int i = 0; i < 5; i++)
        {
            lettuces[i].sprite = i < count ? availableLettuce : disableLettuce;
        }

        fartIcon.sprite = count is >= 2 && !farting ? availableFart : disableFart;
        fartText.sprite = count is >= 2 && !farting ? availableFartText : disableFartText;
        fartButton.sprite = count is >= 2 && !farting ? availableFartButton : disableFartButton;
        fartButton.enabled = count is >= 2 && !farting;
        pressHolder.SetActive(count >= 2 && !farting);

        poopIcon.sprite = count >= 5 ? availablePoop : disablePoop;
        poopText.sprite = count >= 5 ? availablePoopText : disablePoopText;
        poopButton.sprite = count >= 5 ? availablePoopButton : disablePoopButton;
        poopButton.enabled = count >= 5;
        poopFire.sprite = count >= 5 ? availablePoopFire : disablePoopFire;
        holdHolder.SetActive(count >= 5);
    }

    public void ChangeLeftArmColor(bool enabled, bool active)
    {
        leftArmGO.SetActive(active);
        if (enabled)
        {
            leftArm.color = active ? activeColor : enableColor;
            
        }
        else
        {
            leftArm.color = limbDisableColor;
            
        }
    }
    
    public void ChangeRightArmColor(bool enabled, bool active)
    {
        rightArmGO.SetActive(active);
        if (enabled)
        {
            rightArm.color = active ? activeColor : enableColor;
            
        }
        else
        {
            rightArm.color = limbDisableColor;
        }
    }
    
    public void ChangeLeftLegColor(bool enabled, bool active)
    {
        leftLegGO.SetActive(active);
        if (enabled)
        {
            leftLeg.color = active ? activeColor : enableColor;
            
        }
        else
        {
            leftLeg.color = limbDisableColor;
        }
    }
    
    public void ChangeRightLegColor(bool enabled, bool active)
    {
        rightLegGO.SetActive(active);
        if (enabled)
        {
            rightLeg.color = active ? activeColor : enableColor;
            
        }
        else
        {
            rightLeg.color = limbDisableColor;
        }
    }

    public void ChangePlayerRank(Sprite rankImg)
    {
        rank.sprite = rankImg;
    }

    public void ShowEOG(int playerRank)
    {
        // turn on the EOG elements
        endRank.SetActive(true);
        endRankSprite.sprite = rankSprites[playerRank];
        
        // turn off others except for the control ui
        lettuceCounter.SetActive(false);
        rank.gameObject.SetActive(false);
    }
}
