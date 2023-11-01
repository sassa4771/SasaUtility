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
            // Hierarchy��̂��ׂẴQ�[���I�u�W�F�N�g���擾
            GameObject[] gameObjects = GameObject.FindObjectsOfType<GameObject>();

            foreach (GameObject go in gameObjects)
            {
                // �Q�[���I�u�W�F�N�g����R���|�[�l���g���擾
                Component[] components = go.GetComponents<Component>();

                foreach (Component component in components)
                {
                    // �R���|�[�l���g���̃��\�b�h���擾
                    MethodInfo[] methods = component.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                    foreach (MethodInfo method in methods)
                    {
                        // [Sassa]�������t���Ă��郁�\�b�h��T��
                        if (Attribute.IsDefined(method, typeof(EditorCallMethodAttribute)))
                        {
                            EditorCallMethodAttribute editorCallMethodAttribute = (EditorCallMethodAttribute)Attribute.GetCustomAttribute(method, typeof(EditorCallMethodAttribute));

                            Debug.Log($"component: {component.name}, [EditorCallMethod]�������t���Ă��郁�\�b�h: {method.Name}, ����: {editorCallMethodAttribute.Description}");

                            // Component��Method�̃y�A�����X�g�ɒǉ�
                            componentMethodPairs.Add(new ComponentMethodPair
                            {
                                Component = component,
                                Method = method
                            });
                        }
                    }
                }
            }

            // ���\�b�h���Ăяo��
            foreach (var pair in componentMethodPairs)
            {
                pair.Method.Invoke(pair.Component, null);
            }
        }
    }
}
