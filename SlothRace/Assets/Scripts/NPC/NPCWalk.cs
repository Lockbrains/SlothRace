using System;
using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("Collided!");
            Vector3 originPos = transform.position;
            Vector3 destPos = collision.transform.position;
            Rigidbody rigidbody = collision.gameObject.GetComponent<Rigidbody>();
            collision.gameObject.GetComponent<HipCamera>().TurnOnCamera();
            rigidbody.AddForce(25000 * (destPos-originPos).normalized);
        }
    }
}
