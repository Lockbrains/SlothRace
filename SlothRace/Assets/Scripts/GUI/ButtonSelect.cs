using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelect : MonoBehaviour
{
    [SerializeField] private Button defaultButton;
    [SerializeField] private GameManager.State selectState;
    [SerializeField] private float waitTime;
    private bool hasBeenSelected = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasBeenSelected)
        {
            if (GameManager.S.gameState == selectState)
            {
                StartCoroutine(WaitForButton());
            }
        }
    }

    IEnumerator WaitForButton()
    {
        yield return new WaitForSeconds(waitTime);
        EventSystem.current.SetSelectedGameObject(defaultButton.gameObject);
        hasBeenSelected = true;
    }
}
