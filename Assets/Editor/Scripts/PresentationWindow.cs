﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace UTJ
{
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
        private List<VisualTreeAsset> pageAssets;

        private int currentPage;


        private void OnEnable()
        {
            Reload();
        }

        private void Reload()
        {
            currentPage = 0;
            currentWindowRect = new Rect(0, 0, 0, 0);
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
                button.clickable.clicked += () =>
                {
                    this.NextPage();
                };
            });
            // nextPage
            root.Query<Button>("PrevPage").ForEach((button) =>
            {
                button.clickable.clicked += () =>
                {
                    this.PrevPage();
                };
            });
            root.focusable = true;
            root.RegisterCallback<KeyDownEvent>((evt) =>
            {
                if( evt.keyCode == KeyCode.RightArrow)
                {
                    this.NextPage();
                }else if( evt.keyCode == KeyCode.LeftArrow)
                {
                    this.PrevPage();
                }
            });
            this.rootVisualElement.Add(root);
            // load first page
            this.pageAssets = GetSlideAssets();
            ChangePageNumber();
        }
        void NextPage()
        {
            if (this.currentPage < this.pageAssets.Count - 1)
            {
                this.currentPage++;
                ChangePageNumber();
            }
        }
        void PrevPage()
        {
            if (this.currentPage > 0)
            {
                this.currentPage--;
                ChangePageNumber();
            }
        }

        private List<VisualTreeAsset> GetSlideAssets()
        {
            List<VisualTreeAsset> assets = new List<VisualTreeAsset>();
            var guids = AssetDatabase.FindAssets("t:VisualTreeAsset Page");
            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(path);
                assets.Add(asset);
            }
            return assets;
        }

        private void ChangePageNumber()
        {

            if (currentElement != null)
            {
                currentElement.parent.Remove(currentElement);
            }

            currentElement = pageAssets[currentPage ].CloneTree();

            ScalePage(this.currentWindowRect, currentElement);
            root.Insert(0, currentElement);
            root.Query<Label>("PageNumber").ForEach((label) =>
            {
                label.text = (this.currentPage + 1).ToString();
            });
            //
            root.Query<Button>("NextPage").ForEach((button) =>
            {
                button.visible = (this.currentPage < this.pageAssets.Count - 1);
            });

            //
            root.Query<Button>("PrevPage").ForEach((button) =>
            {
                button.visible = (this.currentPage > 0 );
            });
        }

        public void Update()
        {
            var rect = this.rootVisualElement.contentRect;

            if (currentWindowRect == rect)
            {
                return;
            }
            if (float.IsNaN(rect.width) || float.IsNaN(rect.height))
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
        private void ScalePage(Rect rect, VisualElement visualElement)
        {
            const float expectedWidth = 960.0f;
            const float expectedHeight = 540.0f;
            const float expectedAspect = expectedWidth / expectedHeight;
            visualElement.Query<VisualElement>(null, "page").
                ForEach((element) =>
                {
                    if (rect.width == float.NaN || rect.height == float.NaN)
                    {
                        return;
                    }
                    Vector2 offset = new Vector2();
                    float scaleRate = (float)rect.width / expectedWidth;
                    if ((float)rect.width / (float)rect.height < expectedAspect)
                    {
                        // 縦長
                        scaleRate = (float)rect.width / expectedWidth;
                        offset.y = (rect.height - expectedHeight * scaleRate) * 0.5f;
                    }
                    else
                    {
                        // 横長
                        scaleRate = (float)rect.height / expectedHeight;
                        offset.x = (rect.width - expectedWidth * scaleRate) * 0.5f;
                    }
                    element.transform.position = new Vector3(offset.x, offset.y, 0.0f);
                    element.transform.scale = new Vector3(scaleRate, scaleRate, 1);
                });
        }
    }
}