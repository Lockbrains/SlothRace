using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverDetector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            player.Win();
        }
    }
}
