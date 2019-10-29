using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class PresentationWindow : EditorWindow
{
    [MenuItem("Tools/Presentation")]
    public static void Create()
    {
        EditorWindow.GetWindow<PresentationWindow>();
    }

    private Rect oldRect = new Rect();

    private void OnEnable()
    {
        Reload();
    }
    VisualElement root;

    private void Reload()
    {
        this.rootVisualElement.Clear();
        string path = "Assets/Editor/Slides/Presentation.uxml";
        var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        root = asset.CloneTree();

        this.rootVisualElement.Add(root);        
    }

    public void Update()
    {
        var rect = this.rootVisualElement.contentRect;

        if(oldRect == rect)
        {
            return;
        }
        var labels = this.rootVisualElement.Query<Label>().
            ForEach((element) =>
            {
                if(element.userData == null)
                {
                    element.userData = element.resolvedStyle.fontSize;
                }
                float originSize = (float)element.userData;
                element.style.fontSize = originSize * rect.width / 960.0f;
                Debug.Log("originFont " + originSize + "->" + element.style.fontSize);
                element.MarkDirtyRepaint();
                return element;
            });
        

        root.style.width = rect.width;
        root.style.height = rect.height;
        oldRect = rect;
    }
}
