using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class MethodCallManager : MonoBehaviour
    {
        [Serializable]
        public class ComponentMethodPair
        {
            public Component Component;
            public MethodInfo Method;
        }

        public List<ComponentMethodPair> componentMethodPairs = new List<ComponentMethodPair>();

        private void Start()
        {
            // Hierarchy上のすべてのゲームオブジェクトを取得
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                // ゲームオブジェクトからコンポーネントを取得
                Component[] components = go.GetComponents<Component>();

                foreach (Component component in components)
                {
                    // コンポーネント内のメソッドを取得
                    MethodInfo[] methods = component.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (MethodInfo method in methods)
                    {
                        // [Sassa]属性が付いているメソッドを探す
                        if (Attribute.IsDefined(method, typeof(EditorCallMethodAttribute)))
                        {
                            EditorCallMethodAttribute editorCallMethodAttribute = (EditorCallMethodAttribute)Attribute.GetCustomAttribute(method, typeof(EditorCallMethodAttribute));

                            Debug.Log($"component: {component.name}, [EditorCallMethod]属性が付いているメソッド: {method.Name}, 説明: {editorCallMethodAttribute.Description}");

                            // ComponentとMethodのペアをリストに追加
                            componentMethodPairs.Add(new ComponentMethodPair
                            {
                                Component = component,
                                Method = method
                            });
                        }
                    }
                }
            }

            // メソッドを呼び出す
            foreach (var pair in componentMethodPairs)
            {
                pair.Method.Invoke(pair.Component, null);
            }
        }
    }
}
