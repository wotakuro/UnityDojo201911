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
      <UTJ.MyVideoElement videoPath="Assets/test.mp4">
      </UTJ.KurokawaElement>
    </ui:UXML>
     */
    // ↑自前で UXMLの要素を追加出来る様子…
    public class MyVideoElement : VisualElement
    {
        // 独自のデータVideoPath
        public string videoPath;

        // UXMLからオブジェクトを生成するためのファクトリー
        public new class UxmlFactory : UxmlFactory<MyVideoElement, UxmlTraits> { }

        // UXMLから要素を抜き出してきてオブジェクトに適用する部分
        public new class UxmlTraits : VisualElement.UxmlTraits
        {
            //ここで独自のAttributeを指定します
            UxmlStringAttributeDescription m_videoPath = 
                new UxmlStringAttributeDescription { name = "videoPath" };

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
                string vPath = m_videoPath.GetValueFromBag(bag, cc);
                MyVideoElement videoElement = (MyVideoElement)ve;
                videoElement.videoPath = vPath;

                // ビデオ部分実装
                VideoClipImporter videoClip = AssetImporter.GetAtPath(vPath) as VideoClipImporter;
                if (videoClip != null) {
                    videoClip.PlayPreview();
                    Background background = videoElement.style.backgroundImage.value;
                    background.texture = videoClip.GetPreviewTexture() as Texture2D;
                    videoElement.style.backgroundImage = background;
                }
                else
                {
                    Debug.LogError("VideoClipが見つかりません\n" + vPath);
                }
            }
        }
    }
}
