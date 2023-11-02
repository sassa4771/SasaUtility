using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ImageSample
{
    public class HowToUseIt : MonoBehaviour
    {
        private ExternalImageManager eim;
        public GameObject ParentObject;

        // Start is called before the first frame update
        void Start()
        {
            eim = new ExternalImageManager("Image", "SampleImage.png", ParentObject);
            eim.InstanceImageObject();
        }
    }
}