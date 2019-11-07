using UnityEditor;
using UnityEngine.UIElements;

public class NewUICs : EditorWindow

{
    [MenuItem("Samples/001_NewUI")]
    public static void Create()
    {
        EditorWindow.GetWindow<NewUICs>();
    }
    void OnEnable()
    {
        // uxml 読み込み
        string path = "Assets/Editor/Samples/001/NewUI.uxml";
        var asset = 
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        // 読み込んだUXMLをEditorWindowに配置
        asset.CloneTree(this.rootVisualElement);
    }
}
