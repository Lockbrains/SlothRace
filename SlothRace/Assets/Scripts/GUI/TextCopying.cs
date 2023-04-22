using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextCopying : MonoBehaviour
{

    [SerializeField] private Text copyTarget;

    [SerializeField] private Text thisText;
    // Update is called once per frame
    void Update()
    {
        thisText.text = copyTarget.text;
    }
}
