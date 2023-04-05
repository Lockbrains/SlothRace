using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Video;

public class NPCWalk : MonoBehaviour
{

    #region Variables
    public float walkingSpeed;
    public Vector3 directionNormalized;

    private int walkDirection = 1;

    private bool getHit = false;
    #endregion
    
    #region Unity Basics
    void Update()
    {
        Vector3 currentPos = transform.position;
        currentPos += directionNormalized * (walkDirection * walkingSpeed * Time.deltaTime);
        transform.position = currentPos;

        transform.forward = directionNormalized * walkDirection;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("NPCTrigger"))
        {
            walkDirection *= -1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.CompareTag("Player"))
        {
            Vector3 originPos = transform.position;
            Vector3 destPos = collision.transform.position;
            Rigidbody component = collision.gameObject.GetComponent<Rigidbody>();
            component.AddForce(walkingSpeed * 1000 * (destPos-originPos).normalized);
            Player player = collision.gameObject.GetComponent<HipCamera>().player;

            // stop car after collision
            StartCoroutine(DisableCar());
            //if (!getHit) RespawnPlayer(player);
            //GUIManager.S.PlayerWins(player.GetPlayerID());
            //GameManager.S.gameState = GameManager.State.GameEnd;
        }
    }

    #endregion
    
    private void RespawnPlayer(Player gamePlayer)
    {
        StartCoroutine(WaitToRespawn(gamePlayer));
    }

    private IEnumerator WaitToRespawn(Player player)
    {
        getHit = true;
        yield return new WaitForSeconds(4.0f);
        player.ResetPosition();
        getHit = false;
    }

    private IEnumerator DisableCar()
    {
        // disable car for 10 secs
        float currentSpeed = walkingSpeed;
        walkingSpeed = 0; 
        yield return new WaitForSeconds(10f);

        // reenable car
        walkingSpeed = currentSpeed;
    }
}
