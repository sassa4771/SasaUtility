
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace SasaUtility.Demo
{
    public class MethodCallEditor : EditorWindow
    {
        [Serializable]
        public class ComponentMethodPair
        {
            public Component Component;
            public MethodInfo Method;
        }

        public List<ComponentMethodPair> componentMethodPairs = new List<ComponentMethodPair>();

        [MenuItem("SassaUtility/Demo/MethodCallEditor")]
        private static void Create()
        {
            // 生成
            MethodCallEditor window = GetWindow<MethodCallEditor>("MethodCallEditor");
        }

        private void OnGUI()
        {
            // EditorWindowのサイズを調整
            float windowHeight = 220f; // ウィンドウの高さを適宜調整
            float windowWidth = 400f; // ウィンドウの幅を適宜調整
            this.minSize = new Vector2(windowWidth, windowHeight);

            // Cubeオブジェクトを読み込むボタン (黄色)
            GUI.backgroundColor = Color.yellow;

            // 選択中のオブジェクトをHierarchyから選択するボタン
            if (GUILayout.Button("Methodを取得する"))
            {
                FindMethod();
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("取得したメソッド");
            }

            //XZコントローラ
            using (new GUILayout.VerticalScope())
            {
                // 取得したメソッドを表示し、ボタンで呼び出す
                GUI.backgroundColor = Color.white;
                foreach (ComponentMethodPair methodpair in componentMethodPairs)
                {
                    EditorCallMethodAttribute editorCallMethodAttribute = (EditorCallMethodAttribute)Attribute.GetCustomAttribute(methodpair.Method, typeof(EditorCallMethodAttribute));
                    if (GUILayout.Button($"[{methodpair.Component}]\n{methodpair.Method.Name} : {editorCallMethodAttribute.Description}"))
                    {

                        methodpair.Method.Invoke(methodpair.Component, null);
                    }
                }
            }
        }

        private void FindMethod()
        {
            // ボタンをクリックしたときの処理をここに移動
            componentMethodPairs.Clear();

            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                Component[] components = go.GetComponents<Component>();

                foreach (Component component in components)
                {
                    MethodInfo[] methods = component.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (MethodInfo method in methods)
                    {
                        if (Attribute.IsDefined(method, typeof(EditorCallMethodAttribute)))
                        {
                            EditorCallMethodAttribute editorCallMethodAttribute = (EditorCallMethodAttribute)Attribute.GetCustomAttribute(method, typeof(EditorCallMethodAttribute));

                            Debug.Log($"component: {component.name}, [EditorCallMethod]属性が付いているメソッド: {method.Name}, 説明: {editorCallMethodAttribute.Description}");

                            componentMethodPairs.Add(new ComponentMethodPair
                            {
                                Component = component,
                                Method = method
                            });
                        }
                    }
                }
            }
        }

        private void OnEnable()
        {
            // 5秒ごとに「Methodを取得する」ボタンをクリックする
            EditorApplication.update += UpdateButtonAutomatically;
        }

        private void OnDisable()
        {
            // ウィンドウが無効になったら自動クリックを停止する
            EditorApplication.update -= UpdateButtonAutomatically;
        }

        private void UpdateButtonAutomatically()
        {
            // 5秒ごとに「Methodを取得する」ボタンをクリック
            if (Time.realtimeSinceStartup % 5.0f < Time.deltaTime)
            {
                FindMethod();
            }
        }
    }
}
