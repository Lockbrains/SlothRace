using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBoost : MonoBehaviour
{
    public SpeedBoostData speedData;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // get player script
            Player player = collision.gameObject.GetComponent<HipCamera>().player;

            // add speed boost to player only if stack == 0
            if (player.playerAbilities.Count == 0)
            {
                player.playerAbilities.Push(this.gameObject);
                player.hasItem = true;
                player.TellGUIManagerIHaveAnItem();
                Debug.Log("push speedboost to stack");
            }
            else
            {
                Debug.Log("you reached the limit of max number of abilities");
            }

            this.gameObject.SetActive(false);
            //Destroy(this.gameObject, 1f);
        }
    }
}
