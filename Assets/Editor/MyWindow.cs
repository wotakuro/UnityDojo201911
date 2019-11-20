using UnityEngine;
using UnityEditor;

class MyWindow : EditorWindow
{
    [MenuItem("Samples/_OldStyle")]
    public static void Create()
    {
        EditorWindow.GetWindow<MyWindow>();
    }
    // 描画はC#で全部書く
    void OnGUI()
    {
        EditorGUILayout.LabelField("テキスト表示");
        if (GUILayout.Button("決定"))
        {
            // 何か処理する
        }
    }
}
