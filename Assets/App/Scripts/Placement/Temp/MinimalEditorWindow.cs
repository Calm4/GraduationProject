using UnityEngine;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

public class MinimalEditorWindow : OdinEditorWindow
{
    [MenuItem("Tools/Minimal Editor")]
    private static void OpenWindow()
    {
        GetWindow<MinimalEditorWindow>().Show();
        Debug.Log("Minimal Editor Window Opened");
    }

    private void OnGUI()
    {
        GUILayout.Label("Minimal Editor Window is open", EditorStyles.boldLabel);
    }
}