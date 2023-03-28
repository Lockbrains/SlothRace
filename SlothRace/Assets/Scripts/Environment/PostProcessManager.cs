using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PostProcessManager : MonoBehaviour
{
    [Header("Post Process Boxes")] 
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject levelSelect;
    [SerializeField] private GameObject waitForPlayers;
    
    // Update is called once per frame
    void Update()
    {
        UpdatePostProcessBox();
    }

    private void UpdatePostProcessBox()
    {
        if (GameManager.S != null)
        {
            switch (GameManager.S.gameState)
            {
                case GameManager.State.LevelSelection:
                    levelSelect.SetActive(true);
                    main.SetActive(false);
                    waitForPlayers.SetActive(false);
                    break; 
                case GameManager.State.WaitForPlayers:
                    levelSelect.SetActive(false);
                    main.SetActive(false);
                    waitForPlayers.SetActive(true);
                    break;
                default:
                    main.SetActive(true);
                    levelSelect.SetActive(false);
                    waitForPlayers.SetActive(false);
                    break;
            }
        }
    }
}
