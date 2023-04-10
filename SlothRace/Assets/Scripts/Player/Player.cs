using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;


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
    public float poopSpeed;
    public float movementSpeed;
    public float camRotationSpeed = 100;
    public float playerRotationSpeed = 2;
    private float actualRotationSpeed = 0;

    public float originalAnimatorSpeed = 1;
    public float animatorSpeed;

    public float slowAmt = 0.95f;

    private bool _isSwitchingToLeft, _isSwitchingToRight;
  
    [Header("Player Status")]
    public bool isMovingLeft;
    public bool isAttacking;
    public bool leftLeg, leftArm, rightLeg, rightArm;
    public bool speedBoost;

    [Header("Player Abilities")]
    public Stack<GameObject> playerAbilities = new Stack<GameObject>();
    public bool hasItem;

    private int lettuceCounter = 0;
    private int maxfoodCount = 5;

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

    
    [Header("Lettuce and Poop")]
    [SerializeField] private GameObject poopPrefab;
    [SerializeField] private GameObject poopHUD;
    [SerializeField] private Scrollbar poopProgress;
    private bool pooping = false;
    

    [Header("Rank")]
    public int rank;

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
                GUIManager.S.player1Anim = slothAnimator;
                break;
            case 1:
                GUIManager.S.player2Anim = slothAnimator;
                break;
            case 2:
                GUIManager.S.player3Anim = slothAnimator;
                break;
            case 3:
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
            UpdatePlayerRotation();
            UpdateCameraRotation();
            CheckPoop();
        }   
    }

    private void CheckPoop()
    {
        if (pooping)
        {
            poopProgress.size += poopSpeed * Time.deltaTime;
        }

        if (Mathf.Abs(poopProgress.size - 1.0f) <= float.Epsilon)
        {
            poopHUD.SetActive(false);
            poopProgress.size = 0;
            // poop ends
            lettuceCounter = 0;
            pooping = false;
            UpdateCount();
            
            // instantiate poop
            // get the hip position
            Vector3 playerPos = camPosition.transform.position;
            playerPos.y = -1.5f;
            Debug.Log("pooping");
            GameObject newPoop = Instantiate(poopPrefab, playerPos - (camPosition.transform.forward * 3f), Quaternion.identity);
            
            newPoop.GetComponent<Rigidbody>().AddForce((-camPosition.transform.forward * 3f + new Vector3(0,10f,0)).normalized * 100f);
        }
    }
    #endregion
    
    #region Control
    void SlothMovement()
    {
        // compensate for physical system with speed that's too slow
        float targetSpeed = movementSpeed;
        Vector3 movement = camPosition.transform.forward.normalized * movementSpeed * slothAnimator.speed;

        //since it's in update and continuous the vector has to be multiplied by Time.deltaTime to be frame independent
        Vector3 curPos = transform.position;
        curPos += movement * Time.deltaTime;
        transform.position = curPos;

        // the player tries to move
        if (isMovingLeft)
        {
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
        
        if (pooping)
        {
            slothAnimator.speed = 0;
            actualRotationSpeed = 0;
        }

    }
    
    public void OnLeftStickMove(InputAction.CallbackContext context)
    {
        leftStick = context.ReadValue<Vector2>();
    }

    private void UpdatePlayerRotation()
    {
        float inputValue = leftStick.x;
        
        // read the current rotation
        var currentRotation = player.transform.rotation;
        
        // set the offset
        float y_offset = inputValue * playerRotationSpeed + currentRotation.eulerAngles.y;
        Vector3 playerEulerAngles = currentRotation.eulerAngles;
        playerEulerAngles.y = y_offset + camPosition.transform.eulerAngles.y;
        currentRotation = Quaternion.Euler(playerEulerAngles);
        //player.transform.rotation = currentRotation;
        player.transform.rotation = Quaternion.Lerp(player.transform.rotation, currentRotation, playerRotationSpeed * Time.deltaTime);
    }
    
    private void UpdateCameraRotation()
    {
        float inputValueX = leftStick.x;
        float inputValueY = leftStick.y;

        // read the current rotation
        var rotation1 = camPosition.transform.rotation;
        
        // rotating around the y axis
        float startY = rotation1.eulerAngles.y;
        float startX = rotation1.eulerAngles.x;
        
        float rotation_y = inputValueX * camRotationSpeed + rotation1.eulerAngles.y;
        float rotation_x = inputValueY * camRotationSpeed + startX;
        if (rotation_x > 180 && rotation_x < 340) rotation_x = 340;
        else if (rotation_x < 180 && rotation_x > 40) rotation_x = 40;
        
        rotation1 = Quaternion.Euler(rotation_x, rotation_y, 0f);
        camPosition.transform.rotation =
            Quaternion.Lerp(camPosition.transform.rotation, rotation1, camRotationSpeed * Time.deltaTime);
    }
    
    // RightStickMove: move the camera
    public void OnRightStickMove(InputAction.CallbackContext context)
    {
        rightStick = context.ReadValue<Vector2>();
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
        PlayerManager.S.playerHasStart[playerID] = true;
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
        
        PlayerManager.S.playerHasStart[playerID] = true;
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
        
        PlayerManager.S.playerHasStart[playerID] = true;
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
        
        PlayerManager.S.playerHasStart[playerID] = true;
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if(GameManager.S.gameState == GameManager.State.GameEnd)
        {
            SceneManager.LoadScene("Alpha Test");
        }
    }
    
    public void OnUseItem(InputAction.CallbackContext context)
    {
        if (GameManager.S.gameState == GameManager.State.GameStart)
        {
            if (context.started)
            {
                if (lettuceCounter <= 2)
                {
                    // Todo
                    Debug.Log("Your stomach is still empty.");    
                } else if (lettuceCounter < 5)
                {
                    // fart
                    Debug.Log("farting");
                    // instantiate fart
                    lettuceCounter-=2;
                    // increase speed
                    movementSpeed = movementSpeed / slowAmt;
                    animatorSpeed = animatorSpeed / slowAmt;
                    // update UI
                    // todo
                }
                else
                {
                    //poop
                    poopHUD.SetActive(true);
                    pooping = true;
                }
            }

            if (context.canceled)
            {
                if (lettuceCounter >= 5)
                {
                    pooping = false;
                }
            }
        }
        
        // old speed boost
        /*if (context.started && playerAbilities.Count != 0 && playerAbilities.Peek().name.Contains("SpeedBoost"))
        {
            hasItem = false;
            GameObject speedItem = playerAbilities.Pop();
            SpeedBoost speed = speedItem.GetComponent<SpeedBoost>();
            SpeedBoostData speedData = speed.speedData;
            StartCoroutine(StartSpeedBoost(speedData));
            TellGUIManagerIHaveUsedTheItem();
        }*/
    }

    private IEnumerator PoopDelay()
    {
        // get current speed
        float curSpeed = movementSpeed;
        float curAnimator = animatorSpeed;

        // cant move while pooping
        animatorSpeed = 0;
        movementSpeed = 0;
        yield return new WaitForSeconds(3f);

        // reset to cur speed
        movementSpeed = curSpeed;
        animatorSpeed = curAnimator;

        // instantiate poop
        // behind player position
        Vector3 playerPos = camPosition.transform.position;
        playerPos.y = -3.5f;

        Debug.Log("pooping");
        GameObject newPoop = Instantiate(poopPrefab, playerPos - (camPosition.transform.forward * 3f), Quaternion.identity);

        // increase speed 3x
        StartCoroutine(StartSpeedBoost());
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

    public void TellGUIManagerICantEatMore()
    {
        //todo
        return;
    }
    #endregion
    
    #region Player Items
    private void SpeedBoost()
    {
        Debug.Log("speeding");
        animatorSpeed = animatorSpeed * 3;
        movementSpeed = movementSpeed * 3;
        speedBoost = true;
    }

    private void ResetSpeed()
    {
        animatorSpeed = originalAnimatorSpeed;
        movementSpeed = originalMoveSpeed;
        speedBoost = false;
        Debug.Log("slow down");
    }

    private IEnumerator StartSpeedBoost()
    {
        SpeedBoost();
        yield return new WaitForSeconds(10f);
        ResetSpeed();
    }


    public void ResetPosition()
    {
        PlayerManager.S.RespawnPlayer(playerID);
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

    public bool EatLettuce()
    {
        lettuceCounter++;
        if (lettuceCounter > 5)
        {
            lettuceCounter = 5;
            return false;
        }

        return true;
    }

    public void UpdateCount()
    {
        GUIManager.S.UpdateCount(playerID, lettuceCounter);
    }
    
    public void Win()
    {
        GameManager.S.gameState = GameManager.State.GameEnd;
        GUIManager.S.PlayerWins(playerID);
    }
    
    #endregion
}
