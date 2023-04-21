using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class CameraAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimationBool();
    }

    private void SetAnimationBool()
    {
        switch (GameManager.S.gameState)
        {
            case GameManager.State.TitleScreen:
                _animator.SetBool("IsOnTitle", true);
                _animator.SetBool("IsOnLevelSelection", false);
                _animator.SetBool("IsOnWaitForPlayer", false);
                _animator.SetBool("IsOnMapping", false);
                break;
            case GameManager.State.LevelSelection:
                _animator.SetBool("IsOnTitle", false);
                _animator.SetBool("IsOnLevelSelection", true);
                _animator.SetBool("IsOnWaitForPlayer", false);
                _animator.SetBool("IsOnMapping", false);
                break;
            case GameManager.State.Mapping:
                _animator.SetBool("IsOnTitle", false);
                _animator.SetBool("IsOnLevelSelection", false);
                _animator.SetBool("IsOnWaitForPlayer", false);
                _animator.SetBool("IsOnMapping", true);
                break;
            case GameManager.State.WaitForPlayers:
                _animator.SetBool("IsOnTitle", false);
                _animator.SetBool("IsOnLevelSelection", false);
                _animator.SetBool("IsOnWaitForPlayer", true);
                _animator.SetBool("IsOnMapping", false);
                break;
            default:
                break;
        }
    }
}
