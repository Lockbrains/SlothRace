using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelectButton : MonoBehaviour
{
    public void LoadLevel(int playerNum)
    {
        GUIManager.S.OnLevelSelectButtonClick(playerNum);
    }
}
