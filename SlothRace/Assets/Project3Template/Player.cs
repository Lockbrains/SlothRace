using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Input Data")]
    [SerializeField] private int playerID;
    [SerializeField] private Sloth _sloth;
    [SerializeField] private GameObject _physicalSloth;

    [Header("Player Properties")] 
    public float movementSpeed = 0.4f;
    public float maxMovementSpeed = 2;
    public float camRotationSpeed = 100;
    public float playerRotationSpeed = 2;
    public float speedBoostTime = 4f;

    public float animatorSpeed = 1;
    public float maxAnimatorSpeed = 2;

    [Header("Player Status")]
    public bool isMovingLeft;
    public bool isAttacking;
    public bool leftLeg, leftArm, rightLeg, rightArm;
    public bool speedBoost;

    [Header("Player Abilities")]
    public Stack<string> playerAbilities = new Stack<string>();

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

    public GameObject slothCamera;

    public GameObject player;
    
    // private bool sprinting = false;
    private float verticalVelocity = 0;

    private void Awake()
    {
       
    }

    private void Start()
    {
        //controller = GetComponent<CharacterController>();

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

        GameManager.S.joinedPlayer++;
        slothAnimator.speed = 0;
    }

    void Update()
    {
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            SlothMovement();
        }
        SetAnimation();
        SetPlayerStatusInHUD();
    }

    public int GetPlayerID()
    {
        return playerID;
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
    
    
    void SlothMovement()
    {
        float targetSpeed = movementSpeed;
        Vector3 movement = new Vector3(leftStick.x * targetSpeed, verticalVelocity, leftStick.y * targetSpeed) * slothAnimator.speed;

        //since it's in update and continuous the vector has to be multiplied by Time.deltaTime to be frame independent
        Vector3 curPos = transform.position;
        curPos += movement * Time.deltaTime;
        transform.position = curPos;
        
        // Debug.Log(leftStick);
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
                    slothAnimator.speed = animatorSpeed;
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
                    slothAnimator.speed = animatorSpeed;
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
        if (GameManager.S.maxPlayerCount == 2)
        {
            leftStick = context.ReadValue<Vector2>();
        }

        float inputValue = context.ReadValue<Vector2>().x;
        float rotation = inputValue * playerRotationSpeed + player.transform.rotation.eulerAngles.y;
        Vector3 playerEulerAngles = player.transform.rotation.eulerAngles;
        playerEulerAngles.y = rotation;
        player.transform.rotation = Quaternion.Euler(playerEulerAngles);

        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;
    }

    public void OnRightStickMove(InputAction.CallbackContext context)
    {
        float inputValue = context.ReadValue<Vector2>().x;
        float rotation = inputValue * camRotationSpeed + slothCamera.transform.rotation.eulerAngles.y;

        slothCamera.transform.rotation = Quaternion.Euler(21.35f, rotation, 0f);
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
        
        else if (context.canceled)
        {
            isAttacking = false;
        }
    }

    public void OnSpeedBoost(InputAction.CallbackContext context)
    {
        if (context.started && playerAbilities.Count != 0 && playerAbilities.Peek() == "SpeedBoost")
        {
            StartCoroutine(StartSpeedBoost(speedBoostTime));
            playerAbilities.Pop();
        }
    }

    private IEnumerator StartSpeedBoost(float waitTime)
    {
        SpeedBoost();
        yield return new WaitForSeconds(waitTime);
        ResetSpeed();
        
    }

    private void SpeedBoost()
    {
        animatorSpeed = maxAnimatorSpeed;
        Debug.Log("speeding");
        movementSpeed = maxMovementSpeed;
        speedBoost = true;
    }

    private void ResetSpeed()
    {
        animatorSpeed = 1;
        slothAnimator.speed = 1;
        movementSpeed = 0.4f;
        speedBoost = false;
        Debug.Log("slow down");
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(GameManager.S.gameState == GameManager.State.GameEnd)
        {
            SceneManager.LoadScene("FirstLevel");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Car"))
        {
            Debug.Log("Collided!");
            GameManager.S.SendPlayerToOrigin(playerID);
        }
        
    }

    public void ResetPosition()
    {
        GameManager.S.SendPlayerToOrigin(playerID);
        GameObject ragdoll = transform.Find("PhysicalSloth").gameObject;
        GameObject hips = ragdoll.transform.Find("mixamorig:Hips").gameObject;
        Debug.Log("hips:" + hips);
        hips.GetComponent<ResetPosition>().resetPosition();
        Transform[] allJoints = hips.GetComponentsInChildren<Transform>();
        foreach (Transform child in allJoints)
        {
            child.gameObject.GetComponent<ResetPosition>().resetPosition();
        }
    }

}
