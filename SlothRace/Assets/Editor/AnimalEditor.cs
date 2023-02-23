using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Animal))]
[CanEditMultipleObjects]
public class AnimalEditor : Editor
{
    private Animal _target;
    
    private int _currentlySelected = 0;
    
    // Creating Animal Data
    
    private string _hint;
    
    private bool _showData;
    private bool _showSkills;

    
    private void OnEnable()
    {
        _target = base.target as Animal;
    }

    public override void OnInspectorGUI()
    {
        string[] tabs = new string[2] { "Text Information", "Art Assets"};
        _currentlySelected = GUILayout.Toolbar(_currentlySelected, tabs);
        if (_currentlySelected == 0)
        {
            // in text information
            DrawTextInformation();
        }
        else
        {
            DrawArtAssets();
        }
        
        if (GUI.changed)
        {
            EditorUtility.SetDirty(_target);
        }
        
    }

    private void DrawTextInformation()
    {
        _showData = EditorGUILayout.BeginFoldoutHeaderGroup(_showData, "Data");
        if (_showData)
        {
            _target.animalName = EditorGUILayout.TextField("Name", _target.animalName);
            EditorGUILayout.Space(30);
            _hint =
                "Speed describes how fast the animal moves(relative to their size).\nSo even though ants can move fast, they are moving similar distance in the unit time compared to, for example, a sloth.";
            EditorGUILayout.HelpBox(_hint, MessageType.Info);
            EditorGUILayout.HelpBox("Tony's comment: Since there can be significant subjective biases on choosing the level, I will limit the following fields only from 1 to 5.", MessageType.Info);
            _target.speed = EditorGUILayout.IntSlider("Speed", _target.speed, 1, 5);
            _target.size = EditorGUILayout.IntSlider("Size", _target.size, 1, 5);
            _target.steadiness = EditorGUILayout.IntSlider("Steadiness", _target.steadiness, 1, 5);
  
            EditorGUILayout.Space(30);
        }
        
        EditorGUILayout.EndFoldoutHeaderGroup();
        

        _showSkills = EditorGUILayout.BeginFoldoutHeaderGroup(_showSkills, "Skill");
        if (_showSkills)
        {
            EditorGUILayout.HelpBox("Tony's comment: Let's keep the title in only one verb. For example, \"Roll\"", MessageType.Info);
            _target.skillTitle = EditorGUILayout.TextField("Skill Title", _target.skillTitle);
            EditorGUILayout.LabelField("Skill Description");
            _target.skillDescriptions = EditorGUILayout.TextArea(_target.skillDescriptions, GUILayout.Height(100));
            EditorGUILayout.LabelField("Note");
            _target.note = EditorGUILayout.TextArea(_target.note, GUILayout.Height(300));
            
        }
        
    }

    private void DrawArtAssets()
    {
        EditorGUILayout.HelpBox("This is the photo that's going to be shown at the character selection interface.", MessageType.Info);
        EditorGUILayout.LabelField("Profile Photo");
        _target.animalProfilePhoto = EditorGUILayout.ObjectField(_target.animalProfilePhoto, typeof(Sprite), true,
            GUILayout.Height(300), GUILayout.Width(300)) as Sprite;
        EditorGUILayout.Space(30);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Mesh");
        _target.animalMesh = EditorGUILayout.ObjectField(_target.animalMesh, typeof(MeshRenderer), true) as MeshRenderer;
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Collider");
        _target.meshCollider = EditorGUILayout.ObjectField(_target.meshCollider, typeof(MeshCollider), true) as MeshCollider;
        EditorGUILayout.EndHorizontal();
    }
}
