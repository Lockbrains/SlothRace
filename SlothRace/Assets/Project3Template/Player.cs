using System;
using DualSenseSample.Inputs;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.SceneManagement;
using UniSense;
using DualSenseGamepadHID = UniSense.DualSenseGamepadHID;

public class Player : MonoBehaviour
{
    [Header("Input Data")]
    [SerializeField] private int playerID;
    [SerializeField] private Sloth _sloth;
    [SerializeField] private GameObject _physicalSloth;
    private bool _isReadyToGame;

    [Header("Player Properties")] 
    public float movementSpeed = 10;
    public float rotationSpeed = 100;
    private bool _isSwitchingToLeft, _isSwitchingToRight;
  
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
    
    private DualSenseTriggerState leftTriggerState;
    private DualSenseTriggerState rightTriggerState;

    [SerializeField] private AbstractDualSenseBehaviour listener;
    public DualSenseGamepadHID DualSense;

    #region Unity Basics

    private void Awake()
    {
        leftTriggerState = new DualSenseTriggerState
        {
            EffectType = DualSenseTriggerEffectType.ContinuousResistance,
            EffectEx = new DualSenseEffectExProperties(),
            Section = new DualSenseSectionResistanceProperties(),
            Continuous = new DualSenseContinuousResistanceProperties()
        };

        rightTriggerState = new DualSenseTriggerState
        {
            EffectType = DualSenseTriggerEffectType.ContinuousResistance,
            EffectEx = new DualSenseEffectExProperties(),
            Section = new DualSenseSectionResistanceProperties(),
            Continuous = new DualSenseContinuousResistanceProperties()
        };
    }

    private void Start()
    {
        GameManager.S.dualSenseMonitor.listeners[playerID] = listener;
        GameManager.S.dualSenseMonitor.gameObject.SetActive(true);
        //controller = GetComponent<CharacterController>();
        DualSense = listener.DualSense;

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        
        playerID = playerInput.playerIndex;
        GUIManager.S.PlayerJoin(playerID);
        switch (playerID)
        {
            case 0:
                GameManager.S.player1 = this.gameObject;
                GUIManager.S.player1Anim = slothAnimator;
                break;
            case 1:
                GameManager.S.player2 = this.gameObject;
                GUIManager.S.player2Anim = slothAnimator;
                break;
            case 2:
                GameManager.S.player3 = this.gameObject;
                GUIManager.S.player3Anim = slothAnimator;
                break;
            case 3:
                GameManager.S.player4 = this.gameObject;
                GUIManager.S.player4Anim = slothAnimator;
                break;
            default:
                break;
        }

        GameManager.S.joinedPlayer++;
        slothAnimator.speed = 0;
        _isReadyToGame = false;
        _isSwitchingToRight = false;
        _isSwitchingToLeft = false;
    }

    private void CheckDSController()
    {
        var state = new DualSenseGamepadState
        {
            LeftTrigger = leftTriggerState,
            RightTrigger = rightTriggerState
        };
        DualSense?.SetGamepadState(state);
    }
    void Update()
    {
        CheckDSController();
        
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            SlothMovement();
        }
        SetAnimation();
        SetPlayerStatusInHUD();
    }

    #endregion
    

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
        GUIManager.S.MoveLeft(playerID, isMovingLeft);
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
        if (leftStick.magnitude == 0f)
        {
            slothAnimator.speed = 0;
            //Gamepad.current.SetMotorSpeeds(0f, 0f);
        }
        else
        {
            // left joystick moving, the player tries to move
            if (isMovingLeft)
            {
                leftTriggerState.Continuous.StartPosition = (byte)255;
                leftTriggerState.Continuous.Force = (byte)255;
                rightTriggerState.Continuous.StartPosition = (byte)0;
                rightTriggerState.Continuous.Force = (byte)255;
                GUIManager.S.EnableLeft(playerID);
                _isSwitchingToRight = true;
                if (_isSwitchingToLeft)
                {
                    GUIManager.S.RefreshHUDColor(playerID, false);
                    _isSwitchingToLeft = false;
                }
                if (leftArm && rightLeg)
                {
                    slothAnimator.speed = 1;
                    float size = slothAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    Gamepad.current.SetMotorSpeeds(0.01f, 1f);
                }
                else
                {
                    slothAnimator.speed = 0;
                    Gamepad.current.SetMotorSpeeds(0f, 0f);
                }
            }
            else
            {
                GUIManager.S.DisableLeft(playerID);
                _isSwitchingToLeft = true;
                rightTriggerState.Continuous.StartPosition = (byte)255;
                rightTriggerState.Continuous.Force = (byte)255;
                leftTriggerState.Continuous.StartPosition = (byte)0;
                leftTriggerState.Continuous.Force = (byte)255;
                if (_isSwitchingToRight)
                {
                    GUIManager.S.RefreshHUDColor(playerID, true);
                    _isSwitchingToRight = false;
                }
                if (leftLeg && rightArm)
                {
                    slothAnimator.speed = 1;
                    float size = slothAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    Gamepad.current.SetMotorSpeeds(1f, 0.01f);
                }
                else
                {
                    slothAnimator.speed = 0;
                    Gamepad.current.SetMotorSpeeds(0f, 0f);
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
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            leftStick = context.ReadValue<Vector2>();
        }
        
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
        
        if (context.canceled)
        {
            isAttacking = false;
        }
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
    }

    public void OnGetReady(InputAction.CallbackContext context)
    {
        if (GameManager.S.gameState == GameManager.State.WaitForPlayers)
        {
            if (!_isReadyToGame)
            {
                GameManager.S.readyPlayer++;
                GUIManager.S.PlayerReady(playerID);
                _isReadyToGame = true;
            }
            else
            {
                GameManager.S.readyPlayer--;
                GUIManager.S.PlayerJoin(playerID);
                _isReadyToGame = false;
            }
            
        } 
    }

}
