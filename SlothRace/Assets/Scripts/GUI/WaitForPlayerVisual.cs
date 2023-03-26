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
    [SerializeField] private PlayerJoinStatus status;
    public int playerID;
    public bool isPrepared;

    // Update is called once per frame
    private void Start()
    {
        status = PlayerJoinStatus.NotJoined;
    }

    private void Update()
    {
        UpdateWithStatus();
    }

    private void UpdateWithStatus()
    {
        switch(status)
        {
            case PlayerJoinStatus.NotJoined:
                visual.sprite = waitForJoinSprite;
                Debug.Log("visual should be changed to wait for join.");
                break;
            case PlayerJoinStatus.NotPrepared:
                visual.sprite = joinedSprite;
                Debug.Log("visual should be changed to wait for norprepared.");
                break;
            case PlayerJoinStatus.Prepared:
                visual.sprite = readySprite;
                Debug.Log("visual should be changed to wait for prepared.");
                break;
            default:
                break;
        }
    }

    public void Join()
    {
        status = PlayerJoinStatus.NotPrepared;
    }
}
