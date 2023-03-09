using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [Header("Input Data")]
    [SerializeField] private int playerID;
    
    [Header("Player Properties")]
    [Tooltip("Normal speed")]
    public float movementSpeed = 10;

    [Tooltip("Rotation speed if not instant")]
    public float rotationSpeed = 100;

    [Tooltip("Sprint speed")]
    public float sprintSpeed = 10.0f;

    [Tooltip("Look/rotation sensitivity for first person movement")]
    public float lookSensitivity = 5.0f;

    [Tooltip("The height the player can jump")]
    public float jumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float gravity = -15.0f;

    [Tooltip("The angle the player can look up and down")]
    public float upDownRange = 60.0f;

    public bool isMovingLeft;
    //the analog values read from the controller
    public Vector2 leftStick = Vector2.zero;
    public Vector2 rightStick = Vector2.zero;

    public bool leftLeg, leftArm, rightLeg, rightArm;

    //a complex component that facilitates the control of a character
    public CharacterController controller;

    public PlayerInput playerInput;

    public float animationSpeed = 1;
    public float animationSprintSpeed = 2;

    // private bool sprinting = false;
    private float verticalVelocity = 0;



    // Sloth Animation
    public Animator slothAnimator;

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
        if (playerID == 0) GameManager.S.player1 = this.gameObject;
        else GameManager.S.player2 = this.gameObject;
    }

    void Update()
    {
        SlothMovement();
        if (isMovingLeft)
        {
            slothAnimator.SetBool("MoveLeft", true);
        }
        else
        {
            slothAnimator.SetBool("MoveLeft", false);
        }
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
        if (context.started) leftArm = true;
        if (context.canceled) leftArm = false;
    }

    public void OnMoveRightLeg(InputAction.CallbackContext context)
    {
        if (context.started) rightLeg = true;
        if (context.canceled) rightLeg = false;
    }
    
    public void OnMoveLeftLeg(InputAction.CallbackContext context)
    {
        if (context.started) leftLeg = true;
        if (context.canceled) leftLeg = false;
    }

    public void OnMoveRightArm(InputAction.CallbackContext context)
    {
        if (context.started) rightArm = true;
        if (context.canceled) rightArm = false;
    }
    

}
