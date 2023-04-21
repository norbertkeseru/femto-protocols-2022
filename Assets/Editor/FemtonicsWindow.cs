using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Környezet : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;
    bool teleporting;
    float teleportDelta;


    // Add menu named "My Window" to the Window menu
    [MenuItem("Femtonics/Környezet")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        Környezet window = (Környezet)EditorWindow.GetWindow(typeof(Környezet));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);
        teleporting = EditorGUILayout.Toggle("Teleporting", PositionTracking.teleporting);
        teleportDelta = EditorGUILayout.FloatField("Teleport Delta", PositionTracking.teleportDelta);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}


public class Funkcionalitás : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    // Add menu named "My Window" to the Window menu
    [MenuItem("Femtonics/Funkcionalitás")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        Funkcionalitás window = (Funkcionalitás)EditorWindow.GetWindow(typeof(Funkcionalitás));
        window.Show();
    }

    void OnGUI()
    {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        myString = EditorGUILayout.TextField("Text Field", myString);

        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
    }
}