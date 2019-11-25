using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;
using UnityEditor;

public class CustomPropertySample : EditorWindow
{
    [MenuItem("Samples/004_CustomProperty")]
    public static void Create()
    {
        EditorWindow.GetWindow<CustomPropertySample>();
    }
    private void OnEnable()
    {
        // uxml 読み込み
        string path = "Assets/Editor/Samples/004/CustomPropertySample.uxml";
        var asset =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        // 読み込んだUXMLをEditorWindowに配置
        asset.CloneTree(this.rootVisualElement);

        //　UXML側にStyleへのリンクがあるので明示的に呼ばなくても良いのですが… 
        string ussPath = "Assets/Editor/Samples/004/CustomPropertySample.uss";
        var stylesheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>(ussPath);

        // CustomStylePropertyを読むところ
        rootVisualElement.Query<Label>(null, "mycustom").ForEach((label) =>
        {
            // CustomStyleResolvedEventに処理する
            // このイベントが飛ぶ前だとCustomStylePropertyが読めない
            label.RegisterCallback<CustomStyleResolvedEvent>(
                (evt) =>
                {
                    // CustomStylePropertyを読むところ
                    int val;
                    var customProperty = new CustomStyleProperty<int>("--data-origin");
                    bool res = label.customStyle.TryGetValue(customProperty, out val);
                    if (res)
                    {
                        label.text = "--data-origin:" + val;
                    }
                    else
                    {
                        label.text = "not found customProperty";
                    }
                });
        });
    }
}
