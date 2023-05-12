using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private GameObject sfx_move;
    [SerializeField] private GameObject sfx_fart;
    [SerializeField] private GameObject sfx_item;
    [SerializeField] private GameObject sfx_shout;

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

    private void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            GenerateSoundEffect(sfx_shout, 1.0f);
        }
    }

    private void GenerateSoundEffect(GameObject sfx, float time)
    {
        GameObject g = Instantiate(sfx);
        Destroy(g, time);
    }
    public void LaunchMove()
    {
        GameObject g = Instantiate(sfx_move);
        Destroy(g, 3f);
    }

    public void PickUpItem()
    {
        GenerateSoundEffect(sfx_item, 1.0f);
    }

    public void Fart()
    {
        GenerateSoundEffect(sfx_fart, 3.0f);
    }
}
