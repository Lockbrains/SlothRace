using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountdownAssignment : MonoBehaviour
{
    [SerializeField] private GameObject[] cd2p;
    [SerializeField] private GameObject[] cd3p;
    [SerializeField] private GameObject[] cd4p;

    [SerializeField] private GameObject CD2P;
    [SerializeField] private GameObject CD3P;
    [SerializeField] private GameObject CD4P;

    public GameObject[] AssignmentCDHUD(int playerNum)
    {
        switch (playerNum)
        {
            case 2:
                return cd2p;
            case 3:
                return cd3p;
            case 4:
                return cd4p;
            default:
                Debug.Log("Wrong PlayerNum");
                return Array.Empty<GameObject>();
        }
    }

    public void ActivateCDHUD(int playerNum)
    {
        switch (playerNum)
        {
            case 2:
                CD2P.SetActive(true);
                CD3P.SetActive(false);
                CD4P.SetActive(false);
                break;
            case 3:
                CD2P.SetActive(false);
                CD3P.SetActive(true);
                CD4P.SetActive(false);
                break;
            case 4:
                CD2P.SetActive(false);
                CD3P.SetActive(false);
                CD4P.SetActive(true);
                break;
            default:
                break;
        }
    }
}
