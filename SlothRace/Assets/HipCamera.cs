using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HipCamera : MonoBehaviour
{
    public GameObject flyCamera;
    public PlayerInput playerGroup;
    
    public void TurnOnCamera()
    {
        flyCamera.SetActive(true);
        playerGroup.camera = flyCamera.GetComponent<Camera>();
    }
}
