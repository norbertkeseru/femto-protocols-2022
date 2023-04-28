using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class Környezet : EditorWindow
{
    [MenuItem("Femtonics/Environment")]
    static void Init()
    {
        Környezet window = (Környezet)GetWindow(typeof(Környezet));
        window.minSize = new Vector2(600, 300);
        window.Show();
    }

    void OnGUI()
    {
        //Creates new scene
        string SceneName = EditorGUILayout.TextField()
        if (GUILayout.Button("New Scene", GUILayout.Width(90)))
        {
            var newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            EditorSceneManager.SaveScene(newScene, "Assets/Scenes/"+ SceneName + ".unity");

        }
    }
}


public class Functionality : EditorWindow
{
    string myString = "Hello World";
    bool groupEnabled;
    bool myBool = true;
    float myFloat = 1.23f;

    [MenuItem("Femtonics/Functionality")]
    static void Init()
    {
        Functionality window = (Functionality)GetWindow(typeof(Functionality));
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