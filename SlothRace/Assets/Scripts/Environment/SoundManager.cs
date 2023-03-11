using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject sfx_move;

    public static SoundManager S;

    private void Awake()
    {
        if (S)
        {
            Destroy(S.gameObject);
        }
        else
        {
            S = this;
        }
    }

    public void LaunchMove()
    {
        GameObject g = Instantiate(sfx_move);
        Destroy(g, 3f);
    }
}
