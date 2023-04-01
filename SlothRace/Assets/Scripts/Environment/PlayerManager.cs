using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [Header("Players")]
    private List<PlayerInput> players = new List<PlayerInput>();
    private List<Player> playerControllers = new List<Player>();
    [SerializeField] private List<Transform> respawningPoints;
    [SerializeField] private List<LayerMask> playerLayers;

    private PlayerInputManager _playerInputManager;

    private void Awake()
    {
        _playerInputManager = FindObjectOfType<PlayerInputManager>();
    }

    private void OnEnable()
    {
        _playerInputManager.onPlayerJoined += AddPlayer;
    }

    private void OnDisable()
    {
        _playerInputManager.onPlayerJoined -= AddPlayer;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddPlayer(PlayerInput player)
    {
        players.Add(player);
        playerControllers.Add(player.transform.parent.GetComponent<Player>());

        // Respawn point set
        Transform playerTransform = player.transform.parent;
        
        // Convert layer mask to an integer
        int layerToAdd = (int)Mathf.Log(playerLayers[players.Count - 1].value, 2);

        // set the layer
        playerTransform.GetComponentInChildren<CinemachineVirtualCamera>().gameObject.layer = layerToAdd;
        
        // add the layer
        player.camera.cullingMask |= 1 << layerToAdd;
        
        // set the action in the custom cinemachine input handler
        playerTransform.GetComponentInChildren<InputHandler>().horizontal = player.actions.FindAction("RightStickMove");
    }
}
