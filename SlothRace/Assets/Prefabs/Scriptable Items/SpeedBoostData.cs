using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SpeedBoostData : ScriptableObject
{
    public string itemName = "SpeedBoost";
    public float duration = 4f;
    public float animationSpeed;
    public float movementSpeed;
}
