using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerIndicator : MonoBehaviour
{

    [SerializeField] private Player _player;
    [SerializeField] private Image img_icon;
    [SerializeField] private Sprite player1, player2;
    
    // Update is called once per frame
    void Update()
    {
        switch (_player.GetPlayerID())
        {
            case 0:
                img_icon.sprite = player1;
                break;
            case 1:
                img_icon.sprite = player2;
                break;
            default:
                break;
        }
    }
}
