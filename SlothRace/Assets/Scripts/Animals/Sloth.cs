using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sloth : MonoBehaviour
{
    public bool isMovingLeft;

    [SerializeField] private Player _player;
    
    // Update is called once per frame
    void Update()
    {
        _player.isMovingLeft = isMovingLeft;
    }
}
