using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

// もし xmlの名前をショートカットしたいなら…
// [assembly: UxmlNamespacePrefix("My.Second.Namespace", "second")]


namespace UTJ
{
    /*

    <ui:UXML xmlns:ui="UnityEngine.UIElements" xmlns:uie="UnityEditor.UIElements">
      <ui:Label text="Hello World! From UXML">
      </ui:Label>
      <UTJ.MyAssetPreview assetPath="Assets/test.mp4">
      </UTJ.KurokawaElement>
    </ui:UXML>
     */
    // ↑自前で UXMLの要素を追加出来る様子…
    public class MyAssetPreview : VisualElement
    {
        // 独自のデータAssetPath
        string assetPath;

        // UXMLからオブジェクトを生成するためのファクトリー
        public new class UxmlFactory : UxmlFactory<MyAssetPreview, UxmlTraits> { }

        // UXMLから要素を抜き出してきてオブジェクトに適用する部分
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //ここで独自のAttributeを指定します
            UxmlStringAttributeDescription assetPathAttr = 
                new UxmlStringAttributeDescription { name = "assetPath" };

            // 子要素を持つかどうか
            public override IEnumerable<UxmlChildElementDescription> uxmlChildElementsDescription
            {
                get { yield break; }
            }
            // パース時に呼び出されます
            public override void Init(VisualElement ve, IUxmlAttributes bag, CreationContext cc)
            {
                base.Init(ve, bag, cc);
                // 要素からデータを取ってきます
                string assetPath = assetPathAttr.GetValueFromBag(bag, cc);
                MyAssetPreview assetPreview = (MyAssetPreview)ve;
                assetPreview.assetPath = assetPath;

                // Preview部分実装
                UnityEngine.Object asset = 
                    AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                if (asset != null) {
                    Background background = assetPreview.style.backgroundImage.value;
                    background.texture = AssetPreview.GetAssetPreview(asset);
                    assetPreview.style.backgroundImage = background;
                }
                else
                {
                    Debug.LogError("Assetがみつかりません\n" + assetPath);
                }
            }
        }
    }
}
