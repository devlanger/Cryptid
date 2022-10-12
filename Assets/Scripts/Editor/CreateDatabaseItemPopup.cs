using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CreateDatabaseItemPopup: EditorWindow
{
    void OnGUI()
    {
        EditorGUILayout.LabelField("This is an example of EditorWindow.ShowPopup", EditorStyles.wordWrappedLabel);
        GUILayout.Space(70);
        if (GUILayout.Button("Agree!")) this.Close();
    }
}