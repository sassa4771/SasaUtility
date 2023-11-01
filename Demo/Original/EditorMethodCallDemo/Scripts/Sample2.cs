using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class Sample2 : MonoBehaviour
    {
        [EditorCallMethod("This is a sample2 method1.")]
        public void Method1()
        {
            Debug.Log("Sample2 Method1");
        }

        [EditorCallMethod("This is a sample2 method2.")]
        public void Method2()
        {
            Debug.Log("Sample2 Method2");
        }
    }
}