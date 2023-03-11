using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input Data")]
    [SerializeField] private int playerID;
    [SerializeField] private Sloth _sloth;

    [Header("Player Properties")] 
    public float movementSpeed = 10;
    public float rotationSpeed = 100;
  
    [Header("Player Status")]
    public bool isMovingLeft;
    public bool isAttacking;
    public bool leftLeg, leftArm, rightLeg, rightArm;

    //the analog values read from the controller
    
    [Header("Analog Value")]
    public Vector2 leftStick = Vector2.zero;
    public Vector2 rightStick = Vector2.zero;
    
    [Header("Controllers and Animation")]
    //a complex component that facilitates the control of a character
    public CharacterController controller;
    // Sloth Animation
    public Animator slothAnimator;
    
    public PlayerInput playerInput;
    
    // private bool sprinting = false;
    private float verticalVelocity = 0;

    private void Awake()
    {
       
    }

    private void Start()
    {
        controller = GetComponent<CharacterController>();

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        
        playerID = playerInput.playerIndex;
        if (playerID == 0)
        {
            GameManager.S.player1 = this.gameObject;
            GUIManager.S.player1Anim = slothAnimator;
        }
        else
        {
            GameManager.S.player2 = this.gameObject;
            GUIManager.S.player2Anim = slothAnimator;
        }

        GameManager.S.playerNum++;
    }

    void Update()
    {
        if (!_sloth.isAttacking) SlothMovement();
        else SlothAttack();
        SetAnimation();
        SetPlayerStatusInHUD();
    }

    private void SetAnimation()
    {
        slothAnimator.SetBool("MoveLeft", isMovingLeft);
        slothAnimator.SetBool("Attacking", isAttacking);
    }

    private void SetPlayerStatusInHUD()
    {
        if (playerID == 0)
        {
            GUIManager.S.isMovingLeft1 = isMovingLeft;
        }
        else
        {
            GUIManager.S.isMovingLeft2 = isMovingLeft;
        }
    }
    
    void SlothAttack()
    {
        slothAnimator.speed = 1;
    }
    
    void SlothMovement()
    {
        float targetSpeed = movementSpeed;
        Vector3 movement = new Vector3(leftStick.x * targetSpeed, verticalVelocity, leftStick.y * targetSpeed) * slothAnimator.speed;

        //since it's in update and continuous the vector has to be multiplied by Time.deltaTime to be frame independent
        controller.Move(movement * Time.deltaTime);
        
        // use the left joystick to control the direction
        if (leftStick.magnitude > 0.1f)
        {
            transform.Rotate(0, leftStick.x * Time.deltaTime * rotationSpeed, 0) ;
        }
        
        // if the left joystick is at its original position, stop the animation
        if (leftStick.magnitude == 0f) slothAnimator.speed = 0;
        else
        {
            // left joystick moving, the player tries to move
            if (isMovingLeft)
            {
                GUIManager.S.EnableLeft(playerID);
                if (leftArm && rightLeg)
                {
                    slothAnimator.speed = 1;
                }
                else
                {
                    slothAnimator.speed = 0;
                }
            }
            else
            {
                GUIManager.S.DisableLeft(playerID);
                if (leftLeg && rightArm)
                {
                    slothAnimator.speed = 1;
                }
                else
                {
                    slothAnimator.speed = 0;
                }
            }
            
        }


    }
    

    /*
     * Every time an Input Action fires, the Player Input Component will trigger functions 
     * on the same game object that match the name of that Action, prefixed with n
     */

    // this is the proper way to name an input, after the action so it can be remapped for different devices

    //this is a less proper naming but more intuitive if you are used to just check an axis
    public void OnLeftStickMove(InputAction.CallbackContext context)
    {
        leftStick = context.ReadValue<Vector2>();
        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;
    }

    public void OnRightStickMove(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
    }

    public void OnMoveLeftArm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftArm = true;
            //SoundManager.S.LaunchMove();
        }

        if (context.canceled)
        {
            leftArm = false;
        }
        
        GUIManager.S.ChangeLeftArmColor(isMovingLeft, leftArm, playerID);
    }

    public void OnMoveRightLeg(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightLeg = true;
            //SoundManager.S.LaunchMove();
        }

        if (context.canceled)
        {
            rightLeg = false;
        }
        GUIManager.S.ChangeRightLegColor(isMovingLeft, rightLeg, playerID);
    }
    
    public void OnMoveLeftLeg(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftLeg = true;
            //SoundManager.S.LaunchMove();
        }

        if (context.canceled)
        {
            leftLeg = false;
        }
        GUIManager.S.ChangeLeftLegColor(!isMovingLeft, leftLeg, playerID);

    }

    public void OnMoveRightArm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightArm = true;
            //SoundManager.S.LaunchMove();
        }

        if (context.canceled)
        {
            rightArm = false;
        }
        
        GUIManager.S.ChangeRightArmColor(!isMovingLeft, rightArm, playerID);

    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isAttacking = true;
        }
    }
    

}
