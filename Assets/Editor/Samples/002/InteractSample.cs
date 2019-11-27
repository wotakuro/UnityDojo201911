using UnityEditor;
using UnityEngine.UIElements;

public class InteractSample : EditorWindow

{
    [MenuItem("Samples/002_InteractSample")]
    public static void Create()
    {
        EditorWindow.GetWindow<InteractSample>();
    }
    void OnEnable()
    {
        // uxml 読み込み
        string path = "Assets/Editor/Samples/002/InteractSample.uxml";
        var asset =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
        // 読み込んだUXMLをEditorWindowに配置
        asset.CloneTree(this.rootVisualElement);

        // EditorWindow下にある「AddBtn」という
        // 名前のボタンに対して処理をします
        rootVisualElement.Query<Button>("AddBtn").
            ForEach((button) => {
#if UNITY_2019_3_OR_NEWER
                button.clicked += this.OnClickAddBtn;
#else
                button.clickable.clicked += this.OnClickAddBtn;
#endif
            });

        // 同様に「DelBtn」にも処理を追加します
        rootVisualElement.Query<Button>("DelBtn").
            ForEach((button) =>
            {
#if UNITY_2019_3_OR_NEWER
                button.clicked += this.OnClickDelBtn;
#else
                button.clickable.clicked += this.OnClickDelBtn;
#endif
            });
     }


    // 追加するボタンが押されたときに呼び出されます
        void OnClickAddBtn()
    {
        // TextField
        TextField textFiled = 
            rootVisualElement.Query<TextField>().AtIndex(0);
        string textValue = textFiled.value;
        // Labelを動的に生成します
        var newLineLabel = new Label(textValue);
        // 削除時にQueryで見つけるように myitemクラスを追加します
        newLineLabel.AddToClassList("myitem");
        // 「ScrollList」という名前のScrollViewに
        // 生成したLabelを追加します
        rootVisualElement.Query<ScrollView>("ScrollList").
            AtIndex(0).Add(newLineLabel);
    }

    // 削除するボタンが押されたとき
    void OnClickDelBtn()
    {
        // ScrollViewを取得します
        ScrollView scrollView = 
            rootVisualElement.Query<ScrollView>("ScrollList").
            AtIndex(0);
        //ScrollViewの中にあるmyitemを持つモノを探してきて…
        //最後の要素をピックアップします
        var element = 
            scrollView.Query<VisualElement>(null,"myitem").Last();
        // 要素があるようなら…
        if (element != null)
        {
            // 該当要素をparentからRemoveして削除します
            element.parent.Remove(element);
        }
    }
}
