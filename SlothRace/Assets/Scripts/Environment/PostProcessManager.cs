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
    [SerializeField] private GameObject mapping;
    
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
                    mapping.SetActive(false);
                    break; 
                case GameManager.State.Mapping:
                    levelSelect.SetActive(false);
                    main.SetActive(false);
                    waitForPlayers.SetActive(false);
                    mapping.SetActive(true);
                    break;
                case GameManager.State.WaitForPlayers:
                    levelSelect.SetActive(false);
                    main.SetActive(false);
                    waitForPlayers.SetActive(true);
                    mapping.SetActive(false);
                    break;
                default:
                    main.SetActive(true);
                    levelSelect.SetActive(false);
                    waitForPlayers.SetActive(false);
                    mapping.SetActive(false);
                    break;
            }
        }
    }
}
