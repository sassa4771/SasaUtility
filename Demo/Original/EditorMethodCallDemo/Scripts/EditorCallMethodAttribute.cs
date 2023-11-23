using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class EditorCallMethodAttribute : Attribute
    {
        public string Description { get; }

        public EditorCallMethodAttribute(string description)
        {
            Description = description;
        }
    }
}