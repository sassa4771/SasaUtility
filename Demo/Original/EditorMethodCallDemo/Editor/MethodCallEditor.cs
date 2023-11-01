
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
            // ����
            MethodCallEditor window = GetWindow<MethodCallEditor>("MethodCallEditor");
        }

        private void OnGUI()
        {
            // EditorWindow�̃T�C�Y�𒲐�
            float windowHeight = 220f; // �E�B���h�E�̍�����K�X����
            float windowWidth = 400f; // �E�B���h�E�̕���K�X����
            this.minSize = new Vector2(windowWidth, windowHeight);

            // Cube�I�u�W�F�N�g��ǂݍ��ރ{�^�� (���F)
            GUI.backgroundColor = Color.yellow;

            // �I�𒆂̃I�u�W�F�N�g��Hierarchy����I������{�^��
            if (GUILayout.Button("Method���擾����"))
            {
                FindMethod();
            }

            GUI.backgroundColor = Color.white;
            using (new GUILayout.HorizontalScope(EditorStyles.toolbar))
            {
                GUILayout.Label("�擾�������\�b�h");
            }

            //XZ�R���g���[��
            using (new GUILayout.VerticalScope())
            {
                // �擾�������\�b�h��\�����A�{�^���ŌĂяo��
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
            // �{�^�����N���b�N�����Ƃ��̏����������Ɉړ�
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

                            Debug.Log($"component: {component.name}, [EditorCallMethod]�������t���Ă��郁�\�b�h: {method.Name}, ����: {editorCallMethodAttribute.Description}");

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
            // 5�b���ƂɁuMethod���擾����v�{�^�����N���b�N����
            EditorApplication.update += UpdateButtonAutomatically;
        }

        private void OnDisable()
        {
            // �E�B���h�E�������ɂȂ����玩���N���b�N���~����
            EditorApplication.update -= UpdateButtonAutomatically;
        }

        private void UpdateButtonAutomatically()
        {
            // 5�b���ƂɁuMethod���擾����v�{�^�����N���b�N
            if (Time.realtimeSinceStartup % 5.0f < Time.deltaTime)
            {
                FindMethod();
            }
        }
    }
}
