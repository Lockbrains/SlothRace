using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitForPlayerVisual : MonoBehaviour
{
    
    public enum PlayerJoinStatus
    {
        NotJoined, NotPrepared, Prepared
    }

    [Header("Contents")]
    [SerializeField] private Image visual;
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite waitForJoinSprite;
    [SerializeField] private Sprite joinedSprite;

    [Header("Status")]
    public PlayerJoinStatus status;
    public int playerID;
    public bool isPrepared;

    // Update is called once per frame
    void Update()
    {
        visual.enabled = playerID >= GameManager.S.maxPlayerCount;
    }

    void UpdateWithStatus()
    {
        switch(status)
        {
            case PlayerJoinStatus.NotJoined:
                visual.sprite = waitForJoinSprite;
                break;
            case PlayerJoinStatus.NotPrepared:
                visual.sprite = joinedSprite;
                break;
            case PlayerJoinStatus.Prepared:
                visual.sprite = readySprite;
                break;
            default:
                break;
        }
    }
}
