using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Animals/New Animal", order = 1)]
[CanEditMultipleObjects]
public class Animal : ScriptableObject
{

    public int speed, size, steadiness;

    public string animalName;
    public string descriptions;
    public string note;

    public string skillTitle;
    public string skillDescriptions;
    
    public Sprite animalProfilePhoto;

    public MeshRenderer animalMesh;

    public MeshCollider meshCollider;

}
