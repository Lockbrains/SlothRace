using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // get player script
            Player player = collision.gameObject.GetComponent<HipCamera>().player;

            // add speed boost to player only if stack == 0
            if (player.EatLettuce())
            {
                player.hasItem = true;
                player.UpdateCount();
                // decrease movement speed
                player.movementSpeed = player.movementSpeed * player.slowAmt;
                // decrease animator speed
                player.animatorSpeed = player.animatorSpeed * player.slowAmt;
                player.TellGUIManagerIHaveAnItem();
            }
            else
            {
                player.TellGUIManagerICantEatMore();
            }

            Destroy(this.gameObject);
        }
    }
}
