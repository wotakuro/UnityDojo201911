using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

public class CustomElementSample : EditorWindow
{
    [MenuItem("Samples/005_CustomElement")]
    public static void Create()
    {
        EditorWindow.GetWindow<CustomElementSample>();
    }
    private void OnEnable()
    {
        // uxml 読み込み
        string path = "Assets/Editor/Samples/005/CustomElementSample.uxml";
        var asset =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        // 読み込んだUXMLをEditorWindowに配置
        asset.CloneTree(this.rootVisualElement);

    }
}