using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{
    #region Variables
    [Header("Input Data")]
    [SerializeField] private int playerID;
    [SerializeField] private Sloth _sloth;
    [SerializeField] private GameObject _physicalSloth;
    public Camera playerCamera;
    private bool _isReadyToGame;

    [Header("Player Properties")] 
    [SerializeField] private List<LayerMask> playerLayers;
    public float originalMoveSpeed = 0.4f;
    public float camRotationSpeed = 100;
    public float playerRotationSpeed = 2;
    public float movementSpeed;
    private float actualRotationSpeed = 0;

    public float originalAnimatorSpeed = 1;
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


    #endregion

    #region Unity Basics

    private void Start()
    {
        // setting the playerID as the id from playerInput
        playerID = playerInput.playerIndex;

        // the player should be generated in the WaitForPlayer state
        GUIManager.S.PlayerJoin(playerID);
        GameManager.S.joinedPlayer++;
        
        // Set up the player input comp
        if (playerInput == null)
        {
            playerInput = GetComponent<PlayerInput>();
        }


        // Set up GameManager and GUIManager slothAnimator
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
        movementSpeed = originalMoveSpeed;
        animatorSpeed = originalAnimatorSpeed;
    }


    void Update()
    {      
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            SlothMovement();
            SetAnimation();
            SetPlayerStatusInHUD();
        }   
    }

    #endregion
    
    #region Control
    void SlothMovement()
    {
        // the player tries to move
        if (isMovingLeft)
        {
            GUIManager.S.EnableLeft(playerID);
            _isSwitchingToRight = true;
            if (_isSwitchingToLeft)
            {
                GUIManager.S.RefreshHUDColor(playerID, false);
                _isSwitchingToLeft = false;
            }
            if (leftArm && rightLeg && !rightArm && !leftLeg)
            {
                actualRotationSpeed = playerRotationSpeed;
                slothAnimator.speed = animatorSpeed;
            }
            else
            {
                actualRotationSpeed = 0;
                slothAnimator.speed = 0;
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
            if (leftLeg && rightArm && !leftArm && !rightLeg)
            {
                actualRotationSpeed = playerRotationSpeed;
                slothAnimator.speed = animatorSpeed;
            }
            else
            {
                actualRotationSpeed = 0;
                slothAnimator.speed = 0;
            }
        }

    }
    
    public void OnLeftStickMove(InputAction.CallbackContext context)
    {
        if (GameManager.S.maxPlayerCount == 2)
        {
            leftStick = context.ReadValue<Vector2>();
        }

        float inputValue = context.ReadValue<Vector2>().x;

        // player rotation
        var rotation1 = player.transform.rotation;
        float rotation = inputValue * actualRotationSpeed + rotation1.eulerAngles.y;
        Vector3 playerEulerAngles = rotation1.eulerAngles;
        playerEulerAngles.y = rotation;
        rotation1 = Quaternion.Euler(playerEulerAngles);
        player.transform.rotation = rotation1;

        // update camera rotation so its directly behind player
        //var rotation2 = camPosition.transform.rotation;
        //float camYRotation = inputValue * playerRotationSpeed + rotation2.eulerAngles.y;
        //float camXRotation = rotation2.eulerAngles.x;
        //float camZRotation = rotation2.eulerAngles.z;

        //rotation2 = Quaternion.Euler(camXRotation, camYRotation, camZRotation);
        //camPosition.transform.rotation = rotation2;

    }

    
    // RightStickMove: move the camera
    public void OnRightStickMove(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
        //float inputValue = context.ReadValue<Vector2>().x;
        
        //var rotation1 = camPosition.transform.rotation;
        //float start = rotation1.eulerAngles.y;
        //float rotation = inputValue * camRotationSpeed + rotation1.eulerAngles.y;
        //rotation1 = Quaternion.Euler(0f, rotation, 0f);
        //camPosition.transform.rotation = rotation1;
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
        }

        if (context.canceled)
        {
            leftArm = false;
        }
        
        GUIManager.S.ChangeLeftArmColor(isMovingLeft, leftArm, playerID);
        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;
    }

    public void OnMoveRightLeg(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightLeg = true;
        }

        if (context.canceled)
        {
            rightLeg = false;
        }
        GUIManager.S.ChangeRightLegColor(isMovingLeft, rightLeg, playerID);
        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;
    }
    
    public void OnMoveLeftLeg(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            leftLeg = true;
        }

        if (context.canceled)
        {
            leftLeg = false;
        }
        GUIManager.S.ChangeLeftLegColor(!isMovingLeft, leftLeg, playerID);
        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;

    }

    public void OnMoveRightArm(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            rightArm = true;
        }

        if (context.canceled)
        {
            rightArm = false;
        }
        
        GUIManager.S.ChangeRightArmColor(!isMovingLeft, rightArm, playerID);
        if (playerID == 0) GameManager.S.player1Started = true;
        else GameManager.S.player2Started = true;

    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(GameManager.S.gameState == GameManager.State.GameEnd)
        {
            SceneManager.LoadScene("Alpha Test");
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

    #endregion
    
    #region GUI & Animation
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
    #endregion
    
    #region Player Items
    private void SpeedBoost(SpeedBoostData speedData)
    {
        animatorSpeed = speedData.animationSpeed;
        movementSpeed = speedData.movementSpeed;
        speedBoost = true;
    }

    private void ResetSpeed()
    {
        animatorSpeed = originalAnimatorSpeed;
        //slothAnimator.speed = originalanimatorSpeed;
        movementSpeed = originalMoveSpeed;
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
    #endregion
    
    #region Public Interface
    public int GetPlayerID()
    {
        return playerID;
    }

    public void Win()
    {
        GameManager.S.gameState = GameManager.State.GameEnd;
        GUIManager.S.PlayerWins(playerID);
    }
    
    #endregion
}
