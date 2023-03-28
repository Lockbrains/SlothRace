using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitForPlayerVisual : MonoBehaviour
{
    
    public enum PlayerJoinStatus
    {
        NotPrepared, Prepared
    }

    [Header("Contents")]
    [SerializeField] private Image visual;
    [SerializeField] private Sprite readySprite;
    [SerializeField] private Sprite joinedSprite;

    [Header("Status")]
    [SerializeField] private PlayerJoinStatus status;
    public int playerID;
    public bool isPrepared;

    // Update is called once per frame
    void Start()
    {
        status = PlayerJoinStatus.NotPrepared;
    }

    void Update()
    {
        UpdateWithStatus();
    }

    private void UpdateWithStatus()
    {
        switch(status)
        {
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

    public void Join()
    {
        status = PlayerJoinStatus.NotPrepared;
    }

    public void Ready()
    {
        status = PlayerJoinStatus.Prepared;
    }
}
