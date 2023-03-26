using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHUD : MonoBehaviour
{
    [Header("UI Objects")] 
    [SerializeField] private Image rank;
    [SerializeField] private Image leftArm, rightLeg, rightArm, leftLeg;
    [SerializeField] private Text t_leftArm, t_rightLeg, t_rightArm, t_leftLeg;
    [SerializeField] private Scrollbar progress;
    [SerializeField] private GameObject leftPress, rightPress;
    [SerializeField] private Image item;
    private bool isMovingLeft = false;
    
    [Header("UI Settings")]
    [SerializeField] private Color enableColor;
    [SerializeField] private Color disableColor;
    [SerializeField] private Color limbDisableColor;
    [SerializeField] private Color activeColor;

    [Header("Font Colors")] 
    [SerializeField] private Color lightFontColor;
    [SerializeField] private Color darkFontColor;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (leftArm != null) t_leftArm.color = (leftArm.color == enableColor) ? darkFontColor : lightFontColor;
        if (rightArm != null) t_rightArm.color = (rightArm.color == enableColor) ? darkFontColor : lightFontColor;
        if (leftLeg != null) t_leftLeg.color = (leftLeg.color == enableColor) ? darkFontColor : lightFontColor;
        if (rightLeg != null) t_rightLeg.color = (rightLeg.color == enableColor) ? darkFontColor : lightFontColor;
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

    public void InitialColorAdjustment()
    {
        leftArm.color = limbDisableColor;
        rightArm.color = enableColor;
        leftLeg.color = enableColor;
        rightLeg.color = limbDisableColor;
    }
    public void Left()
    {
        leftPress.SetActive(true);
        rightPress.SetActive(false);
    }

    public void Right()
    {
        leftPress.SetActive(false);
        rightPress.SetActive(true);
    }

    public void ChangeLeftArmColor(bool enabled, bool active)
    {
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
        if (enabled)
        {
            rightLeg.color = active ? activeColor : enableColor;
        }
        else
        {
            rightLeg.color = limbDisableColor;
        }
    }
}
