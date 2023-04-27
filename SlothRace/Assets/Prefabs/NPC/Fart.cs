using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public ParticleSystem part;
    private bool istriggered = false;
    public float stunDuration = 20f;

    private void Start()
    {
        part = GetComponent<ParticleSystem>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (istriggered) return;
        else
        {
            // set trigger to true, only want to use stun once
            istriggered = true;
            Debug.Log("stunning Player");
            // get player script
            Player player = gameObject.GetComponent<HipCamera>().player;
            StartCoroutine(StunningPlayer(player)); 

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

