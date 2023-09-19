using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SasaUtility.Demo
{
    public class FrameSplitterUI : MonoBehaviour
    {
        VideoFrameSplitter frameSplitter;
        protected const string IMAGE_SAVE_FOLDER = "Image";
        public string filepath = "";

        // Start is called before the first frame update
        void Start()
        {
            frameSplitter = VideoFrameSplitter.instance;
            frameSplitter.SetVideo(filepath);
        }

        public void OnClickStartSplite()
        {
            StartCoroutine(frameSplitter.VideoSplit(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER));
            System.Diagnostics.Process.Start(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER);
        }

        public void GetThumbnail()
        {
            StartCoroutine(frameSplitter.GetFirstFrame(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER));
            System.Diagnostics.Process.Start(Application.persistentDataPath + "/" + IMAGE_SAVE_FOLDER);
        }

    }
}