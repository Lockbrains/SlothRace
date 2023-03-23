using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetPosition : MonoBehaviour
{
    Rigidbody rb;
    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("awake");
        rb = GetComponent<Rigidbody>();
        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
        
    }


    public void resetPosition()
    {
        // remove force
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // reset joint positions
        transform.localPosition = originalLocalPosition;

        // reset joint rotations
        transform.localRotation = originalLocalRotation;

    }
}
