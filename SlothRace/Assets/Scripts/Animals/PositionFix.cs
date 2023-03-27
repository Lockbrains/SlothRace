using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionFix : MonoBehaviour
{
    [SerializeField] private Transform _slothHip;
    public float offset = 0f;

    // Update is called once per frame
    void Update()
    {
        transform.position = _slothHip.position;
    }
}
