using UnityEditor;
using UnityEngine.UIElements;


public class IMGUIContainerSample : EditorWindow
{
    [MenuItem("Samples/003_IMGUIContainerSample")]
    public static void Create()
    {
        EditorWindow.GetWindow<IMGUIContainerSample>();
    }
    void OnEnable()
    {
        // uxml 読み込み
        string path = "Assets/Editor/Samples/003/IMGUIContainerSample.uxml";
        var asset =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        // 読み込んだUXMLをEditorWindowに配置
        asset.CloneTree(this.rootVisualElement);
        //　IMGUIContainerをScrollViewの中にくっつけます
        var element = new IMGUIContainer(this.IMGUIExecute);
        this.rootVisualElement.Query<ScrollView>().First().Add(element);
    }
    // OnGUIだと別で呼ばれてしまうので名前を変更…
    void IMGUIExecute()
    {
        for (int i = 0; i < 15; ++i)
        {
            EditorGUILayout.LabelField("IMGUIのコンテンツ(" + i +")");
        }
    }
}
