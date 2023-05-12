using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    public ParticleSystem part;
    private bool istriggered = false;
    public float stunDuration = 2f;

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

