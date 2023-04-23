using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerHUDAssignment : MonoBehaviour
{
    [SerializeField] private PlayerHUD[] TwoPlayer;
    [SerializeField] private PlayerHUD[] ThreePlayer;
    [SerializeField] private PlayerHUD[] FourPlayer;

    [SerializeField] private GameObject TwoPlayerHUD;
    [SerializeField] private GameObject ThreePlayerHUD;
    [SerializeField] private GameObject FourPlayerHUD;
    
    public PlayerHUD[] AssignPlayerHUD(int playerNum)
    {
        switch (playerNum)
        {
            case 2:
                return TwoPlayer;
            case 3:
                return ThreePlayer;
            case 4:
                return FourPlayer;
            default:
                return Array.Empty<PlayerHUD>();
        }
    }

    public void TurnOnHUD(int playerNum)
    {
        switch (playerNum)
        {   
            case 2:
                TwoPlayerHUD.SetActive(true);
                ThreePlayerHUD.SetActive(false);
                FourPlayerHUD.SetActive(false);
                break;
            case 3:
                TwoPlayerHUD.SetActive(false);
                ThreePlayerHUD.SetActive(true);
                FourPlayerHUD.SetActive(false);
                break;
            case 4:
                TwoPlayerHUD.SetActive(false);
                ThreePlayerHUD.SetActive(false);
                FourPlayerHUD.SetActive(true);
                break;
            default:
                break;
            
        }
    }
    
    
}
