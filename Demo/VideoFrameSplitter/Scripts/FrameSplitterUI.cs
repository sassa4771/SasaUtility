using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class FrameSplitterUI : MonoBehaviour
    {
        VideoFrameSplitter frameSplitter;
        protected const string IMAGE_SAVE_FOLDER = "Image";

        // Start is called before the first frame update
        void Start()
        {
            frameSplitter = VideoFrameSplitter.instance;
            frameSplitter.SetVideo("C:/Users/hurob/Documents/Unity/TernTableApp_2022.05.27/Assets/Plugins/SasaUtility.v1.1.0/Demo/VideoFrameSplitter/Videos/SampleVideo.mp4");
        }

        public void OnClickStartSplite()
        {
            StartCoroutine(frameSplitter.VideoSplit(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER));
        }

        public void GetThumbnail()
        {
            StartCoroutine(frameSplitter.GetFirstFrame(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER));
        }

    }
}