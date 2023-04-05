using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stun : MonoBehaviour
{
    public float stunDuration = 4f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            Debug.Log("stunning Player");
            // get player script
            Player player = other.gameObject.GetComponent<HipCamera>().player;
            StartCoroutine(StunningPlayer(player));
            // set trigger to false, only want to use stun once
            gameObject.GetComponent<Collider>().isTrigger = false;

            
            
        }
    }

    private IEnumerator StunningPlayer(Player player)
    {
        float curSpeed = player.movementSpeed;
        float animSpeed = player.animatorSpeed;

        player.movementSpeed = 0;
        player.animatorSpeed = 0;
        Debug.Log("cant move");
        yield return new WaitForSeconds(stunDuration);

        // reset speeds
        player.movementSpeed = curSpeed;
        player.animatorSpeed = animSpeed;
    }
}
