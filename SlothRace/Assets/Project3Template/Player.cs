using System;
using System.Collections;
using System.Collections.Generic;
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
    public float originalmoveSpeed = 0.4f;
    public float movementSpeed;
    public float camRotationSpeed = 100;
    public float playerRotationSpeed = 2;

    public float originalanimatorSpeed = 1;
    public float animatorSpeed;

    private bool _isSwitchingToLeft, _isSwitchingToRight;
  
    [Header("Player Status")]
    public bool isMovingLeft;
    public bool isAttacking;
    public bool leftLeg, leftArm, rightLeg, rightArm;
    public bool speedBoost;

    [Header("Player Abilities")]
    public Stack<GameObject> playerAbilities = new Stack<GameObject>();
    public bool hasItem;

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
    public GameObject camPosition;

    [Header("Rank")]
    public int rank;

 

    // private bool sprinting = false;
    private float verticalVelocity = 0;

    private DualSenseTriggerState leftTriggerState;
    private DualSenseTriggerState rightTriggerState;
    private DualSenseRumble _rumble;

    [SerializeField] private AbstractDualSenseBehaviour listener;
    public DualSenseGamepadHID DualSense;
    [HideInInspector] public bool hasDualSense;

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
        playerID = playerInput.playerIndex;
        GUIManager.S.PlayerJoin(playerID);
        GameManager.S.joinedPlayer++;
        
        GameManager.S.dualSenseMonitor.listeners[playerID] = listener;
        if(GameManager.S.joinedPlayer == GameManager.S.maxPlayerCount)
            GameManager.S.dualSenseMonitor.gameObject.SetActive(true);
        hasDualSense = false;

        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }
        
        
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
        
        slothAnimator.speed = 0;
        _isReadyToGame = false;
        _isSwitchingToRight = false;
        _isSwitchingToLeft = false;

        // set movement speed and animation speed to default values
        movementSpeed = originalanimatorSpeed;
        animatorSpeed = originalanimatorSpeed;
    }

    private void CheckDSController()
    {
        leftTriggerState.Continuous.StartPosition = 0;
        leftTriggerState.Continuous.Force = 255;
        rightTriggerState.Continuous.StartPosition = 0;
        rightTriggerState.Continuous.Force = 255;
        
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
        //SetPlayerCamera();
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

    public void TellGUIManagerIHaveAnItem()
    {
        GUIManager.S.SetItemAvailability(playerID, true);
    }

    public void TellGUIManagerIHaveUsedTheItem()
    {
        GUIManager.S.SetItemAvailability(playerID, false);
    }
    
    void SlothMovement()
    {
        float targetSpeed = movementSpeed;
        Vector3 movement = new Vector3(leftStick.x * targetSpeed, verticalVelocity, leftStick.y * targetSpeed) * slothAnimator.speed;

        //since it's in update and continuous the vector has to be multiplied by Time.deltaTime to be frame independent
        Vector3 curPos = transform.position;
        curPos += movement * Time.deltaTime;
        transform.position = curPos;

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
                GUIManager.S.EnableLeft(playerID);
                _isSwitchingToRight = true;
                if (_isSwitchingToLeft)
                {
                    GUIManager.S.RefreshHUDColor(playerID, false);
                    _isSwitchingToLeft = false;
                }
                if (leftArm && rightLeg)
                {
                    slothAnimator.speed = animatorSpeed;
                    float size = slothAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    //Gamepad.current.SetMotorSpeeds(0.01f, 1f);
                }
                else
                {
                    slothAnimator.speed = 0;
                    //Gamepad.current.SetMotorSpeeds(0f, 0f);
                }
            }
            else
            {
                GUIManager.S.DisableLeft(playerID);
                _isSwitchingToLeft = true;
                if (_isSwitchingToRight)
                {
                    GUIManager.S.RefreshHUDColor(playerID, true);
                    _isSwitchingToRight = false;
                }
                if (leftLeg && rightArm)
                {
                    slothAnimator.speed = animatorSpeed;
                    float size = slothAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                    //Gamepad.current.SetMotorSpeeds(1f, 0.01f);
                }
                else
                {
                    slothAnimator.speed = 0;
                    //Gamepad.current.SetMotorSpeeds(0f, 0f);
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

        // player rotation
        float rotation = inputValue * playerRotationSpeed + player.transform.rotation.eulerAngles.y;
        Vector3 playerEulerAngles = player.transform.rotation.eulerAngles;
        playerEulerAngles.y = rotation;
        player.transform.rotation = Quaternion.Euler(playerEulerAngles);

        // update camera rotation so its directly behind player
        float camYRotation = inputValue * playerRotationSpeed + camPosition.transform.rotation.eulerAngles.y;
        float camXRotation = camPosition.transform.rotation.eulerAngles.x;
        float camZRotation = camPosition.transform.rotation.eulerAngles.z;

        //float offset = camYRotation - player.transform.rotation.eulerAngles.y;

        camPosition.transform.rotation = Quaternion.Euler(camXRotation, camYRotation, camZRotation);
       

        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;
    }

    public void OnRightStickMove(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
        float inputValue = context.ReadValue<Vector2>().x;

        float start = camPosition.transform.rotation.eulerAngles.y;

        float rotation = inputValue * camRotationSpeed + camPosition.transform.rotation.eulerAngles.y;

        camPosition.transform.rotation = Quaternion.Euler(0f, rotation, 0f);

        /*if (context.canceled)
        {
            StartCoroutine(CameraSmoothSnap(rotation, start));
        }

        if (context.started)
        {
            StopCoroutine("CameraSmoothSnap");
        }*/
    }

    private IEnumerator CameraSmoothSnap(float rotation, float start)
    {
        float timePassed = 0;
        float lerpValue;

        while (timePassed < 1f)
        {
            if (rotation > start - 180 && rotation < start)
            {
                
                lerpValue = Mathf.Lerp(rotation, 360 + start, timePassed);
                camPosition.transform.rotation = Quaternion.Euler(0f, lerpValue, 0f);
            }
            else
            {
                lerpValue = Mathf.Lerp(rotation, start, timePassed);
                camPosition.transform.rotation = Quaternion.Euler(0f, lerpValue, 0f);
            }
            timePassed += Time.deltaTime;
            yield return null;
        }

        lerpValue = 0;
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

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(GameManager.S.gameState == GameManager.State.GameEnd)
        {
            SceneManager.LoadScene("Alpha Test");
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

    public void Win()
    {
        GameManager.S.gameState = GameManager.State.GameEnd;
        GUIManager.S.PlayerWins(playerID);
    }

    private void SpeedBoost(SpeedBoostData speedData)
    {
        animatorSpeed = speedData.animationSpeed;
        movementSpeed = speedData.movementSpeed;
        speedBoost = true;
    }

    private void ResetSpeed()
    {
        animatorSpeed = originalanimatorSpeed;
        //slothAnimator.speed = originalanimatorSpeed;
        movementSpeed = originalmoveSpeed;
        speedBoost = false;
        Debug.Log("slow down");
    }

    private IEnumerator StartSpeedBoost(SpeedBoostData speedData)
    {
        SpeedBoost(speedData);
        yield return new WaitForSeconds(speedData.duration);
        ResetSpeed();

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

    public void OnSpeedBoost(InputAction.CallbackContext context)
    {
        if (context.started && playerAbilities.Count != 0 && playerAbilities.Peek().name.Contains("SpeedBoost"))
        {
            hasItem = false;
            GameObject speedItem = playerAbilities.Pop();
            SpeedBoost speed = speedItem.GetComponent<SpeedBoost>();
            SpeedBoostData speedData = speed.speedData;
            StartCoroutine(StartSpeedBoost(speedData));
            TellGUIManagerIHaveUsedTheItem();
        }
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
