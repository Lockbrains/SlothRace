using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectButton : MonoBehaviour
{

    public Button btn_2player, btn_3player, btn_4player;

    private void Update()
    {
            btn_2player.interactable = GameManager.S.gameState == GameManager.State.LevelSelection;
            btn_3player.interactable = GameManager.S.gameState == GameManager.State.LevelSelection;
            btn_4player.interactable = GameManager.S.gameState == GameManager.State.LevelSelection;

    }
    public void LoadLevel(int playerNum)
    {
        GUIManager.S.OnLevelSelectButtonClick(playerNum);
    }
}
