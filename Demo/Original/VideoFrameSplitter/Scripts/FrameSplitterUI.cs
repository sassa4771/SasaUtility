using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

namespace SasaUtility.Demo.Original
{
    public class FrameSplitterUI : MonoBehaviour
    {
        [SerializeField] private Button OneFrameButton;
        [SerializeField] private Button SpliteButton;
        VideoFrameSplitter frameSplitter;
        protected const string IMAGE_SAVE_FOLDER = "Image";
        public string filepath = "";    // Plugins\SasaUtility\Demo\Original\VideoFrameSplitter\Videos\SampleVideo.mp4

        void Start()
        {
            OneFrameButton.onClick.AsObservable().Subscribe(_ => GetThumbnail()).AddTo(this);
            SpliteButton.onClick.AsObservable().Subscribe(_ => OnClickStartSplit()).AddTo(this);

            frameSplitter = VideoFrameSplitter.instance;
            frameSplitter.SetVideo(Application.dataPath + "/" + filepath);
        }

        public void OnClickStartSplit()
        {
            StartCoroutine(SplitVideoAndOpenFolder());
        }

        public void GetThumbnail()
        {
            StartCoroutine(GetFirstFrameAndOpenFolder());
        }

        IEnumerator SplitVideoAndOpenFolder()
        {
            string outputPath = Path.Combine(Application.persistentDataPath, IMAGE_SAVE_FOLDER + "/" + PathController.GetDateTimeFileName());
            yield return StartCoroutine(frameSplitter.VideoSplit(outputPath));

            // サムネイルが生成された後にフォルダを開く
            OpenFolder(outputPath);
        }

        IEnumerator GetFirstFrameAndOpenFolder()
        {
            string outputPath = Path.Combine(Application.persistentDataPath, IMAGE_SAVE_FOLDER + "/" + PathController.GetDateTimeFileName());
            UnityEngine.Debug.Log(outputPath);

            yield return StartCoroutine(frameSplitter.GetFirstFrame(outputPath));

            // サムネイルが生成された後にフォルダを開く
            OpenFolder(outputPath);
        }

        void OpenFolder(string folderPath)
        {
            // フォルダが存在するか確認
            if (Directory.Exists(folderPath))
            {
                // フォルダを開く
                Process.Start(folderPath);
            }
            else
            {
                UnityEngine.Debug.LogError("フォルダが見つかりません: " + folderPath);
            }
        }

    }
}