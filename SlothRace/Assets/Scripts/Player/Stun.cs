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
            SoundManager.S.Vomit();
            StartCoroutine(StunningPlayer(player));
            // set trigger to false, only want to use stun once
            gameObject.GetComponent<SphereCollider>().isTrigger = false;

        }
    }

    private IEnumerator StunningPlayer(Player player)
    {
        player.stunned = true;

        Debug.Log("cant move");
        yield return new WaitForSeconds(stunDuration);

        // reset speeds
        player.stunned = false;
    }
}
