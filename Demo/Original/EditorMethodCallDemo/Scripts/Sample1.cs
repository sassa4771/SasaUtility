using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class Sample1 : MonoBehaviour
    {
        [EditorCallMethod("This is a sample1 method.")]
        public void Method1()
        {
            Debug.Log("Sample1 Method1");
        }

        [EditorCallMethod("This is a sample1 method.")]
        public void Method2()
        {
            Debug.Log("Sample1 Method2");
        }
    }
}