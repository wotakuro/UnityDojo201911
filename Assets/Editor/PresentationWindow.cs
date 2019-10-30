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

    private VisualElement root;
    private VisualElement currentElement;
    private Rect currentWindowRect = new Rect();

    private int currentPage;
    

    private void OnEnable()
    {
        Reload();
    }

    private void Reload()
    {
        Debug.Log("Reload!!!");
        currentWindowRect = new Rect(0,0,0,0);
        this.rootVisualElement.Clear();
        string path = "Assets/Editor/Slides/Presentation.uxml";
        var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        root = asset.CloneTree();

        // register Reload Button
        root.Query<Button>("Reload").ForEach((button) =>
        {
            button.clickable.clicked += Reload;
        });
        this.rootVisualElement.Add(root);
        // nextPage
        root.Query<Button>("NextPage").ForEach((button) =>
        {
            button.clickable.clicked += ()=>{
                this.currentPage++;
                ChangePageNumber();
            };
        });

        // nextPage
        root.Query<Button>("PrevPage").ForEach((button) =>
        {
            button.clickable.clicked += () => {
                this.currentPage--;
                ChangePageNumber();
            };
        });
        // label
        this.rootVisualElement.Add(root);
    }

    private List<VisualTreeAsset> GetSlideAssets()
    {
        List<VisualTreeAsset> assets = new List<VisualTreeAsset>();
        var guids = AssetDatabase.FindAssets("t:VisualTreeAsset Page");
        foreach( var guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
            assets.Add(asset);
        }
        return assets;
    }

    private void ChangePageNumber()
    {
        var assets = GetSlideAssets();

        if( currentElement != null)
        {
            currentElement.parent.Remove(currentElement);
        }

        currentElement = assets[currentPage-1].CloneTree();

        ScalePage(this.currentWindowRect, currentElement);
        root.Insert(0, currentElement);
        root.Query<Label>("PageNumber").ForEach((label) =>
        {
            label.text = this.currentPage.ToString();
        });
    }


    public void Update()
    {
        var rect = this.rootVisualElement.contentRect;

        if(currentWindowRect == rect)
        {
            return;
        }
        if(float.IsNaN( rect.width ) || float.IsNaN(rect.height))
        {
            currentWindowRect = rect;
            return;
        }

        // page scaling
        ScalePage(rect, this.rootVisualElement);
        root.style.width = rect.width;
        root.style.height = rect.height;
        currentWindowRect = rect;
    }
    // page scaling
    private void ScalePage(Rect rect,VisualElement visualElement)
    {
        visualElement.Query<VisualElement>(null, "page").
            ForEach((element) =>
            {
                if (rect.width == float.NaN || rect.height == float.NaN)
                {
                    return;
                }
                element.transform.scale = new Vector3((float)rect.width / 960.0f, (float)rect.height / 540.0f, 1);
            });
    }
}
