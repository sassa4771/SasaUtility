using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using System;
using System.IO;

namespace SasaUtility.Demo.Original
{
    /// <summary>
    /// VideoPlayer???g?p???????????t???[????PNG???????????N???X
    /// </summary>
    public class VideoFrameSplitter : MonoBehaviour
    {
        public static VideoFrameSplitter instance { get { return Instance; } }
        private static VideoFrameSplitter Instance;

         public int splitImage = 32;
        public bool isReady { get { return IsReady; } }
        [SerializeField] private bool ShowImage = false;
        [SerializeField] private RawImage rawImage; //?f?????\??????RawImage

        public VideoPlayer vp;
        public int maxFrame = 0;
        private bool seekDone = true;
        private bool IsReady = false;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);

            vp = this.GetComponent<VideoPlayer>();
        }

        void Start()
        {
            vp.prepareCompleted += OnVideoPrepared;
            vp.Prepare();
            vp.seekCompleted += seekCompleted;
        }

        /// <summary>
        /// ?t???[?????X?????????????????\?b?h
        /// </summary>
        /// <param name="par"></param>
        void seekCompleted(VideoPlayer par)
        {
            StartCoroutine(WaitToUpdateRenderTextureBeforeEndingSeek());
        }

        /// <summary>
        /// ?t???[?????X?????????????????o?????????\?b?h
        /// </summary>
        /// <returns></returns>
        IEnumerator WaitToUpdateRenderTextureBeforeEndingSeek()
        {
            yield return new WaitForEndOfFrame();
            //Debug.Log("Do Seek True");
            seekDone = true;
        }

        /// <summary>
        /// videoPlayer?????????????????????o?????????\?b?h
        /// </summary>
        /// <param name="source"></param>
        private void OnVideoPrepared(VideoPlayer source)
        {
            //???????????t???[????????
            maxFrame = (int)vp.frameCount;
            Debug.Log("Max Frame: " + maxFrame);

            if (ShowImage) rawImage.texture = source.texture;
            source.Play();
            source.Pause();
            IsReady = true;
        }

        /// <summary>
        /// ???????t???[????????????png?????????????????\?b?h
        /// </summary>
        /// <returns></returns>
        public IEnumerator GetFirstFrame(string folderPath, UnityAction callback = null)
        {
            if (vp.url != "")
            {
                //???????I??????????????????
                while (!IsReady) yield return null;

                if (seekDone)
                {
                    vp.frame = 0;
                    vp.Pause();
                    seekDone = false;
                    yield return new WaitForSeconds(1);
                    string path = SaveVideoPlayerFrame(Application.persistentDataPath + "/" + folderPath);

                    Debug.Log("file://" + path);
                    if (callback != null) callback.Invoke();
                }
            }
            else
            {
                Debug.LogError("videoPlayer??url?????????Z?b?g???????????????B");
            }
        }

        /// <summary>
        /// videoPlayer????????splitImage?????????????????Apng??????????
        /// </summary>
        /// <returns></returns>
        public IEnumerator VideoSplit(string folderPath, UnityAction callback = null)
        {
            if (vp.url != "")
            {
                //???????I??????????????????
                while (!IsReady) yield return null;

                string[] savePath = new string[splitImage];
                string previousImage = null;

                int count = 0;
                int frameCount = 0;
                int previosFrame = -1;

                //Debug.Log(maxFrame / 32);
                while(count < splitImage) {
                    if (previousImage == null || File.Exists(previousImage))
                    {
                        if (seekDone)
                        {
                            vp.frame = frameCount;
                            // You should pause while you seek for better stability
                            vp.Pause();
                            seekDone = false;

                            if (vp.frame == previosFrame) vp.frame = frameCount + 1;
                            if (vp.frame == 0) vp.frame = 1;
                             yield return null;

                            if (vp.frame != previosFrame)
                            {
                                previousImage = SaveVideoPlayerFrame(folderPath);
                                Debug.Log("frameCount: " + frameCount + ", frame: " + vp.frame + ", previous: " + previosFrame);
                                count++;
                                previosFrame = frameCount;
                                frameCount += maxFrame / splitImage;
                            }
                        }
                    }
                    yield return null;
                }

                if (callback != null) callback.Invoke();
            }
            else
            {
                Debug.LogError("videoPlayer??url?????????Z?b?g???????????????B");
            }
}

        /// <summary>
        /// ??????videoPlayer??Frame??png?????????????????\?b?h
        /// </summary>
        /// <returns>?????????p?X?????p</returns>
        string SaveVideoPlayerFrame(string folderPath)
        {
            Texture2D tex = new Texture2D((int)vp.texture.width, (int)vp.texture.height, TextureFormat.RGB24, false);
            RenderTexture.active = (RenderTexture)vp.texture;
            tex.ReadPixels(new Rect(0, 0, tex.width, tex.height), 0, 0);
            tex.Apply();

            string savePath = PathController.GetSavePath(folderPath, "png", false);
            Texture2Png.ConvertToPngAndSave(savePath, tex);

            Debug.Log(vp.frame);

            Destroy(tex);

            return savePath;
        }

        /// <summary>
        /// videoPlayer???????????????????\?b?h
        /// </summary>
        /// <param name="urlPath"></param>
        public void SetVideo(string urlPath)
        {
            vp.url = "file://" + urlPath;
            Debug.Log(vp.url);
            IsReady = false;
        }
    }
}