using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class NPCWalk : MonoBehaviour
{

    public float walkingSpeed;
    public Vector3 directionNormalized;

    private int walkDirection = 1;
    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos += directionNormalized * (walkDirection * walkingSpeed * Time.deltaTime);
        transform.position = currentPos;

        transform.forward = directionNormalized * walkDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "NPCTrigger")
        {
            walkDirection *= -1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Player")
        {
            Vector3 originPos = transform.position;
            Vector3 destPos = collision.transform.position;
            Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            rigidbody.AddForce(25000 * (destPos-originPos).normalized);
            Player player = collision.gameObject.GetComponent<HipCamera>().player;
            GUIManager.S.PlayerWins(player.GetPlayerID());
            GameManager.S.gameState = GameManager.State.Restart;
        }
    }

    private void RespawnPlayer(Player gamePlayer)
    {
        StartCoroutine(WaitToRespawn(gamePlayer));
    }

    private IEnumerator WaitToRespawn(Player player)
    {
        yield return new WaitForSeconds(4.0f);
        player.ResetPosition();
    }
}
