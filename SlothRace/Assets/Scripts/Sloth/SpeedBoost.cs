using System.Collections;
using System.Collections.Generic;
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
            if (player.foodCounter < 5)
            {
                player.hasItem = true;
                player.foodCounter++;
                // decrease movement speed
                player.movementSpeed = player.movementSpeed * player.slowAmt;
                // decrease animator speed
                player.animatorSpeed = player.animatorSpeed * player.slowAmt;

                player.TellGUIManagerIHaveAnItem();
                Debug.Log("adding lettuce: " + player.foodCounter + "/5");
            }
            else
            {
                Debug.Log("you reached the limit of max number of lettuces");
            }

            Destroy(this.gameObject);
        }
    }
}
